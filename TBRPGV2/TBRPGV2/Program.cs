using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static TBRPGV2.Creature;

namespace TBRPGV2
{
    internal class Program
    {
        const bool chooseStartSkills = true;
        public const int amountStartSkills = 3;



        //Player , Enemy test class , what class is selected
        static Creature currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
        static Creature.allClasses testingEnemyClass = Creature.allClasses.Healer;
        static int classSelection = 0;


        //What GetSkillInfo(); changes
        static string skillName = "";
        static string[] skillDescription = { "", "", "", "", "" };
        static string[] activeSkillsIcon = new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "};

        //Selected skill in the level 20 skill selection screen
        static int pagesDown = 0;
        static int selectedSkill = 0;



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
                {
                    while (currentPlayer.skills[amountStartSkills] == allSkills.None && currentPlayer.currentLevel >= 10)
                    {
                        try
                        {
                            //Select a 4rth skill at level 20
                            int amount = SelectSkill();
                            MoveSelectedSkill(amount, amountStartSkills);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You already have all skills!");
                            break;
                        }
                    }
                }
                currentPlayer.health = currentPlayer.maxHealth;
                currentPlayer.classAbilityUses = 0;
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
            CreatureSkills(currentPlayer, !chooseStartSkills, amountStartSkills);
            Console.Clear();
            ShowRandomSkills();
            Console.ReadKey();
            Console.Clear();

            //Makes the player stats
            currentPlayer.RecalculateStats();
            currentPlayer.health = currentPlayer.maxHealth;
        }

        static void CreatureSkills(Creature crt, bool random, int amountOfSkills)
        {
            for (int i = 0; i < amountOfSkills; i++)
            {
                if (random)
                {
                    crt.skills[i] = crt.randomSkill();
                }
                else
                {
                    selectedSkill = 0;
                    while (crt.skills[i] == allSkills.None)
                    {
                        int amount = SelectSkill();
                        MoveSelectedSkill(amount, i);
                    }
                }
            }
        }
        static void StartBattle(bool showStatsAtStart)
        {
            //Makes the enemy
            Creature enemy = new Creature(testingEnemyClass, currentPlayer.currentLevel);
            Enemy enemyBrain = new Enemy(enemy, currentPlayer);
            enemy.currentClass = enemyBrain.ChooseClass();
            enemyBrain.ChooseExtraSkill();
            //Console.WriteLine($"Enemy Class: {enemy.currentClass}");

            //Recalculate Stats
            enemy.RecalculateStats(showStatsAtStart);
            currentPlayer.RecalculateStats(showStatsAtStart);
            enemy.health = enemy.maxHealth;

            bool activeBattle = true;
            while (activeBattle)
            {
                DrawBattle(ConsoleColor.DarkGray,enemy);
                Console.Title = "TBRPG";
                bool selectingAttack = true;

                //Makes some ints
                int selectedAttack = 0;
                int healing = 0;
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
                            AddChargerCharge(enemy, true);
                            break;
                        case ConsoleKey.D6:
                            currentPlayer.itemsInInv[0].UseItem();
                            break;
                        case ConsoleKey.D0:
                            currentPlayer.health = 0;
                            selectedAttack = -1;
                            break;
                    }
                    damage = currentPlayer.Attack(out dodge,out healing, selectedAttack);
                    if (damage != -100)
                    {
                        selectingAttack = false;
                    }
                }

