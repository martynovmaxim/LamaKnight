using System;
using System.Data;
using System.Threading;
using System.Media;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;


namespace LamaKnight
{
    public class State
    {
        public List<string> Description;
        public List<string> Options;
        public List<State> States;

        public State()
        {
            Description = new List<string>();
            Options = new List<string>();
            States = new List<State>();
        }

        public int ProceedInput(int maxVar = 3)
        {
            ConsoleKey keyPressed = new ConsoleKey();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                    if (maxVar >= 1) return 1;
                    else break;
                case ConsoleKey.D2:
                    if (maxVar >= 2) return 2;
                    else break;
                case ConsoleKey.D3:
                    if (maxVar >= 3) return 3;
                    else break;
                default:
                    break;
            }
            return ProceedInput();
        }


        public virtual void Enter()
        {
            Console.Clear();
            Console.CursorVisible = false;
            foreach (string line in Description) Console.WriteLine(line);
            Console.WriteLine();
            foreach (string line in Options) Console.WriteLine(line);
            int input = ProceedInput(States.Count);
            States[input - 1].Enter();
        }
    }

    class EndState : State
    {
        public EndState() : base()
        {

        }

        public override void Enter()
        {

        }
    }

    public class StateMachine
    {
        public List<State> States;

        public StateMachine()
        {
            States = new List<State>();
        }

        public StateMachine(int num)
        {
            States = new List<State>();
            for (int i = 0; i < num; i++)
            {
                State buf = new State();
                States.Add(buf);
            }
        }

        public void Add(State newState)
        {
            States.Add(newState);
        }

        public void Run()
        {
            States[0].Enter();
        }

    }

    public class Quiz
    {
        public string question;
        public List<string> answers;
        public int rightAnswer;

        public Quiz()
        {
            question = "";
            answers = new List<string>();
            rightAnswer = 0;
        }
    }

    public class RoomBase
    {
        public List<string> image;
        public List<string> text;
        public List<string> interactions;
        public RoomBase nextRoom;

        public string DefaultPath = "";

        public string entryAnimPath;
        public List<List<string>> entryAnimation;
        public int entryDuration = 1500;
        public string entrySound;

        public string treasureAnimPath = "Chest/Open.txt";
        public List<List<string>> treasureAnim;
        public string treasureSound = "Treasure.wav";

        public string PlayerDamagedAnimPath;
        public List<List<string>> PlayerDamagedAnim;
        public string PlayerDamagedSound = "PlayerDamaged.wav";

        public string PlayerDeathScreenPath = "you_died.txt";
        public List<List<string>> PlayerDeathScreenAnim;
        public string PlayerDeathScreenSound = "PlayerDeathScreen.wav";

        //pathes


        public Program game;

        public RoomBase() : base()
        {
            text = new List<string>();
            interactions = new List<string>();
            image = new List<string>();
        }
        public RoomBase(Program _game)
        {
            game = _game;
            game.rooms.Add(this);    // MAY CAUSE PROBLEMS
            SetDefaults();
            entryAnimation = LoadAnimation(entryAnimPath);
            treasureAnim = LoadAnimation(treasureAnimPath);
            PlayerDamagedAnim = LoadAnimation(PlayerDamagedAnimPath);
            PlayerDeathScreenAnim = LoadAnimation(PlayerDeathScreenPath);
        }

        public virtual void SetDefaults()
        {
            entrySound = DefaultPath + "Entry.wav";
            entryAnimPath = DefaultPath + "Entry.txt";
            PlayerDamagedAnimPath = DefaultPath + "PlayerDamaged.txt";
        }

        public virtual void PlayAnimation(List<List<string>> animation, int duration = 1500, bool loop = false, int maxCount = 3, int count = 0)
        {
            foreach (List<string> frame in animation)
            {
                game.mainImage = frame;
                game.UpdateFrame();
                Thread.Sleep(duration / animation.Count);
            }
            if (loop && (count <= maxCount))
            {
                //Console.WriteLine(count);
                PlayAnimation(animation, duration, loop, maxCount, count += 1);
            }
            else
            {
                Console.Clear();
                game.UpdateFrame();
                return;
            }
        }

        public void PlayEntryAnimation(bool loop = false, int duration = 1500, int maxCount = 3, int count = 0)
        {
            game.PlayFX(entrySound);
            PlayAnimation(entryAnimation, entryDuration, loop, maxCount, count);
        }

        public virtual void LoadEntryAnimation(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                List<string> buffer = new List<string>();
                while (!reader.EndOfStream)
                {
                    buffer.Add(reader.ReadLine());
                }
                for (int i = 0; i < buffer.Count; i += 54)
                {
                    entryAnimation.Add(buffer.GetRange(i, Math.Min(54, buffer.Count - i)));
                }
                image = entryAnimation[entryAnimation.Count - 1];
            }
        }

        public List<List<string>> LoadAnimation(string path)
        {
            if (!File.Exists(path)) return new List<List<string>>();
            using (StreamReader reader = new StreamReader(path))
            {
                List<string> bufferFrame = new List<string>();
                List<List<string>> bufferAnim = new List<List<string>>();

                while (!reader.EndOfStream)
                {
                    bufferFrame.Add(reader.ReadLine());
                }
                for (int i = 0; i < bufferFrame.Count; i++)
                {
                    if (bufferFrame[i].Length > 2) bufferFrame[i] = bufferFrame[i].Remove(bufferFrame[i].Length - 1);
                    else bufferFrame[i] += "    ";
                }
                for (int i = 0; i < bufferFrame.Count; i += 54)
                {
                    bufferAnim.Add(bufferFrame.GetRange(i, Math.Min(54, bufferFrame.Count - i)));
                }
                return bufferAnim;
            }
        }

        public virtual void Enter()
        {
            game.mainImage = image;
            game.textOutput = text;
            game.interactive = interactions;
            game.currentRoom = this;
            game.player.health += 3;
            PlayEntryAnimation();
        }

        public static int FindLast(List<int> array, int match)
        {
            int index = -1;
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] == match) index = i;
            }
            //Console.WriteLine(index);
            return index;
        }
        public virtual bool haveEnemy() { return false; }

        public void PlayTreasureAnimation()
        {
            game.PlayFX(treasureSound);
            PlayAnimation(treasureAnim);
        }

        public void PlayPlayerDamagedAnimation()
        {
            game.PlayFX(PlayerDamagedSound);
            PlayAnimation(PlayerDamagedAnim, 250);
        }

        public void ShowDeathScreen()
        {
            //There should go something like Press 1 to restart... Should create restart function...
            game.PlayFX(PlayerDeathScreenSound);
            PlayAnimation(PlayerDeathScreenAnim);
        }

        public virtual void showLevelUp()
        {
            Console.Clear();
            game.textOutput = new List<string> { "With great victory comes GREAT REWARD", "You see a great full of treasures chest openes", "What you will choose?" };
            game.interactive = new List<string> { "[1] Sword (Strength + 1)", "[2] Great wand (Inteligency +1)", "[3] Great shield (Defence +1)" };
            PlayTreasureAnimation();
            int input = game.ProceedInput();
            game.player.LevelUp(input - 1);
            TransitionRoom(nextRoom); //ROOM TRANSITION
        }

        public virtual void TransitionRoom(RoomBase newRoom)
        {
            game.PlayFX("Transition.wav");
            Console.Clear();
            Thread.Sleep(1800);
            newRoom.Enter();
        }

    }

    public class QuizRoom : RoomBase
    {
        public Quiz quiz;
        bool correctAnswer = false;
        public QuizRoom() : base() { }

        public QuizRoom(Program _game) : base(_game) 
        {
            
        }

        public override void SetDefaults()
        {
            DefaultPath = "Quiz/";
            base.SetDefaults();
            //entryAnimPath = "Quiz/Entry.txt";

        }

        public override void Enter()
        {
            text = new List<string> {"", "", ""};
            interactions = new List<string> { quiz.question, quiz.answers[0], quiz.answers[1], quiz.answers[2]};
            base.Enter();
            int input = game.ProceedInput();
            if (input == quiz.rightAnswer)
            {
                game.player.LevelRandomUp();
                correctAnswer = true;
                game.PlayFX("Quiz/QuizCorrect.wav");
                PlayAnimation(LoadAnimation("Quiz/Open.txt"));
                game.textOutput = new List<string> {"CORRECT!", "Interesting... How such a stupid creature could know this?..", "Whatever, I keep my words, so you may enter this time..." };
            }
            else
            {
                game.player.TakeDamage(5);
                game.PlayFX("Quiz/QuizWrong.wav");
                correctAnswer = false;
                game.textOutput = new List<string> { "WRONG!", "Of course such a stupid creature cannot knew those very basics facts", "I will teleport you somewhere!" };
            }

            game.interactive = new List<string> {"Press any button to leave"};
            Console.Clear();
            game.UpdateFrame();
            input = game.ProceedInput();
            if (correctAnswer) TransitionRoom(nextRoom); //ROOM TRANSITION
            else TransitionRoom(nextRoom); //ROOM TRANSITION
        }
    }

    public class RoomWithEnemy : RoomBase
    {
        public EnemyBase enemy;
        public List<int> enemiesStats = new List<int> {10, 10, 10};
        public int enemiesHealth = 50;
        public float enemiesMultiplier = 0.5f;

        public string enemyDeathAnimPath;
        public List<List<string>> enemyDeathAnim;
        public int deathAnimDuration = 1500;
        public string EnemyDeathSound;

        public string enemyGetDamageAnimPath;
        public List<List<string>> enemyGetDamageAnim;
        public int enemyGetDamageAnimDuration = 500;
        public string enemyGetDamageSound;

        public RoomWithEnemy() : base()
        {
            enemy = new EnemyBase();
            enemyDeathAnim = LoadAnimation(enemyDeathAnimPath);
            enemyGetDamageAnim = LoadAnimation(enemyGetDamageAnimPath);
        }

        public RoomWithEnemy(Program _game) : base(_game)
        {
            enemy = new EnemyBase(_game);
            enemyDeathAnim = LoadAnimation(enemyDeathAnimPath);
            enemyGetDamageAnim = LoadAnimation(enemyGetDamageAnimPath);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            enemyDeathAnimPath = DefaultPath + "Death.txt";
            EnemyDeathSound = DefaultPath + "Death.wav";
            enemyGetDamageAnimPath = DefaultPath + "GetDamage.txt";
            enemyGetDamageSound = DefaultPath + "GetDamage.wav";
        }

        RoomWithEnemy(EnemyBase _enemy) : base()
        {
            enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
            enemy.RegenerateEnemy(enemiesStats, enemiesHealth, enemiesMultiplier);
            FightRound();
        }

        public void FightRound()
        {
            int playerInput = game.ProceedInput();//ProceedInput();

            Random r = new Random();
            //Utility system
            int enemiesInput = 0;
            int randInt = r.Next(1, enemy.stats.Sum() + 1);
            for (int i = 1; i < enemy.stats.Count + 1; i++)
            {
                if (randInt <= enemy.stats.Take(i).Sum())
                {
                    enemiesInput = i;
                    break;
                }
            }

            int playerGetsBonus = 0;
            int enemieGetsBonus = 0;
            int bonusDamage = 5;

            //Processing the battle system (rock-paper-scissors)
            if (playerInput != enemiesInput)
            {

                List<int> comparator = new List<int> { 1, 2, 3, 1 };
                if (FindLast(comparator, playerInput) - FindLast(comparator, enemiesInput) == 1) enemieGetsBonus = 1;
                else playerGetsBonus = 1;
            }
            else game.PlayFX("Tie.wav");
            //Damage calculation
            int damage = game.player.stats[playerInput - 1] + bonusDamage * playerGetsBonus - enemy.stats[enemiesInput - 1] - bonusDamage * enemieGetsBonus;
            //Show enemies playerInput
            List<string> actions = new List<string> { "Attack", "Cast a spell", "Defend" };
            game.textOutput.Clear();
            string s = String.Format("Enemie decided to {0}, you decided to {1}", actions[enemiesInput - 1], actions[playerInput - 1]);
            string s2 = String.Format("As a result, {0} taking {1} damage!", (damage < 0) ? "you are" : "enemy is", Math.Abs(damage));

            game.textOutput = new List<string> { s, s2, "Next round!" };
            //Processing damage
            if (damage > 0) enemy.TakeDamage(damage);
            else if (damage < 0)
            {
                //Jonas, if you are reading this, just know, that we are waiting for your onlyfans page :)
                game.player.TakeDamage(damage * (-1));
            }
            else
            {
                Console.Clear();
                game.UpdateFrame();
                FightRound();
            }
        }

        public void PlayEnemyDeathAnimation()
        {
            game.PlayFX(EnemyDeathSound);
            PlayAnimation(enemyDeathAnim, deathAnimDuration);
        }

        public void PlayEnemyGetDamageAnimation()
        {
            game.PlayFX(enemyGetDamageSound);
            PlayAnimation(enemyGetDamageAnim, enemyGetDamageAnimDuration);
        }

        public override bool haveEnemy()
        {
            return enemy.health > 0;
        }
    }

    public class Pawn
    {
        public int health;
        public List<int> stats;
        public Program game;

        public Pawn()
        {
            health = 50;
            stats = new List<int> { 10, 10, 10 };
        }

        public Pawn(Program _game)
        {
            game = _game;
            health = 50;
            stats = new List<int> { 10, 10, 10 };
        }

        public virtual void Death()
        {

        }

        public virtual void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
            else
            {
                DamageFX();
            }
        }

        public virtual void DamageFX()
        {
            Console.Clear();
            game.UpdateFrame();
        }

    }

    public class Player : Pawn
    {
        public Player() : base()
        {

        }

        public Player(Program _game) : base(_game)
        {
            health = 100;
        }

        public override void DamageFX()
        {
            base.DamageFX();
            game.currentRoom.PlayPlayerDamagedAnimation();
            //game.currentRoom.
            if (!game.currentRoom.haveEnemy()) return;
            RoomWithEnemy castedRoom = (RoomWithEnemy)game.currentRoom;
            if (castedRoom != null) castedRoom.FightRound();
        }

        public override void Death()
        {
            base.Death();
            game.currentRoom.ShowDeathScreen();
        }

        public void LevelUp(int stat)
        {
            stats[stat]++;
        }

        public void LevelRandomUp()
        {
            Random r = new Random();
            int stat = r.Next(0, 3);
            LevelUp(stat);
        }
    }

    public class EnemyBase : Pawn
    {
        public EnemyBase() : base() { }
        public EnemyBase(Program _game)
        {
            game = _game;
            generateEnemy();
        }

        public virtual void generateEnemy()
        {
            int sum = game.player.stats.Sum();
            int sum2 = stats.Sum();
            for (int i = 0; i < stats.Count; i++)
            {
                stats[i] *= sum;
                stats[i] /= sum2;
            }
        }

        public void RegenerateEnemy(List<int> newStats, int newHealth, float multiplier = 0.5f)
        {
            stats = newStats;
            health = newHealth;
            generateEnemy();
            for (int i = 0; i < stats.Count; i++) stats[i] = (int)(stats[i] * multiplier);
        }

        public override void DamageFX()
        {
            Console.Clear();
            RoomWithEnemy castedRoom = (RoomWithEnemy)game.currentRoom;
            castedRoom.PlayEnemyGetDamageAnimation();
            castedRoom.FightRound();
        }

        public override void Death()
        {
            RoomWithEnemy castedRoom = (RoomWithEnemy)game.currentRoom;
            castedRoom.PlayEnemyDeathAnimation();
            //add treasure function to RoomWithEnemy
            castedRoom.showLevelUp();
        }

    }

    public class Program
    {
        public List<string> mainImage;
        public List<string> textOutput;
        public List<string> interactive;
        public Player player;
        public RoomBase currentRoom;
        public List<RoomBase> rooms;

        public SoundPlayer musicPlayer = null;
        public SoundPlayer fxPlayer = null;

        public Program()
        {
            mainImage = new List<string>();
            textOutput = new List<string>();
            interactive = new List<string>();
            player = new Player(this);
            currentRoom = new RoomBase();
            rooms = new List<RoomBase>();
        }

        public void UpdateFrame()
        {
            //clear
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            //mainImage
            foreach (string line in mainImage)
            {
                Console.WriteLine(line);
            }
            //stats
            Console.WriteLine();
            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            if (!currentRoom.haveEnemy()) Console.WriteLine("Player Health: {0}, Strength: {1}, Intelligency: {2}, Defence: {3}", player.health, player.stats[0], player.stats[1], player.stats[2]);
            else
            {
                RoomWithEnemy castedRoom = (RoomWithEnemy)currentRoom;
                EnemyBase enemy = castedRoom.enemy;
                Console.WriteLine("PLAYER     Health: {0}, Strength: {1}, Inteligency: {2}, Defence: {3}                                                                 ENEMY     Health: {4}, Strength: {5}, Inteligency: {6}, Defence: {7}", player.health, player.stats[0], player.stats[1], player.stats[2], enemy.health, enemy.stats[0], enemy.stats[1], enemy.stats[2]);
            }
            for (int i = 0; i < Console.WindowWidth - 1; i++)
            {
                Console.Write("-");
            }
            Console.Write("\n");
            //textOutput
            foreach (string line in textOutput)
            {
                Console.WriteLine(line);
            }
            //interactivities
            Console.WriteLine();
            foreach (string line in interactive)
            {
                Console.WriteLine(line);
            }

        }

        public int ProceedInput()
        {
            ConsoleKey keyPressed = new ConsoleKey();
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            while (Console.KeyAvailable) Console.ReadKey(true);
            keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.D2:
                    return 2;
                case ConsoleKey.D3:
                    return 3;
                default:
                    break;

            }

            return ProceedInput();
        }

        public void PlayFX(string path, bool overwrite = true)
        {
            if (fxPlayer != null && overwrite)
            {
                if (!overwrite) return;
                StopFX();
            }
            fxPlayer = new SoundPlayer(path);
            fxPlayer.Play();
        }
        public void StopFX()
        {
            fxPlayer.Stop();
            fxPlayer.Dispose();
            fxPlayer = null;
        }

        public void PlayMusic(string path, bool overwrite = false)
        {
            if (musicPlayer != null)
            {
                if (!overwrite) return;
                StopMusic();
            }
            musicPlayer = new SoundPlayer(path);
            musicPlayer.PlayLooping();
        }
        public void StopMusic()
        {
            musicPlayer.Stop();
            musicPlayer.Dispose();
            musicPlayer = null;
        }

        static void Main(string[] args)
        {
            Program game = new Program();

            Console.WriteLine("Press ALT + ENTER To go fullscreenmode. After that press ENTER to start");
            Console.ReadLine();
            Console.Clear();
            game.PlayMusic("Intro.wav");

            //Simple Text Adventure...
            Console.CursorVisible = false;
            StateMachine BasicStateMahine = new StateMachine(36);
            EndState end = new EndState();
            BasicStateMahine.Add(end);

            BasicStateMahine.States[0].Description = new List<string> { "The moon is full tonight, so you see perfectly well what are you doing.", " " };
            BasicStateMahine.States[0].Options = new List<string> { "[1] Stop admiring the moon and focus on your task." };
            BasicStateMahine.States[0].States.Add(BasicStateMahine.States[1]);

            BasicStateMahine.States[1].Description = new List<string> { "The lock finally clicks.", "After many months of figuring out how to put your hooves on lockpicking, you finally manage to open your prizon cell.", "The path out is silent and, hopfully, clear.", "In this unfamiliar situation it's difficult for you to decide what to do next.", " " };
            BasicStateMahine.States[1].Options = new List<string> { "[1] Sneak away as silent as a grave.", "[2] Run away at the speed of sound.", "[3] Stay here and live comfortably for the rest of your life." };
            BasicStateMahine.States[1].States.Add(BasicStateMahine.States[2]);
            BasicStateMahine.States[1].States.Add(BasicStateMahine.States[3]);
            BasicStateMahine.States[1].States.Add(BasicStateMahine.States[4]);

            BasicStateMahine.States[2].Description = new List<string> { "You decide to walk slow but steady toward the dream of your life.", "One hoof at a time, you walk where hundreds of people walk every day.", "You know that somewhere there's an exit from this place, but you're unsure where.", " " };
            BasicStateMahine.States[2].Options = new List<string> { "[1] Go right - that's where people usually come from.", "[2] Go left - that's where people usually go." };
            BasicStateMahine.States[2].States.Add(BasicStateMahine.States[15]);
            BasicStateMahine.States[2].States.Add(BasicStateMahine.States[16]);

            BasicStateMahine.States[3].Description = new List<string> { "No time to wait!", "It can be any time when the supervisor will come to check the cell!", "An idea crosses your mind.", " " };
            BasicStateMahine.States[3].Options = new List<string> { "[1] Make a dummy of yourself in the cell, so nobody notices your escape.", "[2] Nah, just run, I'll be long gone when they come to the cell.", "[3] Just close the cell after myself." };
            BasicStateMahine.States[3].States.Add(BasicStateMahine.States[7]);
            BasicStateMahine.States[3].States.Add(BasicStateMahine.States[6]);
            BasicStateMahine.States[3].States.Add(BasicStateMahine.States[8]);

            BasicStateMahine.States[4].Description = new List<string> { "Is it worth it all?", "Is uncomfortable freedom worth all that months of lockpicking?", "You don't even know what awaits you there.", " " };
            BasicStateMahine.States[4].Options = new List<string> { "[1] I have nothing left to lose. I'll go", "[2] I don't want to lose what I have, because I won't have even that if I leave. Staying is my way." };
            BasicStateMahine.States[4].States.Add(BasicStateMahine.States[5]);
            BasicStateMahine.States[4].States.Add(BasicStateMahine.States[9]);

            BasicStateMahine.States[5].Description = new List<string> { "You decide to go, but what is your way to do it?", " " };
            BasicStateMahine.States[5].Options = new List<string> { "[1] Sneak away as silent as a grave", "[2] Run away at the speed of sound" };
            BasicStateMahine.States[5].States.Add(BasicStateMahine.States[2]);
            BasicStateMahine.States[5].States.Add(BasicStateMahine.States[3]);

            BasicStateMahine.States[6].Description = new List<string> { "You get ready to run, but first you need to decide where - it's the first time you're confronting this task.", " " };
            BasicStateMahine.States[6].Options = new List<string> { "[1] Go right - that's where people usually come from", "[2] Go left - that's where people usually go" };
            BasicStateMahine.States[6].States.Add(BasicStateMahine.States[15]);
            BasicStateMahine.States[6].States.Add(BasicStateMahine.States[16]);

            BasicStateMahine.States[7].Description = new List<string> { "While you're making a dummy of hay and water, you hear a crack of a branch.", "It can both your neighboring monkey, who usually mimics funny sounds in his sleep, or it can be someone approaching.", " " };
            BasicStateMahine.States[7].Options = new List<string> { "[1] Halt and wait for some time", "[2] Continue making a dummy", "[3] What's ready will do. I'll run right now" };
            BasicStateMahine.States[7].States.Add(BasicStateMahine.States[10]);
            BasicStateMahine.States[7].States.Add(BasicStateMahine.States[12]);
            BasicStateMahine.States[7].States.Add(BasicStateMahine.States[11]);

            BasicStateMahine.States[8].Description = new List<string> { "After the cell clicks behind you, you find yourself in another unusual situation - you need to choose where to go.", " " };
            BasicStateMahine.States[8].Options = new List<string> { "[1] Go right - that's where people usually come from", "[2] Go left - that's where people usually go" };
            BasicStateMahine.States[8].States.Add(BasicStateMahine.States[15]);
            BasicStateMahine.States[8].States.Add(BasicStateMahine.States[16]);

            BasicStateMahine.States[9].Description = new List<string> { "You decide to stay and keep what's deer to your heart", " " };
            BasicStateMahine.States[9].Options = new List<string> { "[1] Alt + F4 (Restart)", "[2] Go back in time" };
            BasicStateMahine.States[9].States.Add(BasicStateMahine.States[0]);
            BasicStateMahine.States[9].States.Add(BasicStateMahine.States[4]);

            BasicStateMahine.States[10].Description = new List<string> { "No, it wasn't the monkey - you now hear steps approaching.", "If you don't leave now, the watcher will close the door and you'll need to suffer with lockpicking for months again.", "But if you leave right now there is a chance that the supervisor will notice you and raise alarm.", " " };
            BasicStateMahine.States[10].Options = new List<string> { "[1] Go. Now. Forget the door", "[2] Go but close the door", "I don't want to lose what I have, because I won't have even that if I leave. Staying is my way.", " " };
            BasicStateMahine.States[10].States.Add(BasicStateMahine.States[18]);
            BasicStateMahine.States[10].States.Add(BasicStateMahine.States[17]);
            BasicStateMahine.States[10].States.Add(BasicStateMahine.States[9]);

            BasicStateMahine.States[11].Description = new List<string> { " " };
            BasicStateMahine.States[11].Options = new List<string> { "[1] Go. Now. Forget the door", "[2] Go but close the door" };
            BasicStateMahine.States[11].States.Add(BasicStateMahine.States[18]);
            BasicStateMahine.States[11].States.Add(BasicStateMahine.States[17]);

            BasicStateMahine.States[12].Description = new List<string> { "You become to entitled to making that dummy as handsome and fluffy as you are, so you don't even notice when the door clicks behind you, closing.", "That caring supervisor was really protective of you and decided to release you of all those choices you would have need to make if you exited that door.", " " };
            BasicStateMahine.States[12].Options = new List<string> { "[1] Alt + F4 (Restart)", "[2] Go back in time" };
            BasicStateMahine.States[12].States.Add(BasicStateMahine.States[0]);
            BasicStateMahine.States[12].States.Add(BasicStateMahine.States[7]);

            BasicStateMahine.States[15].Description = new List<string> { "The path is long, you see weird signs along the way.", "Maybe you should follow one?", " " };
            BasicStateMahine.States[15].Options = new List<string> { "[1] Go where \"Ехит\" leads", "[2] Go where \"Tюlets\" leads", "[3] Go where the tasty smell is coming from" };
            BasicStateMahine.States[15].States.Add(BasicStateMahine.States[19]);
            BasicStateMahine.States[15].States.Add(BasicStateMahine.States[24]);
            BasicStateMahine.States[15].States.Add(BasicStateMahine.States[20]);

            BasicStateMahine.States[16].Description = new List<string> { "You start following the chosen path, but you notice a moving cone of light that you recognize as the supervisor's light", "Have no choice but to go back", " " };
            BasicStateMahine.States[16].Options = new List<string> { "[1] Go back" };
            BasicStateMahine.States[16].States.Add(BasicStateMahine.States[15]);

            BasicStateMahine.States[17].Description = new List<string> { "After the door clicks behind you, closing, you turn around and find yourself in an ususual situation - you need to choose where to go.", " " };
            BasicStateMahine.States[17].Options = new List<string> { "[1] Go right - that's where people usually come from", "[2] Go left - that's where people usually go" };
            BasicStateMahine.States[17].States.Add(BasicStateMahine.States[15]);
            BasicStateMahine.States[17].States.Add(BasicStateMahine.States[16]);

            BasicStateMahine.States[18].Description = new List<string> { "Your legs just carry you before you even notice where you're going.", "But then you find yourself at a crossroad.", "Steps behind your back become faster and louder - he's chasing you!", " " };
            BasicStateMahine.States[18].Options = new List<string> { "[1] Go where \"Ехит\" leads", "[2] Go where \"Tюlets\" leads", "[3] Go where the tasty smell is coming from" };
            BasicStateMahine.States[18].States.Add(BasicStateMahine.States[19]);
            BasicStateMahine.States[18].States.Add(BasicStateMahine.States[24]);
            BasicStateMahine.States[18].States.Add(BasicStateMahine.States[20]);

            BasicStateMahine.States[19].Description = new List<string> { "You run as fast as you can, you're probably even got close to leaving that old man behind you, but didn't notice dark-metal bars that are keeping you on this side of the fence.", "You crush right into them, it takes you some time to regain your strength.", "The exit is close, but it's also closed. You need to go back.", " " };
            BasicStateMahine.States[19].Options = new List<string> { "[1] Go where \"Tюlets\" leads", "[2] Go where the tasty smell is coming from" };
            BasicStateMahine.States[19].States.Add(BasicStateMahine.States[24]);
            BasicStateMahine.States[19].States.Add(BasicStateMahine.States[20]);

            BasicStateMahine.States[20].Description = new List<string> { "When you approach the tasty smelling pit you're really unsure what were you expecting - it's just a pit full of dried hay, that is usually put in your feeder every morning.", "That smell is so warm, homely and cozy, that it makes you reconsider your life choises.", " " };
            BasicStateMahine.States[20].Options = new List<string> { "[1] Stay in the zoo and live comfortably for the rest of your life", "[2] Stay on your path. Hay is nothing compared to free life of an adventurer" };
            BasicStateMahine.States[20].States.Add(BasicStateMahine.States[4]);
            BasicStateMahine.States[20].States.Add(BasicStateMahine.States[21]);

            BasicStateMahine.States[21].Description = new List<string> { "No way back.", "Your will to break free is immence, and nothing can stop such a llama as you are.", " " };
            BasicStateMahine.States[21].Options = new List<string> { "[1] Try another way - to that third sign, whatever it means", "[2] I burrow myself in that pile of hay - the watcher might not find me there" };
            BasicStateMahine.States[21].States.Add(BasicStateMahine.States[22]);
            BasicStateMahine.States[21].States.Add(BasicStateMahine.States[23]);

            BasicStateMahine.States[22].Description = new List<string> { "It reads \"Tюlets\".", " " };
            BasicStateMahine.States[22].Options = new List<string> { "[1] Go there", " " };
            BasicStateMahine.States[22].States.Add(BasicStateMahine.States[24]);

            BasicStateMahine.States[23].Description = new List<string> { "When you lie, burrowed in that warm pile of your favourite food, you start to feel some weird feeling.", "But, never exprerienced that, you cannot understand what's going on.", " ", "After a couple of minutes, when the chasing steps stop right next to the pile, you recognize the feeling.", "You once felt that in your sleep, when liquid sand started to suck you into the depth of that hot desert.", "Same here.", "You're sucked into the void.", " " };
            BasicStateMahine.States[23].Options = new List<string> { "[1] Try and get out of this sucking trap", "[2] Just relax and see where it'll bring you. It's just your favourite food after all" };
            BasicStateMahine.States[23].States.Add(BasicStateMahine.States[25]);
            BasicStateMahine.States[23].States.Add(BasicStateMahine.States[30]);

            BasicStateMahine.States[24].Description = new List<string> { "You spend some time backtracking, but with each step you regret it more and more - an awful smell fills your sensitive nostrils.", "On the other hand, this smell might scare that old man away.", " " };
            BasicStateMahine.States[24].Options = new List<string> { "[1] Check if the man had stopped", "[2] Continue going where that stinky wind is coming from" };
            BasicStateMahine.States[24].States.Add(BasicStateMahine.States[26]);
            BasicStateMahine.States[24].States.Add(BasicStateMahine.States[27]);

            BasicStateMahine.States[25].Description = new List<string> { "With every move you get sucked into the depth even more." };
            BasicStateMahine.States[25].Options = new List<string> { "[1] Just relax and see where it'll bring you. It's just your favourite food after all" };
            BasicStateMahine.States[25].States.Add(BasicStateMahine.States[30]);

            BasicStateMahine.States[26].Description = new List<string> { "Nope, he is still coming. He's probably used to that smell." };
            BasicStateMahine.States[26].Options = new List<string> { "[1] Seems like following the smelling wind is the only option." };
            BasicStateMahine.States[26].States.Add(BasicStateMahine.States[27]);

            BasicStateMahine.States[27].Description = new List<string> { "That narrow path has led you to a small blue vertical house.", "The stink is unbearable, but something makes you think that even that old man won't go there to chase you", " " };
            BasicStateMahine.States[27].Options = new List<string> { "[1] Smash into that blue house" };
            BasicStateMahine.States[27].States.Add(BasicStateMahine.States[28]);

            BasicStateMahine.States[28].Description = new List<string> { "The stink inside the house is just mindblowing.", "You haven't experienced anything like this before, even when you were transported with that family of elephants.", "Right on the brink of your passing out mind you notice that you're about to fall into a black fetid hole", " " };
            BasicStateMahine.States[28].Options = new List<string> { "[1] Pass out" };
            BasicStateMahine.States[28].States.Add(BasicStateMahine.States[29]);

            BasicStateMahine.States[29].Description = new List<string> { "After a quick nap you found yourself not in the haystack anymore (you probably missed a lot of needles there).", "This place is much more darker, colder and scarier.", "But this stinking hole is a way to freedom, you can feel it.", "Or, rather, smell it.", " " };
            BasicStateMahine.States[29].Options = new List<string> { "[1] Step into the path of a free llama knight." };
            BasicStateMahine.States[29].States.Add(BasicStateMahine.States[36]);

            BasicStateMahine.States[30].Description = new List<string> { "You come deeper and deeper, not sure how much time have you already spent descending into the pit." };
            BasicStateMahine.States[30].Options = new List<string> { "[1] Continue relaxing" };
            BasicStateMahine.States[30].States.Add(BasicStateMahine.States[31]);

            BasicStateMahine.States[31].Description = new List<string> { "You even found some extra delicious pieces of hay on your way down." };
            BasicStateMahine.States[31].Options = new List<string> { "[1] Eat them", "[2] Nah, I can't eat when I'm in stress" };
            BasicStateMahine.States[31].States.Add(BasicStateMahine.States[32]);
            BasicStateMahine.States[31].States.Add(BasicStateMahine.States[33]);

            BasicStateMahine.States[32].Description = new List<string> { " " };
            BasicStateMahine.States[32].Options = new List<string> { "[1] You had no idea they have so much food for all the herbivorous animals in this zoo" };
            BasicStateMahine.States[32].States.Add(BasicStateMahine.States[35]);

            BasicStateMahine.States[33].Description = new List<string> { "But they look extra cool, it may be your last chance to try them" };
            BasicStateMahine.States[33].Options = new List<string> { "[1] Okay, I'll eat them" };
            BasicStateMahine.States[33].States.Add(BasicStateMahine.States[34]);

            BasicStateMahine.States[34].Description = new List<string> { "But they look extra cool, it may be your last chance to try them" };
            BasicStateMahine.States[34].Options = new List<string> { "[1] Okay, I'll eat them and take a nap" };
            BasicStateMahine.States[34].States.Add(BasicStateMahine.States[32]);

            BasicStateMahine.States[35].Description = new List<string> { "But they look extra cool, it may be your last chance to try them" };
            BasicStateMahine.States[35].Options = new List<string> { "[1] Okay, I'll eat them and take a nap" };
            BasicStateMahine.States[35].States.Add(BasicStateMahine.States[29]);

            BasicStateMahine.Run();

            game.StopMusic();
            Console.Clear();
            //HERE IS THE MAIN GAME

            //Quizes
            List<Quiz> quizes = new List<Quiz>(8);
            for (int i = 0; i < 8; i++)
            {
                quizes.Add(new Quiz());
            }
            quizes[0].question = "How many characters there may be in int?";
            quizes[0].answers = new List<string> { "[1] 12", "[2] 10", "[3] 11" };
            quizes[0].rightAnswer = 3;

            quizes[1].question = "How do you end a line inside of a function?";
            quizes[1].answers = new List<string> { "[1] ;*", "[2] ;", "[3] :" };
            quizes[1].rightAnswer = 2;

            quizes[2].question = "How {} are called?";
            quizes[2].answers = new List<string> { "[1] embraces/curly fries", "[2] brothers/curly hair", "[3] braces / curly brackets" };
            quizes[2].rightAnswer = 3;

            quizes[3].question = "How many different values can a boolean store?";
            quizes[3].answers = new List<string> { "[1] 2", "[2] 0", "[3] 4.20" };
            quizes[3].rightAnswer = 1;

            quizes[4].question = "A local variable is stored in?";
            quizes[4].answers = new List<string> { "[1] A Code Segment", "[2] A Heap Segment", "[3] A Stack Segment" };
            quizes[4].rightAnswer = 3;

            quizes[5].question = "Which of the following access specifier in C# allows a class to hide its member variables and member functions from other functions and objects?";
            quizes[5].answers = new List<string> { "[1] Private", "[2] Internal", "[3] OnlyFans" };
            quizes[5].rightAnswer = 1;

            quizes[6].question = "Which of the following can be used to define the member of a class externally?";
            quizes[6].answers = new List<string> { "[1] :", "[2] ::", "[3] #" };
            quizes[6].rightAnswer = 2;

            quizes[7].question = "What is the highest value for an int?";
            quizes[7].answers = new List<string> { "[1] 2,147,483,647", "[2] 2,144,843,476", "[3] 1,782,193,321" };
            quizes[7].rightAnswer = 1;

            
            QuizRoom quizRoom1 = new QuizRoom(game);
            QuizRoom quizRoom2 = new QuizRoom(game);
            QuizRoom quizRoom3 = new QuizRoom(game);
            Random r = new Random();
            int quizIndex = r.Next(0, 8);
            quizRoom1.quiz = quizes[quizIndex];
            quizes.RemoveAt(quizIndex);
            quizIndex = r.Next(0, 7);
            quizRoom2.quiz = quizes[quizIndex];
            quizes.RemoveAt(quizIndex);
            quizIndex = r.Next(0, 6);
            quizRoom3.quiz = quizes[quizIndex];

            //monsters
            BossRoom bossRoom = new BossRoom(game);
            BasicPangolinRoom basicPangolinRoom = new BasicPangolinRoom(game);
            ProPangolinRoom proPangolinRoom = new ProPangolinRoom(game);
            BasicScorpioRoom basicScorpioRoom = new BasicScorpioRoom(game);
            ProScorpioRoom proScorpioRoom = new ProScorpioRoom(game);
            BasicBabboonRoom basicBabboonRoom = new BasicBabboonRoom(game);
            ProBabboonRoom proBabboonRoom = new ProBabboonRoom(game);

            //ShowLogo
            RoomBase showLogo = new LogoRoom(game);
            IntroRoom intro = new IntroRoom(game);
            ExitRoom exit = new ExitRoom(game);

            //ordering
            intro.nextRoom = basicPangolinRoom;
            basicPangolinRoom.nextRoom = basicScorpioRoom;
            basicScorpioRoom.nextRoom = quizRoom1;
            quizRoom1.nextRoom = bossRoom;
            bossRoom.nextRoom = proScorpioRoom;
            proScorpioRoom.nextRoom = proPangolinRoom;
            proPangolinRoom.nextRoom = quizRoom2;
            quizRoom2.nextRoom = basicBabboonRoom;
            basicBabboonRoom.nextRoom = quizRoom3;
            quizRoom3.nextRoom = proBabboonRoom;
            proBabboonRoom.nextRoom = exit;

            showLogo.Enter();
            intro.Enter();

            //Console.ReadLine();
            //Console.ReadLine();
            //game.UpdateFrame();
        }
    }
}
