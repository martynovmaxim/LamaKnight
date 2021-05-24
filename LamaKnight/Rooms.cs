using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamaKnight
{
    public class BossRoom : RoomWithEnemy
    {
        public BossRoom() : base()
        {
        }
        public BossRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 2000;

            enemiesStats = new List<int> { 3, 2, 2 };
            enemiesHealth = 70;
            enemiesMultiplier = 0.5f;

            DefaultPath = "Skull/";
            text = new List<string> { "This skull infliced fear in all those who tried to go past him before.", "But not you.", "You are his first Llama Knight." };
            interactions = new List<string> { "How are you going to SMITE him?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();
        }
    }

    public class BasicPangolinRoom : RoomWithEnemy
    {
        public BasicPangolinRoom() : base()
        {
        }
        public BasicPangolinRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 2000;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 3, 1, 6 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 50;
            enemiesMultiplier = 0.5f;

            DefaultPath = "Pangolin/";
            text = new List<string> { "It's muzzle is dripping with mud.", "It's bloodthursty gaze makes you shiver.", "It's the first, but certainly not the last obstacle in your journey to freedom." };
            interactions = new List<string> { "How are you going to deal with that panzered beast?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();


        }
    }

    public class BasicScorpioRoom : RoomWithEnemy
    {
        public BasicScorpioRoom() : base()
        {
        }
        public BasicScorpioRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 750;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 5, 2, 3 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 50;
            enemiesMultiplier = 0.5f;

            DefaultPath = "Scorpio/";
            text = new List<string> { "The only thing you managed to notice is poison dripping from the ceiling.", "A blink - and there's a shimmering scorpion attacking you!", "You barely manage to dodge!" };
            interactions = new List<string> { "What are you going to do about this poisonous critter?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();


        }
    }

    public class BasicBabboonRoom : RoomWithEnemy
    {
        public BasicBabboonRoom() : base()
        {
        }
        public BasicBabboonRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 750;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 2, 5, 1 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 50;
            enemiesMultiplier = 0.5f;

            DefaultPath = "Babboon/";
            text = new List<string> { "You finally see a familiar figure in these sewers.", "It's your long-gone friend Babboon! He was the first one to escape the zoo!", "But when you look into his eyes, you undertand that there is no sign of sanity, friendship, or life in his eyes..." };
            interactions = new List<string> { "What is your way of deciding his fate?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();


        }
    }

    public class ProPangolinRoom : RoomWithEnemy
    {
        public ProPangolinRoom() : base()
        {
        }
        public ProPangolinRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 750;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 2, 1, 7 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 65;
            enemiesMultiplier = 0.8f;

            DefaultPath = "Pangolin/";
            text = new List<string> { "You hear a clicking sound.", "You see organic pieces coming together. It's hard to understand what they are.", "But it's even harder to forget that pangolin's bloodthursty gaze." };
            interactions = new List<string> { "What you are going to do?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();


        }
    }

    public class ProScorpioRoom : RoomWithEnemy
    {
        public ProScorpioRoom() : base()
        {
        }
        public ProScorpioRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 750;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 3, 1, 1 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 80;
            enemiesMultiplier = 0.8f;

            DefaultPath = "Scorpio/";
            text = new List<string> { "Not even the pesticides and chemicals from above can eliminate those scorpions.", "I guess, it takes a Llama Knight do get rid of them.", " " };
            interactions = new List<string> { "Choose your way of e-llama-nation.", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();


        }
    }

    public class ProBabboonRoom : RoomWithEnemy
    {
        public ProBabboonRoom() : base()
        {
        }
        public ProBabboonRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 750;
            deathAnimDuration = 1500;
            enemyGetDamageAnimDuration = 250;

            enemiesStats = new List<int> { 2, 5, 3 }; //Strength, Inteligence, Defence   this is a portions, not absolute
            enemiesHealth = 50;
            enemiesMultiplier = 0.8f;

            DefaultPath = "Babboon/";
            text = new List<string> { "Even in life your friend one of the stubbornest.", "You couldn't deal with this when you were together.", "So you need to do it now." };
            interactions = new List<string> { "How are you gonna take the last step?", "1) Attack with a sword", "2) Cast a spell", "3) Defend" };
            base.SetDefaults();
        }
    }

    public class IntroRoom : RoomBase
    {
        public IntroRoom() : base()
        {
        }
        public IntroRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 4000;
            

            DefaultPath = "Intro/";
            text = new List<string> { "After the fall you look into pitch black void in front of you...", "But when you take the first step forward, the most important step, you hear a metallic CLANCK.", "It's a sword!" };
            interactions = new List<string> { "Look at how many choices you have!", "[1] Take it. What's a knight without their sword?", "", "" };
            base.SetDefaults();
        }

        public override void Enter()
        {
            base.Enter();
            int input = game.ProceedInput();
            TransitionRoom(nextRoom);
        }
    }

    public class LogoRoom : RoomBase
    {
        public LogoRoom() : base()
        {
        }
        public LogoRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 4000;


            DefaultPath = "Logo/";
            text = new List<string> { "Please, follow the rules", "You can use onlu 3 buttons: ", "[1] [2] and [3]" };
            interactions = new List<string> { "Press any of them to start the game", "", "", "" };
            base.SetDefaults();
            
        }
    }

    public class ExitRoom : RoomBase
    {
        public ExitRoom() : base()
        {
        }
        public ExitRoom(Program _game) : base(_game)
        {
        }

        public override void SetDefaults()
        {
            entryDuration = 1500;


            DefaultPath = "Exit/";
            text = new List<string> { "The cold breeze freshens your breath. You drop the sword - you'll find a better one.", "You've been through hell of imprisonment and defeating your dearest friend.", "And now, you have earned your knighthood. But at what cost?.." };
            interactions = new List<string> { "Thank you for playing!", "", "", "" };
            base.SetDefaults();
        }
        public override void Enter()
        {
            base.Enter();
            int input = game.ProceedInput();
            Environment.Exit(0);
        }
    }
}