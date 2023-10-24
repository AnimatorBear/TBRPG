namespace TBRPGV2
{
    internal class Program
    {
        static Creature currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
        static Creature.allClasses testingEnemyClass = Creature.allClasses.Tank;
        static int classSelection = 0;
        //Few stats, Put amountOfClasses to 6 for Bag class
        public static int amountOfClasses = 5;
        static void Main(string[] args)
        {
            //Some Misc things
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "TBRPG";

            NewCharacter();

            do
            {
                currentPlayer.health = currentPlayer.maxHealth;
                StartBattle(false);
            }
            while (true);

            NewCharacter();

            StartBattle(true);

            Console.ReadKey();
        }
        static void NewCharacter()
        {
            currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
            //Sets up the player
            //currentPlayer.damageMultiplier = 500;
            while (currentPlayer.currentClass == Creature.allClasses.Class_None)
            {
                SelectClass();
            }
            currentPlayer.skills[0] = currentPlayer.randomSkill();
            currentPlayer.skills[1] = currentPlayer.randomSkill();
            currentPlayer.skills[2] = currentPlayer.randomSkill();
            ShowRandomSkills();
            Console.ReadKey();
            Console.Clear();

            //Makes the player stats
            currentPlayer.RecalculateStats();
            currentPlayer.health = currentPlayer.maxHealth;
            Console.WriteLine($"Class: {currentPlayer.currentClass}");

            //Makes a battle
            StartBattle(true);
        }
        static void StartBattle(bool showStatsAtStart)
        {
            //Makes the enemy
            Creature enemy = new Creature(testingEnemyClass,currentPlayer.currentLevel);
            Console.WriteLine($"Enemy Class: {enemy.currentClass}");

            //Recalculate Stats
            enemy.RecalculateStats(showStatsAtStart);
            currentPlayer.RecalculateStats(showStatsAtStart) ;
            enemy.health = enemy.maxHealth;

            bool activeBattle = true;
            while (activeBattle)
            {
                Console.Title = "TBRPG";
                Console.WriteLine("-----");
                Console.WriteLine($"Player hp: {currentPlayer.health} , Enemy hp: {enemy.health}");
                Console.WriteLine($"Player dmg: {currentPlayer.damage} , Enemy dmg: {enemy.damage}");
                bool selectingAttack = true;

                //Makes some ints
                int selectedAttack = 0;
                int damage = 0;
                int dodge = 0;
                bool dodged;

                while (selectingAttack)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    AddChargerCharge(enemy);
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            selectedAttack = 1;
                            break;
                        case ConsoleKey.D2:
                            selectedAttack = 2;
                            break;
                        case ConsoleKey.D3:
                            selectedAttack = 3;
                            break;
                        case ConsoleKey.D4:
                            selectedAttack = 4;
                            break;
                        case ConsoleKey.D5:
                            selectedAttack = 5;
                            AddChargerCharge(enemy,true);
                            break;
                        case ConsoleKey.D0:
                            currentPlayer.health = 0;
                            selectedAttack = -1;
                            break;
                    }
                    damage = currentPlayer.Attack(out dodge, selectedAttack);
                    if (damage != -100)
                    {
                        selectingAttack = false;
                    }
                }

                //Enemy Dodging
                #region Enemy Dodge
                dodged = CalculateDodge(enemy, currentPlayer, dodge);
                if(!dodged)
                {
                    enemy.health -= damage;
                }
                else
                {
                    Console.WriteLine("Dodge!");
                }
                #endregion
                //Death check
                if (enemy.health < 1)
                {
                    Console.WriteLine($"☠ Enemy died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    currentPlayer.currentLevel += 1;
                    activeBattle = false;
                }
                else if (enemy.health > enemy.maxHealth)
                {
                    enemy.health = enemy.maxHealth;
                }
                damage = enemy.Attack(out dodge, selectedAttack);
                if (damage == -100)
                {
                    damage = 0;
                }
                //Player Dodging
                #region Player Dodge
                dodged = CalculateDodge(currentPlayer,enemy, dodge);
                if (!dodged)
                {
                    currentPlayer.health -= damage;
                }
                else
                {
                    Console.WriteLine("Dodge!");
                }
                #endregion
                //Death check
                if (currentPlayer.health < 1)
                {
                    Console.WriteLine($"☠ Player died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    activeBattle = false;
                } else if(currentPlayer.health > currentPlayer.maxHealth)
                {
                    currentPlayer.health = currentPlayer.maxHealth;
                }
            }
        }
        static bool CalculateDodge(Creature creature, Creature creature2,int startingDodge)
        {
            //Creature is defending , Creature2 is attacking,
            int totalDodge = startingDodge;

            //Adds or removes dodge based on skills
            for(int i = 0; i < creature.skills.Length; i++)
            {
                if (creature.skills[i] == Creature.allSkills.Fast)
                {
                    totalDodge += 5;
                }
            }
            for (int i = 0; i < creature2.skills.Length; i++)
            {
                if (creature2.skills[i] == Creature.allSkills.Accurate)
                {
                    totalDodge -= 5;
                }
            }

            //Speed
            totalDodge += creature.speed - creature2.speed;

            //The Random
            Random rnd = new Random();
            int rand = rnd.Next(0, 100);
            if (rand < totalDodge)
            {
                return true;
            }
            return false;
        }
        static void AddChargerCharge(Creature enemy,bool remove = false)
        {
            if(currentPlayer.currentClass == Creature.allClasses.Charger)
            {
                currentPlayer.chargerCharge++;
            }
            if (enemy.currentClass == Creature.allClasses.Charger)
            {
                enemy.chargerCharge++;
            }
        }
        static void SelectClass()
        {
            Console.Clear();

            int startingSelection = classSelection;
            ConsoleColor selectionColor = ConsoleColor.White;
            ConsoleColor selectionTextColor = ConsoleColor.Black;
            //Class Icons,Names and Descriptions
            string[] classNames = { "Damage Dealer", "Tank","Healer", "Randomizer", "Charger","Bag" };
            string[][] classDescriptions = new string[6][]
            {
                new string[5]
                {
                    "The damage dealer is a class based around damage","This class lacks in defence","Class Ability:","Do an attack that does 1.5x your damage",""
                },
                    new string[5]
                {
                    "The tank is a class based around damage and health","It is a mix of healer and damage dealer","Class Ability:","Heal 10% of your max health","Still do half a light attack afterwards"
                },
                    new string[5]
                {
                    "The healer is a class based around health","This class lacks in damage","Class Ability:","Heal 50% of your health",""
                },
                    new string[5]
                {
                    "The randomizer is based around randomness","It's strength is based around your luck","Class Ability:","Use a random selected attack",""
                },
                    new string[5]
                {
                    "The charger is based around charging your attacks","Your attacks deal more damage the more you use them","Class Ability:","Every time you dont use your class ability ","your class ability will attack more times when you use it"
                },
                    new string[5]
                {
                    "Punching Bag","Testing Class","Class Ability:","Bag","Icon is a beaker"
                }
            };
            string[][] iconArray = new string[6][]{
                new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "},
                new string[4]{
                    "  /----\\  ",
                    "  | -- |  ",
                    "  |    |  ",
                    "  \\----/  " },
                new string[4]{
                    "    ||    ",
                    "----╝╚----",
                    "----╗╔----",
                    "    ||    "},
                new string[4]{
                    "  ╔----╗  ",
                    "  |  ()|  ",
                    "  |()  |  ",
                    "  ╚----╝  "},
                new string[4]{
                    " ======== ",
                    " |████  | ",
                    " |██    | ",
                    " ======== "},
                new string[4]{
                    "   |  |   ",
                    "   |‾-|   ",
                    "  /    \\  ",
                    " /______\\ "}


            };

            //Places most of the lines
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("========================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("========================================================================\r\n");
            Console.ForegroundColor = ConsoleColor.White;
            //Places all class info and icons
            for (int i = 0; i < amountOfClasses; i++)
            {
                //Class info
                #region Class Info
                Console.CursorVisible = false;
                int center = classNames[classSelection].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop - 15);
                Console.WriteLine(classNames[classSelection]);
                for(int j = 0; j < 1; ++j)
                {
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                }
                for (int j = 0; j < 5; j++)
                {
                    center = classDescriptions[classSelection][j].Length / 2;
                    Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop);
                    Console.WriteLine(classDescriptions[classSelection][j]);
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                for (int j = 0; j < 6; j++)
                {
                    string word = "prob";
                    switch (j)
                    {
                        case 0:
                            word = "Base max HP: " + currentPlayer.classStats[classSelection,0];
                            break;
                        case 1:
                            word = "Base damage: " + currentPlayer.classStats[classSelection, 1];
                            break;
                        case 2:
                            word = "Rounds until ability recharge: " + currentPlayer.classStats[classSelection, 2];
                            break;
                        case 3:
                            if (currentPlayer.classStats[classSelection,3] != 999)
                            {
                                word = "Class ability uses: " + currentPlayer.classStats[classSelection, 3];
                            }
                            else
                            {
                                word = "Class ability uses: Infinite";
                            }
                            break;
                        case 4:
                            word = "Speed: " + currentPlayer.classStats[classSelection, 4];
                            break;
                        case 5:
                            word = "special";
                            break;

                    }
                    center = word.Length / 2;
                    Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop);
                    Console.WriteLine(word);
                }
                #endregion
                //Places all class boxes and put their icons in there
                #region PlaceBoxes
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 2);
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.Write("|");
                if (classSelection == i)
                {
                    Console.BackgroundColor = selectionColor;
                    Console.ForegroundColor = selectionTextColor;
                }
                Console.Write(iconArray[i][0]);
                if (classSelection == i)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("|  ");
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.Write("|");
                if (classSelection == i)
                {
                    Console.BackgroundColor = selectionColor;
                    Console.ForegroundColor = selectionTextColor;
                }
                Console.Write(iconArray[i][1]);
                if (classSelection == i)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("|  ");
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.Write("|");
                if (classSelection == i)
                {
                    Console.BackgroundColor = selectionColor;
                    Console.ForegroundColor = selectionTextColor;
                }
                Console.Write(iconArray[i][2]);
                if (classSelection == i)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("|  ");
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.Write("|");
                if (classSelection == i)
                {
                    Console.BackgroundColor = selectionColor;
                    Console.ForegroundColor = selectionTextColor;
                }
                Console.Write(iconArray[i][3]);
                if (classSelection == i)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("|  ");
                Console.SetCursorPosition(Console.CursorLeft + (15 * i), Console.CursorTop);
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 6);
                #endregion
            }
            //The last line
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 7);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================");


            string sentence = "Press ENTER to select a class";
            int center2 = sentence.Length / 2;
            Console.SetCursorPosition(Console.CursorLeft + 35 - center2, Console.CursorTop + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sentence);
            Console.ForegroundColor = ConsoleColor.White;

            //Move in UI and Select Class
            while (startingSelection == classSelection)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if(key == ConsoleKey.A)
                {
                    classSelection--;
                }
                else if (key == ConsoleKey.D)
                {
                    classSelection++;
                }
                else if (key == ConsoleKey.Enter)
                {
                    switch (classSelection)
                    {
                        case 0:
                            currentPlayer.currentClass = Creature.allClasses.DamageDealer;
                            break;
                        case 1:
                            currentPlayer.currentClass = Creature.allClasses.Tank;
                            break;
                        case 2:
                            currentPlayer.currentClass = Creature.allClasses.Healer;
                            break;
                        case 3:
                            currentPlayer.currentClass = Creature.allClasses.RNG;
                            break;
                        case 4:
                            currentPlayer.currentClass = Creature.allClasses.Charger;
                            break;
                        case 5:
                            currentPlayer.currentClass = Creature.allClasses.Bag;
                            break;
                    }
                    Console.Clear();
                    startingSelection = 100;
                }
                if(classSelection == amountOfClasses)
                {
                    classSelection = 0;
                } else if(classSelection <= -1)
                {
                    classSelection = amountOfClasses - 1;
                }
            }

        }

        //Shows what skills you got at the start of the game
        static void ShowRandomSkills()
        {
            
            Console.ForegroundColor = ConsoleColor.White;
            string[] skillNames = { "", "", ""};
            string[][] skillDescriptions = new string[3][]
            {
                new string[5]
                {
                    " descriptions (1) "," descriptions 2"," descriptions 3"," descriptions 4"," descriptions 5"
                },
                    new string[5]
                {
                    " descriptions (2)"," descriptions 2"," descriptions 3"," descriptions 4"," descriptions 5"
                },
                    new string[5]
                {
                    " descriptions (3) "," descriptions 2"," descriptions 3"," descriptions 4"," descriptions 5"
                }
            };
            //Icons for all skills
            string[][] iconArray = new string[5][]{
                new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "},
                new string[4]{
                    "   _  _   ",
                    "  | \\/ |  ",
                    "   \\  /   ",
                    "    \\/    "},
                new string[4]{
                    "   - ---/\\",
                    " - ----/ /",
                    "- ----/ / ",
                    "-- --/ /  "},
                new string[4]{
                    "          ",
                    "   w.i.p  ",
                    "   w.i.p  ",
                    "          " },
                new string[4]{
                    " ======== ",
                    " |████  | ",
                    " |██    | ",
                    " ======== "} };
            //Icons for the skills you have
            string[][] activeSkillsIconArray = new string[3][]{
                new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "},
                new string[4]{
                    "  /----\\  ",
                    "  | -- |  ",
                    "  |    |  ",
                    "  \\----/  " },
                new string[4]{
                    "    ||    ",
                    "----╝╚----",
                    "----╗╔----",
                    "    ||    "} };
            for(int i = 0; i < 3; i++)
            {
                //Turns everything to unknown because default case was crying
                skillNames[i] = "Unknown";
                skillDescriptions[i][0] = "Unknown";
                skillDescriptions[i][1] = "Unknown";
                for (int j = 0; j < 4; j++)
                {
                    activeSkillsIconArray[i][j] = iconArray[3][j];
                }
                //Changes things based on the skills
                switch (currentPlayer.skills[i])
                {
                    case Creature.allSkills.Healthy:
                        for(int j = 0;j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[1][j];
                        }
                        skillNames[i] = "Healthy";
                        skillDescriptions[i][0] = "+25 HP";
                        skillDescriptions[i][1] = "";
                        break;
                    case Creature.allSkills.Violent:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[0][j];
                        }
                        skillNames[i] = "Violent";
                        skillDescriptions[i][0] = "+5 Damage";
                        skillDescriptions[i][1] = "";
                        break;
                    case Creature.allSkills.Accurate:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[2][j];
                        }
                        skillNames[i] = "Accurate";
                        skillDescriptions[i][0] = "-5 chance ";
                        skillDescriptions[i][1] = "for an enemy to dodge";
                        break;
                    case Creature.allSkills.Fast:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Fast";
                        skillDescriptions[i][0] = "+3 Speed";
                        skillDescriptions[i][1] = "";
                        break;
                    case Creature.allSkills.Fast_Learner:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Fast Learner";
                        skillDescriptions[i][0] = "Useless";
                        skillDescriptions[i][1] = "";
                        break;
                    case Creature.allSkills.Light_Hitter:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Light Hitter";
                        skillDescriptions[i][0] = "1.2x";
                        skillDescriptions[i][1] = "light hit damage";
                        break;
                    case Creature.allSkills.Heavy_Hitter:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Heavy Hitter";
                        skillDescriptions[i][0] = "1.2x";
                        skillDescriptions[i][1] = "heavy hit damage";
                        break;
                    case Creature.allSkills.Glass_Cannon:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Glass Cannon";
                        skillDescriptions[i][0] = "1.5x Damage";
                        skillDescriptions[i][1] = "75% max HP";
                        break;
                    case Creature.allSkills.Stone_Wall:
                        for (int j = 0; j < 4; j++)
                        {
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Stone Wall";
                        skillDescriptions[i][0] = "1.5x max Health";
                        skillDescriptions[i][1] = "75% Damage";
                        break;
                }
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 10);
            //Makes the box and puts the icon in it
            for (int i = 0; i < 3; i++)
            {
                int cursorMoveAmount = 20 * (i + 1);
                cursorMoveAmount = cursorMoveAmount - 10;
                Console.CursorVisible = false;
                int center = skillNames[i].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop - 5);
                Console.WriteLine(skillNames[i] + "\r\n");
                center = skillDescriptions[i][0].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
                Console.WriteLine(skillDescriptions[i][0]);
                center = skillDescriptions[i][1].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
                Console.WriteLine(skillDescriptions[i][1] + "\r\n");
                Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                for(int j = 0; j < 4; j++)
                {
                    Console.Write("|");
                    Console.Write(activeSkillsIconArray[i][j]);
                    Console.WriteLine("|  ");
                    Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                }
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 6);
            }
        }
    }
}