                //Enemy Dodging
                #region Enemy Dodge
                dodged = CalculateDodge(enemy, currentPlayer, dodge);
                if (!dodged)
                {
                    enemy.health -= damage;
                }
                else
                {
                    
                }
                #endregion
                //Death check
                if (enemy.health < 1)
                {
                    //Console.WriteLine($"☠ Enemy died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    Thread.Sleep(1000);
                    currentPlayer.AddXP(CalculateXPGain(currentPlayer, enemy));
                    currentPlayer.chargerCharge = 0;
                    activeBattle = false;
                }
                else if (enemy.health > enemy.maxHealth)
                {
                    enemy.health = enemy.maxHealth;
                }
                damage = enemy.Attack(out dodge, out healing, enemyBrain.ChooseAttack());
                if (damage == -100)
                {
                    damage = 0;
                }
                //Player Dodging
                #region Player Dodge
                dodged = CalculateDodge(currentPlayer, enemy, dodge);
                if (!dodged)
                {
                    currentPlayer.health -= damage;
                }
                else
                {
                    
                }
                #endregion
                //Death check
                if (currentPlayer.health < 1)
                {
                    //Console.WriteLine($"☠ Player died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    Thread.Sleep(1000);
                    activeBattle = false;
                    currentPlayer.chargerCharge = 0;
                } else if (currentPlayer.health > currentPlayer.maxHealth)
                {
                    currentPlayer.health = currentPlayer.maxHealth;
                }
            }
        }
        static int CalculateXPGain(Creature winner, Creature loser)
        {
            int startingXPGain = 150;
            float xpBuff = 1;

            if (winner.currentClass == allClasses.RNG)
            {
                Random rnd = new Random();
                xpBuff = rnd.Next(5, 15);
                xpBuff = xpBuff / 10;
            }
            //Math
            if (winner.currentLevel < loser.currentLevel)
            {
                xpBuff += 0.05f * (loser.currentLevel - winner.currentLevel);
            }
            if (winner.health > winner.maxHealth * 0.75f)
            {
                xpBuff += 0.25f;
            }
            int xpGain = (int)(startingXPGain * xpBuff);
            for (int i = 0; i < winner.skills.Length; i++)
            {
                if (winner.skills[i] == allSkills.Fast_Learner)
                {
                    xpGain += (int)(xpGain * 0.1f);
                }
            }
            return xpGain;
        }
        static bool CalculateDodge(Creature creature, Creature creature2, int startingDodge)
        {
            //Creature is defending , Creature2 is attacking,
            int totalDodge = startingDodge;

            //Adds or removes dodge based on skills
            for (int i = 0; i < creature.skills.Length; i++)
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
        static void AddChargerCharge(Creature enemy, bool remove = false)
        {
            if (currentPlayer.currentClass == Creature.allClasses.Charger)
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
            string[] classNames = { "Damage Dealer", "Tank", "Healer", "Randomizer", "Charger", "Bag" };
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
                for (int j = 0; j < 1; ++j)
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
                            word = "Base max HP: " + currentPlayer.classStats[classSelection, 0];
                            break;
                        case 1:
                            word = "Base damage: " + currentPlayer.classStats[classSelection, 1];
                            break;
                        case 2:
                            word = "Rounds until ability recharge: " + currentPlayer.classStats[classSelection, 2];
                            break;
                        case 3:
                            if (currentPlayer.classStats[classSelection, 3] != 999)
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
                if (key == ConsoleKey.A)
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
                if (classSelection == amountOfClasses)
                {
                    classSelection = 0;
                } else if (classSelection <= -1)
                {
                    classSelection = amountOfClasses - 1;
                }
            }

        }

        //Shows what skills you got at the start of the game
        static void ShowRandomSkills()
        {

            Console.ForegroundColor = ConsoleColor.White;
            //Icons for all skills
            //Icons for the skills you have
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 10);
            //Makes the box and puts the icon in it
            for (int i = 0; i < currentPlayer.skills.Length - 1; i++)
            {
                int cursorMoveAmount = 0;
                if (i == 5)
                {
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 13);
                }
                if (i >= 5)
                {
                    int amount = (int)i / 5;
                    cursorMoveAmount = 20 * (i - ((amount * 5) - 1));
                }
                else
                {
                    cursorMoveAmount = 20 * (i + 1);
                }
                GetSkillInfo(currentPlayer.skills[i]);
                cursorMoveAmount = cursorMoveAmount - 10;
                Console.CursorVisible = false;
                int center = skillName.Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop - 5);
                Console.WriteLine(skillName + "\r\n");
                center = skillDescription[0].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
                Console.WriteLine(skillDescription[0]);
                center = skillDescription[1].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
                Console.WriteLine(skillDescription[1] + "\r\n");
                Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                for (int j = 0; j < 4; j++)
                {
                    Console.Write("|");
                    Console.Write(activeSkillsIcon[j]);
                    Console.WriteLine("|  ");
                    Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                }
                Console.WriteLine("|----------|  ");
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 6);
            }
        }

        static int SelectSkill()
        {
            Console.Clear();

            //All available Skills 
            List<allSkills> allAvSkills = currentPlayer.GetAllAvailableSkills();
            GetSkillInfo(allAvSkills[selectedSkill]);

            //Top Text
            string topOfScreenText = "Select a skill you would like to have";
            int center = topOfScreenText.Length / 2;
            int cursorMoveAmount = 30;

            Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
            Console.WriteLine(topOfScreenText);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 5);

            center = skillName.Length / 2;
            Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop - 5);
            Console.WriteLine(skillName + "\r\n");
            center = skillDescription[0].Length / 2;
            Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
            Console.WriteLine(skillDescription[0]);
            center = skillDescription[1].Length / 2;
            Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
            Console.WriteLine(skillDescription[1]);

            //Selected box
            Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
            Console.WriteLine("|----------|  ");
            Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
            for (int j = 0; j < 4; j++)
            {
                Console.Write("|");
                Console.Write(activeSkillsIcon[j]);
                Console.WriteLine("|  ");
                Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
            }
            Console.WriteLine("|----------|  ");
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 6);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 6);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);

            //Other boxes
            int totalSkillsToGo = allAvSkills.Count - (pagesDown * 5);
            int skills = 5;
            //Writes rows
            for (int j = 0; j < 2; j++)
            {
                if (j == 1)
                {
                    Console.WriteLine();
                }
                if (totalSkillsToGo < 5)
                {
                    //Remaining boxes
                    skills = totalSkillsToGo;
                }
                for (int i = 0; i < skills; i++)
                {
                    //Writes boxes
                    int skillNumber = i + (pagesDown * 5);
                    GetSkillInfo(allAvSkills[skillNumber + (j * 5)]);
                    cursorMoveAmount = 15 * (i + 1);
                    cursorMoveAmount = cursorMoveAmount - 15;
                    Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                    Console.WriteLine("|----------|  ");
                    Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                    for (int n = 0; n < 4; n++)
                    {
                        Console.Write("|");
                        if (selectedSkill == skillNumber + (j * 5))
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.Write(activeSkillsIcon[n]);
                        if (selectedSkill == skillNumber + (j * 5))
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("|  ");
                        Console.SetCursorPosition(Console.CursorLeft + cursorMoveAmount, Console.CursorTop);
                    }
                    Console.WriteLine("|----------|  ");
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 6);
                }
                totalSkillsToGo -= 5;
                skills = 5;
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 6);
            }
            //Bottom Line
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================");
            Console.ForegroundColor = ConsoleColor.White;

            //Confirm text
            string confirmText = "Press ENTER to confirm";
            center = confirmText.Length / 2;
            cursorMoveAmount = 30;
            Console.SetCursorPosition(Console.CursorLeft + ((cursorMoveAmount + 6) - center), Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(confirmText);
            Console.ForegroundColor = ConsoleColor.White;

            return allAvSkills.Count;

        }
        public static void MoveSelectedSkill(int totalSkills, int skillNumber)
        {
            //If you press WASD it moves the selected skill, If you press ENTER it sets your skill
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.A)
            {
                selectedSkill--;
            }
            else if (key == ConsoleKey.D)
            {
                selectedSkill++;
            }
            else if (key == ConsoleKey.S)
            {
                selectedSkill += 5;
                if (selectedSkill >= totalSkills)
                {
                    selectedSkill = totalSkills - 1;
                }
            }
            else if (key == ConsoleKey.W)
            {
                selectedSkill -= 5;
                if (selectedSkill <= -1)
                {
                    selectedSkill += 5;
                }
            } else if (key == ConsoleKey.Enter)
            {
                List<allSkills> allAvSkills = currentPlayer.GetAllAvailableSkills();
                currentPlayer.skills[skillNumber] = allAvSkills[selectedSkill];
                selectedSkill = 0;
            }

            if (selectedSkill >= totalSkills)
            {
                selectedSkill = 0;
            } else if (selectedSkill <= -1)
            {
                selectedSkill = totalSkills - 1;
            }

            if (selectedSkill < pagesDown * 5)
            {
                pagesDown = 0;
            }
            if (selectedSkill > (pagesDown * 5) + 4)
            {
                pagesDown++;
            }

        }
        //Puts the skills Icon, Name and Description into activeSkillsIcon, skillName and skillDescription
        static void GetSkillInfo(allSkills skill)
        {
            //All skill icons
            string[][] iconArray = new string[][]{
                //0 WIP
                new string[4]{
                    "          ",
                    "   w.i.p  ",
                    "   w.i.p  ",
                    "          " },
                //1 Violent
                new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "},
                //2 Healthy
                new string[4]{
                    "   _  _   ",
                    "  | \\/ |  ",
                    "   \\  /   ",
                    "    \\/    "},
                //3 Accurate?
                new string[4]{
                    "   - ---/\\",
                    " - ----/ /",
                    "- ----/ / ",
                    "-- --/ /  "},
                //4 Not Bag
                new string[4]{
                    "   |  |   ",
                    "   |‾-|   ",
                    "  /    \\  ",
                    " /______\\ "},
                //5 Stone Wall
                new string[4]{
                    "‾|‾‾|‾‾|‾‾",
                    "‾‾|‾‾|‾‾|‾",
                    "‾|‾‾|‾‾|‾‾",
                    "‾‾|‾‾|‾‾|‾"},
                //6 Glass Cannon
                new string[4]{
                    "    ||    ",
                    "____||____",
                    "‾‾‾‾||‾‾‾‾",
                    "    ||    "},
                //7 Fast Learner
                new string[4]{
                    " \\  / |‾\\ ",
                    "  \\/  |  |",
                    "  /\\  |_/ ",
                    " /  \\ |   "},
                //8 rng lucky
                new string[4]{
                    "  _/‾‾\\_  ",
                    " / /‾‾\\ \\ ",
                    " \\ \\__/ / ",
                    "  ‾\\__/‾  "},
                //9 Fast
                new string[4]{
                    "- --- --| ",
                    " - ---- / ",
                    "- --- --\\ ",
                    "-- -----| "},
                //10 Heavy hit
                new string[4]{
                    "   |‾‾|   ",
                    "  /‾‾‾‾\\  ",
                    " |1000kg| ",
                    " |______| "},
                //11 Light hit
                new string[4]{
                    "   /‾‾\\   ",
                    "   \\__/   ",
                    "    /     ",
                    "    \\     "} };
            switch (skill)
            {
                //Change icon,name and description based on skill
                case Creature.allSkills.Healthy:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[2][j];
                    }
                    skillName = "Healthy";
                    skillDescription[0] = "+25 HP";
                    skillDescription[1] = "";
                    break;
                case Creature.allSkills.Violent:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[1][j];
                    }
                    skillName = "Violent";
                    skillDescription[0] = "+5 Damage";
                    skillDescription[1] = "";
                    break;
                case Creature.allSkills.Accurate:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[3][j];
                    }
                    skillName = "Accurate";
                    skillDescription[0] = "-5 chance ";
                    skillDescription[1] = "for an enemy to dodge";
                    break;
                case Creature.allSkills.Fast:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[9][j];
                    }
                    skillName = "Fast";
                    skillDescription[0] = "+3 Speed";
                    skillDescription[1] = "";
                    break;
                case Creature.allSkills.Fast_Learner:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[7][j];
                    }
                    skillName = "Fast Learner";
                    skillDescription[0] = "5% more XP";
                    skillDescription[1] = "";
                    break;
                case Creature.allSkills.Light_Hitter:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[11][j];
                    }
                    skillName = "Light Hitter";
                    skillDescription[0] = "1.2x";
                    skillDescription[1] = "light hit damage";
                    break;
                case Creature.allSkills.Heavy_Hitter:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[10][j];
                    }
                    skillName = "Heavy Hitter";
                    skillDescription[0] = "1.2x";
                    skillDescription[1] = "heavy hit damage";
                    break;
                case Creature.allSkills.Glass_Cannon:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[6][j];
                    }
                    skillName = "Glass Cannon";
                    skillDescription[0] = "1.5x Damage";
                    skillDescription[1] = "75% max HP";
                    break;
                case Creature.allSkills.Stone_Wall:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[5][j];
                    }
                    skillName = "Stone Wall";
                    skillDescription[0] = "1.5x max Health";
                    skillDescription[1] = "75% Damage";
                    break;
                case Creature.allSkills.NotBag:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[4][j];
                    }
                    skillName = "Not Bag";
                    skillDescription[0] = "Bag deals damage!!";
                    skillDescription[1] = "0 damage!!";
                    break;
                case Creature.allSkills.rngLucky:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[8][j];
                    }
                    skillName = "Lucky";
                    skillDescription[0] = "All rng stats";
                    skillDescription[1] = "are 2 higher";
                    break;
            }
        } 
        private static void DrawBattle(ConsoleColor color,Creature enemy)
        {
            Console.Clear();
            //Max size: 71L , 26T

            //Lines
            Console.ForegroundColor = color;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("  --------------------------------------------------------------------  ");
            Console.WriteLine("  |                    |             |          |                    |  ");
            Console.WriteLine("  --------------------------------------------------------------------  ");
            for(int i = 0; i < 6; i++)
            {
                Console.WriteLine("  |                    |");
            }

            Console.ForegroundColor= ConsoleColor.White;
            //Text
            int unusedDodge;
            int healing = 0;
            Console.SetCursorPosition(61, 4);
            Console.SetCursorPosition(27, 1);
            Console.WriteLine("Attacks");
            Console.SetCursorPosition(40, 1);
            Console.WriteLine("Items");
/*            Console.SetCursorPosition(26, 3);
            Console.WriteLine("Attack");
            Console.SetCursorPosition(36, 3);
            Console.WriteLine(":");
            Console.SetCursorPosition(38, 3);
            Console.WriteLine("Dmg");*/
            string textAb = "Attacks";
            int centerAb = textAb.Length / 2;
            Console.SetCursorPosition(30 - centerAb, 3);
            Console.Write(textAb);
            Console.SetCursorPosition(37, 3);
            Console.Write(":");
            Console.SetCursorPosition(40, 3);
            Console.Write("Dmg");
            Console.SetCursorPosition(44, 3);
            Console.Write(":");
            Console.SetCursorPosition(46, 3);
            Console.Write("HP");
            int amountMoved = 0;
            for(int i = 5;i < currentPlayer.characterAttacks.Length + 5; i++)
            {
                string text = "";
                if (currentPlayer.characterAttacks[i-5] != attacks.None)
                {
                    switch (currentPlayer.characterAttacks[i - 5])
                    {
                        case attacks.Light_Hit:
                            text = "Light hit";
                            break;
                        case attacks.Heavy_Hit:
                            text = "Heavy hit";
                            break;
                        case attacks.BG_NO:
                            text = "Bag";
                            break;
                        case attacks.HL_LifeSteal:
                            text = "Lifesteal";
                            break;
                        case attacks.RG_Stats:
                            text = "Random Stats";
                            break;
                    }
                    int center = text.Length / 2;
                    Console.SetCursorPosition(30 - center, i);
                    Console.WriteLine(text);
                    Console.SetCursorPosition(37, i);
                    Console.WriteLine(":");
                    Console.SetCursorPosition(40, i);
                    switch (currentPlayer.characterAttacks[i - 5])
                    {
                        case attacks.RG_Stats:
                            Console.SetCursorPosition(39, i);
                            float nerf = 0.05f;
                            Console.WriteLine((int)(((5 + currentPlayer.rng_ExtraLuck) * nerf * currentPlayer.damage))+"-" + (int)(((30 + currentPlayer.rng_ExtraLuck) * nerf * currentPlayer.damage)));
                            break;
                        default:
                            Console.WriteLine(currentPlayer.Attack(out unusedDodge, out healing, i - 4,false));
                            break;

                    }
                    Console.SetCursorPosition(44, i);
                    Console.Write(":");
                    Console.SetCursorPosition(46, i);
                    Console.Write(healing);

                }
                else
                {
                    amountMoved++;
                }
            }
            textAb = "Ability";
            centerAb = textAb.Length / 2;
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
            Console.SetCursorPosition(30 - centerAb, Console.CursorTop);
            Console.Write(textAb);
            Console.SetCursorPosition(37, Console.CursorTop);
            Console.Write(":");
            Console.SetCursorPosition(40, Console.CursorTop);
            Console.Write(currentPlayer.Attack(out unusedDodge, out healing, 5, false));
            Console.SetCursorPosition(44, Console.CursorTop);
            Console.Write(":");
            Console.SetCursorPosition(46, Console.CursorTop);
            Console.Write(healing);
            //Console.WriteLine("Attacks");

            //Health bars
            Console.SetCursorPosition(3, 1);
            float maxHP = enemy.maxHealth;
            float curHP = enemy.health;
            float percent = (100/maxHP) * curHP;
            percent /= 5;
            if(percent > 20)
            {
                percent = 20;
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 0;i < percent; i++)
            {
                Console.Write("█");
            }

            maxHP = currentPlayer.maxHealth;
            curHP = currentPlayer.health;
            percent = (100 / maxHP) * curHP;
            percent /= 5;
            if (percent > 20)
            {
                percent = 20;
            }
            Console.SetCursorPosition((int)(69f - percent), 1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < percent; i++)
            {
                Console.Write("█");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}