﻿using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using static TBRPGV2.Creature;

namespace TBRPGV2
{
    internal class Program
    {
        #region Skills
        //Skills
        const bool chooseStartSkills = false;
        public const int amountStartSkills = 3;
        #endregion
        #region Player, Classes
        //Player , Enemy test class , what class is selected
        static Creature currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
        static Creature.allClasses forceEnemyClass = Creature.allClasses.Class_None;
        static public Creature currentEnemy;
        static int classSelection = 0;
        #endregion
        #region GetSkillInfo Info
        //What GetSkillInfo(); changes
        static string skillName = "";
        static string[] skillDescription = { "", "", "", "", "" };
        static string[] activeSkillsIcon = new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /    ",
                    "  / /     "};
        #endregion
        #region UI stuff
        //Needed for UI
        static bool[] creatureDodgedLastRound = new bool[2];

        //Selected skill in the level 20 skill selection screen
        static int pagesDown = 0;
        static int selectedSkill = 0;

        //Selected ui for battle
        static int selectedTopic = 0;
        static int selectedAttack = 0;
        #endregion

        //Few stats, Put amountOfClasses to 6 for Bag class
        public static int amountOfClasses = 5;
        #region Files
        //File stuff
        private static JsonSerializerOptions _options =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true };
        //Quick save file name
        static string playerFileName = "Saves/QuickPlayerSave.json";
        static string[] playerManualSaveNames = { "Saves/PlayerSave1.json", "Saves/PlayerSave2.json", "Saves/PlayerSave3.json" };
        static int selectedSave = 0;
        static int currentPlayingSave = 0;
        static List<int> notSaveFiles = new List<int>();
        #endregion

        static void Main(string[] args)
        {
            //Some Misc things
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;
            Console.Title = "TBRPG";
            StartMenu();
            LoadPlayer();
            Console.Clear();
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
                            QuickSavePlayer();
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
                //currentPlayer.health = currentPlayer.maxHealth;
                currentPlayer.abilityUses = 0;
                StartBattle();
                QuickSavePlayer();

            }
            while (true);
        }

        #region Character Creation
        static void NewCharacter()
        {
            currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
            //Sets up the player
            while (currentPlayer.currentClass == Creature.allClasses.Class_None)
            {
                SelectClass();
            }
            DecideCreatureSkills(currentPlayer, !chooseStartSkills, amountStartSkills);
            Console.Clear();
            ShowRandomSkills();
            Console.ReadKey();
            Console.Clear();

            //Makes the player stats
            currentPlayer.RecalculateStats();
            currentPlayer.GiveBaseAttacks();
            currentPlayer.health = currentPlayer.maxHealth;
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
            Console.WriteLine("------------------------------------------------------------------------\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("------------------------------------------------------------------------\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("------------------------------------------------------------------------\r\n");
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
                            word = "";
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
            Console.WriteLine("------------------------------------------------------------------------");

            //Writes the ENTER
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
                switch (key)
                {
                    case ConsoleKey.A:
                        classSelection--;
                        break;
                    case ConsoleKey.D:
                        classSelection++;
                        break;
                    case ConsoleKey.Enter:
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
                        break;
                }

                //Makes it go loop
                if (classSelection == amountOfClasses)
                {
                    classSelection = 0;
                }
                else if (classSelection <= -1)
                {
                    classSelection = amountOfClasses - 1;
                }
            }

        }

        static void DecideCreatureSkills(Creature crt, bool random, int amountOfSkills)
        {
            for (int i = 0; i < amountOfSkills; i++)
            {
                if (random)
                {
                    crt.skills[i] = crt.GetRandomSkill();
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

        //Shows what skills you got at the start of the game
        static void ShowRandomSkills()
        {

            Console.ForegroundColor = ConsoleColor.White;
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
        #endregion

        #region Battle
        static void StartBattle()
        {
            //Makes the enemy
            Creature enemy = new Creature(allClasses.Class_None, currentPlayer.currentLevel);
            Enemy enemyBrain = new Enemy(enemy, currentPlayer);
            currentEnemy = enemy;
            if(forceEnemyClass != allClasses.Class_None)
            {
                enemy.currentClass = forceEnemyClass;
            }
            else
            {
                enemy.currentClass = enemyBrain.ChooseClass();
            }
            enemyBrain.ChooseExtraSkill();

            //Recalculate Stats
            enemy.RecalculateStats();
            currentPlayer.RecalculateStats();
            enemy.health = enemy.maxHealth;

            bool activeBattle = true;
            DrawText(enemy, ConsoleColor.DarkGray);
            DrawBattle(ConsoleColor.DarkGray);
            Thread.Sleep(500);
            while (activeBattle)
            {
                Console.Clear();
                if(currentPlayer.health > currentPlayer.maxHealth / 5)
                {
                    DrawBattle(ConsoleColor.DarkGray);
                }
                else
                {
                    DrawBattle(ConsoleColor.DarkRed);
                }
                DrawText(enemy);
                Console.Title = "TBRPG";
                bool selectingAttack = true;

                //Makes some ints
                int selectedAttack = -1;
                int healing = 0;
                int damage = 0;
                int dodge = 0;
                bool dodged;

                while (selectingAttack)
                {
                    //ConsoleKey key = Console.ReadKey(true).Key;
                    AddChargerCharge(enemy);
                    while(selectedAttack == -1)
                    {
                        DrawText(enemy);
                        selectedAttack = AttackMenuInput(enemy);

                    }
                    damage = currentPlayer.Attack(out dodge,out healing, selectedAttack);
                    if (damage != -100)
                    {
                        selectingAttack = false;
                        if(healing > 50)
                        {
                            if (currentPlayer.health > currentPlayer.maxHealth)
                            {
                                currentPlayer.health = currentPlayer.maxHealth;
                            }
                            DrawBattle(ConsoleColor.DarkGreen);
                            Thread.Sleep(200);
                            DrawBattle(ConsoleColor.Green);
                            Thread.Sleep(200);
                            DrawBattle(ConsoleColor.DarkGreen);
                            Thread.Sleep(200);
                        }else if(healing > 0)
                        {
                            if (currentPlayer.health > currentPlayer.maxHealth)
                            {
                                currentPlayer.health = currentPlayer.maxHealth;
                            }
                            DrawBattle(ConsoleColor.Green);
                            Thread.Sleep(200);
                        }
                    }
                }

                //Enemy Dodging
                #region Enemy Dodge
                dodged = CalculateDodge(enemy, currentPlayer, dodge);
                if (!dodged)
                {
                    enemy.health -= damage;
                    creatureDodgedLastRound[1] = false;
                }
                else
                {
                    foreach(allSkills skill in enemy.skills)
                    {
                        if(skill == allSkills.Right_Back)
                        {
                            enemy.health -= damage / 3;
                            currentPlayer.health -= damage / 3;
                        }
                    }
                    creatureDodgedLastRound[1] = true;
                }
                #endregion
                //Death check
                if (enemy.health < 1)
                {
                    enemy.health = 0;

                    //Death animation thingy
                    Console.Clear();
                    DrawText(enemy, ConsoleColor.DarkYellow);
                    DrawBattle(ConsoleColor.DarkYellow);
                    Thread.Sleep(500);
                    DrawText(enemy, ConsoleColor.DarkGray);
                    DrawBattle(ConsoleColor.DarkGray);
                    Thread.Sleep(200);
                    DrawText(enemy, ConsoleColor.Black);
                    DrawBattle(ConsoleColor.Black);
                    Thread.Sleep(500);
                    Thread.Sleep(1000);

                    //Add xp, make sure you cant charger charge over multiple battles (rip mike strats)
                    currentPlayer.AddXP(CalculateXPGain(currentPlayer, enemy));
                    currentPlayer.chargerCharge = 0;

                    //Makes sure your health gets set to your new max health later
                    currentPlayer.RecalculateStats();
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
                    creatureDodgedLastRound[0] = false;
                }
                else
                {
                    foreach (allSkills skill in enemy.skills)
                    {
                        if (skill == allSkills.Right_Back)
                        {
                            enemy.health -= damage / 3;
                            currentPlayer.health -= damage / 3;
                        }
                    }
                    creatureDodgedLastRound[0] = true;
                }
                #endregion
                //Death check
                if (currentPlayer.health < 1)
                {
                    currentPlayer.health = 0;
                    Console.Clear();
                    DrawText(enemy,ConsoleColor.DarkGray);
                    DrawBattle(ConsoleColor.DarkGray);
                    Thread.Sleep(1000);
                    DrawText(enemy, ConsoleColor.Black);
                    DrawBattle(ConsoleColor.Black);
                    Thread.Sleep(1000);
                    activeBattle = false;
                    currentPlayer.chargerCharge = 0;
                    currentPlayer.health = currentPlayer.maxHealth;
                } else if (currentPlayer.health > currentPlayer.maxHealth)
                {
                    currentPlayer.health = currentPlayer.maxHealth;
                }
            }
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
        public static void DrawBattle(ConsoleColor color)
        {
            //Max size: 71L , 26T

            //Lines
            Console.ForegroundColor = color;
            Console.SetCursorPosition(70, 26);
            Console.WriteLine("-");
            Console.SetCursorPosition(0, 0);

            Console.Write("  --------------------------------------------------------------------  ");
            Console.SetCursorPosition(2, 1);
            Console.Write("|");
            Console.SetCursorPosition(69, 1);
            Console.Write("|");
            Console.SetCursorPosition(0, 2);
            Console.Write("  --------------------------------------------------------------------  ");
            Console.SetCursorPosition(2, 3);
            Console.WriteLine("|");
            Console.SetCursorPosition(10, 3);
            Console.WriteLine("|");
            Console.SetCursorPosition(2, 4);
            Console.WriteLine("---------");
            Console.SetCursorPosition(61, 3);
            Console.WriteLine("|");
            Console.SetCursorPosition(69, 3);
            Console.WriteLine("|");
            Console.SetCursorPosition(61, 4);
            Console.WriteLine("---------");


            Console.SetCursorPosition(2, 14);
            Console.WriteLine("---------");
            Console.SetCursorPosition(2, 15);
            Console.WriteLine("|");
            Console.SetCursorPosition(10, 15);
            Console.WriteLine("|");
            Console.SetCursorPosition(61, 15);
            Console.WriteLine("|");
            Console.SetCursorPosition(69, 15);
            Console.WriteLine("|");
            Console.SetCursorPosition(61, 14);
            Console.WriteLine("---------");
            Console.SetCursorPosition(0, 16);
            Console.WriteLine("  --------------------------------------------------------------------  ");
            Console.SetCursorPosition(2, 17);
            Console.WriteLine("|");
            Console.SetCursorPosition(69, 17);
            Console.Write("|");
            Console.SetCursorPosition(0, 18);
            Console.WriteLine("  --------------------------------------------------------------------  ");
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("  --------------------------------------------------------------------  ");

            Console.SetCursorPosition(0, 19);
            for (int i = 0; i < 7; i++)
            {
                Console.SetCursorPosition(2, i + 19);
                Console.WriteLine("|");
                Console.SetCursorPosition(12, i + 19);
                Console.WriteLine("|");
                Console.SetCursorPosition(53, i + 19);
                Console.WriteLine("|");
                Console.SetCursorPosition(69, i + 19);
                Console.WriteLine("|");
            }
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void DrawText(Creature enemy, ConsoleColor color = ConsoleColor.White)
        {

            Console.ForegroundColor = color;

            //Health text
            Console.SetCursorPosition(3, 15);
            Console.WriteLine($"HP: {currentPlayer.health}");

            Console.SetCursorPosition(3, 3);
            Console.WriteLine($"HP: {enemy.health}");

            //Dodge text
            if (creatureDodgedLastRound[0] == true)
            {
                Console.SetCursorPosition(62, 15);
                Console.WriteLine("Dodged");
            }

            if (creatureDodgedLastRound[1] == true)
            {
                Console.SetCursorPosition(62, 3);
                Console.WriteLine("Dodged");
            }

            Console.SetCursorPosition(3, 20);

            //Selected catogory
            if (selectedTopic == 0 && color == ConsoleColor.White)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(">Attacks ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = color;
            }
            else
            {
                Console.WriteLine(" Attacks ");
            }
            Console.SetCursorPosition(3, 23);
            if (selectedTopic == 1 && color == ConsoleColor.White)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("> Items  ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = color;
            }
            else
            {
                Console.WriteLine("  Items  ");
            }

            //Attacks menu
            if (selectedTopic == 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    Console.SetCursorPosition(13, 19 + i);
                    Console.WriteLine("                                        ");
                }
                int[,] locations = { { 17, 20 }, { 39, 20 }, { 17, 23 }, { 39, 23 } };
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(locations[i, 0], locations[i, 1]);
                    string text = "";
                    //What attack and which one you have selected
                    switch (currentPlayer.characterAttacks[i])
                    {
                        case attacks.Light_Hit:
                            text = "Light attack";
                            break;
                        case attacks.Heavy_Hit:
                            text = "Heavy attack";
                            break;
                        case attacks.HL_LifeSteal:
                            text = "Lifesteal";
                            break;
                        case attacks.RG_Stats:
                            text = "Random Stats";
                            break;
                        case attacks.BG_NO:
                            text = "Bag, No.";
                            break;
                        case attacks.DD_HPSacrifice:
                            text = "HP Sacrifice";
                            break;
                    }
                    if (selectedAttack == i && color == ConsoleColor.White)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(">" + text);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(" " + text);
                    }
                }
                if (currentPlayer.roundsUntilAbilityRecharge <= 0 && currentPlayer.abilityUses < currentPlayer.maxAbilityUses)
                {
                    //Class ability if you can use it
                    Console.SetCursorPosition(25, 25);
                    if (selectedAttack == 4 && color == ConsoleColor.White)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(25, 25);
                        Console.Write(">Class ability");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(" Class ability");
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    Console.SetCursorPosition(54, 19 + i);
                    Console.WriteLine("               ");
                }
                Console.SetCursorPosition(54, 19);
                int unusedDodge = 0;
                int unusedHealing = 0;
                #region Attack Descriptions
                if (currentPlayer.currentClass != allClasses.RNG)
                {
                    if (selectedAttack != 4)
                    {
                        switch (currentPlayer.characterAttacks[selectedAttack])
                        {
                            case attacks.Light_Hit:
                                Console.WriteLine("Light Attack");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Light_Hit, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Dodge: {unusedDodge}");
                                break;

                            case attacks.Heavy_Hit:
                                Console.WriteLine("Heavy Attack");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Heavy_Hit, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Dodge: {unusedDodge}");
                                break;

                            case attacks.RG_Stats:
                                Console.WriteLine("You should not be able to have this tf");
                                break;
                            case attacks.BG_NO:
                                Console.WriteLine("Bag, No.");
                                break;
                            case attacks.HL_LifeSteal:
                                Console.WriteLine("Lifesteal");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.HL_LifeSteal, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Healing: {unusedHealing}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Dodge: {unusedDodge}");
                                break;
                            case attacks.DD_HPSacrifice:
                                Console.WriteLine("HP Sacrifice");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.DD_HPSacrifice, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Hurt: {-unusedHealing}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Dodge: {unusedDodge}");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Class Ability");
                        switch (currentPlayer.currentClass)
                        {
                            case allClasses.DamageDealer:
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Class_Ability, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine("Cant Dodge");
                                break;
                            case allClasses.Tank:
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Class_Ability, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Healing: {unusedHealing}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine("Cant Dodge");
                                break;
                            case allClasses.Healer:
                                Console.SetCursorPosition(54, Console.CursorTop);
                                currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Class_Ability, true);
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Healing: {unusedHealing}");
                                break;
                            case allClasses.Charger:
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Class_Ability, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine("Cant Dodge");
                                break;
                        }
                    }
                }
                else
                {
                    if (selectedAttack != 4)
                    {
                        switch (currentPlayer.characterAttacks[selectedAttack])
                        {
                            case attacks.Light_Hit:
                                Console.WriteLine("Light Attack");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Random damage ");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"around {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.Light_Hit, true)}");
                                break;
                            case attacks.RG_Stats:
                                Console.WriteLine("Random Stats");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Damage: {currentPlayer.Attack(out unusedDodge, out unusedHealing, 0, false, true, attacks.RG_Stats, true)}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Healing: {unusedHealing}");
                                Console.SetCursorPosition(54, Console.CursorTop);
                                Console.WriteLine($"Dodge: {unusedDodge}");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Class Ability");
                        Console.SetCursorPosition(54, Console.CursorTop);
                        Console.WriteLine("Does a ");
                        Console.SetCursorPosition(54, Console.CursorTop);
                        Console.WriteLine("random attack");
                    }
                }
                #endregion
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    Console.SetCursorPosition(13, 19 + i);
                    Console.WriteLine("                                        ");
                }
                int[,] locations = { { 17, 20 }, { 35, 20 }, { 17, 22 }, { 35, 22 }, { 17, 24 }, { 35, 24 } };
                Console.SetCursorPosition(10, 25);
                for(int i = 0;i < 6; i++)
                {
                    if (currentPlayer.itemsInInv[i] != null)
                    {
                        Item it = currentPlayer.itemsInInv[i];
                        if (selectedAttack == i && color == ConsoleColor.White)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                Console.SetCursorPosition(54, 19 + j);
                                Console.WriteLine("               ");
                            }
                            Console.SetCursorPosition(locations[i, 0], locations[i, 1]);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($">{it.itemName}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(54, 19);
                            Console.Write($"{it.itemName}");
                            for (int j = 0; j < it.description.Length; j++)
                            {
                                Console.SetCursorPosition(54, 21 + j);
                                Console.WriteLine(it.description[j]);
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(locations[i, 0], locations[i, 1]);
                            Console.Write($" {it.itemName}");
                        }
                    }

                }

            }


            //Health bars
            Console.SetCursorPosition(3, 1);
            float maxHP = enemy.maxHealth;
            float curHP = enemy.health;
            float percent = (100 / maxHP) * curHP;
            percent /= 1.52f;
            if (percent > 66)
            {
                percent = 66;
            }
            if (color == ConsoleColor.White)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("█");
            }
            maxHP = currentPlayer.maxHealth;
            curHP = currentPlayer.health;
            percent = (100 / maxHP) * curHP;
            percent /= 1.52f;
            if (percent > 66)
            {
                percent = 66;
            }
            Console.SetCursorPosition(3, 17);
            if (color == ConsoleColor.White)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            for (int i = 0; i < percent; i++)
            {
                Console.Write("█");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static int AttackMenuInput(Creature enemy)
        {
            bool selected = false;
            while (selected == false)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int lowestAttack = 0;
                if (currentPlayer.roundsUntilAbilityRecharge <= 0 && currentPlayer.abilityUses < currentPlayer.maxAbilityUses)
                {
                    lowestAttack = 4;
                }
                else
                {
                    for (int i = 3; i > -1; i--)
                    {
                        if (currentPlayer.characterAttacks[i] != attacks.None)
                        {
                            lowestAttack = i;
                            break;
                        }
                    }
                }
                switch (key)
                {
                    case ConsoleKey.S:
                        selectedTopic--;
                        selectedAttack = 0;
                        selected = true;
                        break;
                    case ConsoleKey.W:
                        selectedTopic++;
                        selectedAttack = 0;
                        selected = true;
                        break;
                    case ConsoleKey.A:
                        selectedAttack--;
                        if(selectedTopic == 0)
                        {
                            if (selectedAttack > lowestAttack)
                            {
                                selectedAttack = 0;
                            }
                            else if (selectedAttack < 0)
                            {
                                selectedAttack = lowestAttack;
                            }
                            else if (currentPlayer.characterAttacks[selectedAttack] == attacks.None)
                            {
                                for (int i = 3; i > -1; i--)
                                {
                                    if (currentPlayer.characterAttacks[i] != attacks.None)
                                    {
                                        lowestAttack = i;
                                        break;
                                    }
                                }
                                selectedAttack = lowestAttack;
                            }
                        }
                        else
                        {
                            bool hasSpot = true;
                            for(int i = 0; i < 6; i++)
                            {
                                if (currentPlayer.itemsInInv[i] != null)
                                {
                                    hasSpot = false;
                                }
                            }
                            bool firstTime = true;
                            while (!hasSpot)
                            {
                                if (!firstTime)
                                {
                                    selectedAttack--;
                                }
                                else
                                {
                                    firstTime = false;
                                }
                                if (selectedAttack > 5)
                                {
                                    selectedAttack = 0;
                                }
                                else if (selectedAttack < 0)
                                {
                                    selectedAttack = 5;
                                }
                                if (currentPlayer.itemsInInv[selectedAttack] != null)
                                {
                                    hasSpot = true;
                                }
                            }
                        }
                        
                        selected = true;
                        break;
                    case ConsoleKey.D:
                        selectedAttack++;
                        if(selectedTopic == 0)
                        {
                            if (selectedAttack > lowestAttack)
                            {
                                selectedAttack = 0;
                            }
                            else if (selectedAttack < 0)
                            {
                                selectedAttack = lowestAttack;
                            }
                            else if (currentPlayer.characterAttacks[selectedAttack] == attacks.None)
                            {
                                selectedAttack = 4;
                            }
                        }
                        else
                        {
                            bool hasSpot = true;
                            for (int i = 0; i < 6; i++)
                            {
                                if (currentPlayer.itemsInInv[i] != null)
                                {
                                    hasSpot = false;
                                }
                            }
                            bool firstTime = true;
                            while (!hasSpot)
                            {
                                if (!firstTime)
                                {
                                    selectedAttack++;
                                }
                                else
                                {
                                    firstTime = false;
                                }
                                if (selectedAttack > 5)
                                {
                                    selectedAttack = 0;
                                }
                                else if (selectedAttack < 0)
                                {
                                    selectedAttack = 5;
                                }
                                if (currentPlayer.itemsInInv[selectedAttack] != null)
                                {
                                    hasSpot = true;
                                }
                            }
                        }
                        selected = true;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedTopic == 0)
                        {
                            int number = selectedAttack + 1;
                            if (selectedAttack == 4)
                            {
                                selectedAttack = 0;
                            }
                            return number;
                        }
                        else
                        {
                            if(selectedAttack <= 5 && selectedAttack >= 0)
                            {
                                if (currentPlayer.itemsInInv[selectedAttack] != null)
                                {
                                    currentPlayer.itemsInInv[selectedAttack].UseItem(currentPlayer);
                                    currentPlayer.itemsInInv[selectedAttack].uses -= 1;
                                    if (currentPlayer.itemsInInv[selectedAttack].uses <= 0)
                                    {
                                        currentPlayer.itemsInInv[selectedAttack] = null;
                                        bool hasSpot = true;
                                        for (int i = 0; i < 6; i++)
                                        {
                                            if (currentPlayer.itemsInInv[i] != null)
                                            {
                                                hasSpot = false;
                                            }
                                        }
                                        bool firstTime = true;
                                        while (!hasSpot)
                                        {
                                            if (!firstTime)
                                            {
                                                selectedAttack++;
                                            }
                                            else
                                            {
                                                firstTime = false;
                                            }
                                            if (selectedAttack > 5)
                                            {
                                                selectedAttack = 0;
                                            }
                                            else if (selectedAttack < 0)
                                            {
                                                selectedAttack = 5;
                                            }
                                            if (currentPlayer.itemsInInv[selectedAttack] != null)
                                            {
                                                hasSpot = true;
                                            }
                                        }
                                    }
                                }
                            
                            
                            }
                            return -1;
                        }
                    case ConsoleKey.D0:
                        currentPlayer.health = 0;
                        return -2;
                    case ConsoleKey.M:
                        ManualSaveUI();
                        Console.Clear();
                        DrawText(enemy);
                        DrawBattle(ConsoleColor.DarkGray);
                        break;
                }
                if (selectedTopic > 1)
                {
                    selectedTopic = 0;
                }
                else if (selectedTopic < 0)
                {
                    selectedTopic = 1;
                }
            }
            return -1;
        }
        #endregion
        #region After Battle
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
        #endregion
        #region Selecting Skills
        static public int SelectSkill()
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
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
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
                    "    \\     "},
                new string[4]{
                    "‾-_   |‾‾|",
                    "   ‾-_|‾|‾",
                    "   _-‾|‾‾|",
                    "_-‾   |‾|‾"} };
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
                case Creature.allSkills.RngLucky:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[8][j];
                    }
                    skillName = "Lucky";
                    skillDescription[0] = "All rng stats";
                    skillDescription[1] = "are 2 higher";
                    break;
                case Creature.allSkills.Right_Back:
                    for (int j = 0; j < 4; j++)
                    {
                        activeSkillsIcon[j] = iconArray[12][j];
                    }
                    skillName = "Right Back At You";
                    skillDescription[0] = "Dont block, Parry";
                    skillDescription[1] = "Both combatants take 33% damage";
                    break;
            }
        }
        #endregion


        #region Save/Load
        static void QuickSavePlayer()
        {
            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }
            string jsonString = "";
            if(currentPlayingSave >= 1)
            {
                jsonString = JsonSerializer.Serialize(currentPlayer, _options);
                File.WriteAllText(playerManualSaveNames[currentPlayingSave-1], jsonString);
            }
            else
            {
                jsonString = JsonSerializer.Serialize(currentPlayer, _options);
                File.WriteAllText(playerFileName, jsonString);
            }
        }
        static void ManualSavePlayer(int file)
        {
            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }
            currentPlayingSave = file + 1;
            string jsonString = "";
            if (currentPlayingSave >= 1)
            {
                jsonString = JsonSerializer.Serialize(currentPlayer, _options);
                File.WriteAllText(playerManualSaveNames[currentPlayingSave - 1], jsonString);
            }
            else
            {
                jsonString = JsonSerializer.Serialize(currentPlayer, _options);
                File.WriteAllText(playerFileName, jsonString);
            }
        }
        static void ManualSaveUI()
        {
            Creature[] saves = new Creature[4];
            string file;
            bool[] existingFiles = { false, false, false, false };
            if (File.Exists(playerFileName))
            {
                existingFiles[0] = true;
                file = File.ReadAllText(playerFileName);
                saves[0] = JsonSerializer.Deserialize<Creature>(file);

            }
            for (int i = 0; i < playerManualSaveNames.Length; i++)
            {
                if (File.Exists(playerManualSaveNames[i]))
                {
                    existingFiles[i + 1] = true;
                    file = File.ReadAllText(playerManualSaveNames[i]);
                    saves[1 + i] = JsonSerializer.Deserialize<Creature>(file);
                }
            }
            bool selecting = true;
            while (selecting)
            {
                Console.Clear();
                if (existingFiles[0])
                {
                    if (selectedSave == 0)
                    {
                        DrawBox(2, 2, saves[0]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Auto Save");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(2, 2, saves[0]);
                        Console.Write("Auto Save");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(2, 2, null);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (selectedSave == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Auto Save");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("Auto Save");
                    }
                    notSaveFiles.Add(0);
                }
                if (existingFiles[1])
                {
                    if (selectedSave == 1)
                    {
                        DrawBox(35, 2, saves[1]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 1");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(35, 2, saves[1]);
                        Console.Write("Save 1");
                    }
                }
                else
                {
                    notSaveFiles.Add(1);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(35, 2, null);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (selectedSave == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 1");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("Save 1");
                    }
                }
                if (existingFiles[2])
                {
                    if (selectedSave == 2)
                    {
                        DrawBox(2, 17, saves[2]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 2");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(2, 17, saves[2]);
                        Console.Write("Save 2");
                    }
                }
                else
                {
                    notSaveFiles.Add(2);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(2, 17, null);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (selectedSave == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 2");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("Save 2");
                    }
                }
                if (existingFiles[3])
                {
                    if (selectedSave == 3)
                    {
                        DrawBox(35, 17, saves[3]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 3");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(35, 17, saves[3]);
                        Console.Write("Save 3");
                    }
                }
                else
                {
                    notSaveFiles.Add(3);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(35, 17, null);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (selectedSave == 3)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 3");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("Save 3");
                    }
                }
                Console.SetCursorPosition(65, 29);
                if (selectedSave == 4)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(">Back");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write("Back");
                }
                //Draw stuff

                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D:
                        selectedSave++;
                        break;
                    case ConsoleKey.A:
                        selectedSave--; 
                        break;
                    case ConsoleKey.Enter:
                        if(selectedSave == 4)
                        {
                            selecting = false;
                            break;
                        }
                        ManualSavePlayer(selectedSave - 1);
                        selecting = false;
                        selectedSave = 0;
                        break;
                }
                if(selectedSave < 0)
                {
                    selectedSave = playerManualSaveNames.Length + 1;
                }else if (selectedSave > playerManualSaveNames.Length + 1)
                {
                    selectedSave = 0;
                }
            }
        }
        static void ManualLoadUI()
        {
            Creature[] saves = new Creature[4];
            string file;
            bool[] existingFiles = {false,false,false,false};
            if (File.Exists(playerFileName))
            {
                existingFiles[0] = true;
                file = File.ReadAllText(playerFileName);
                saves[0] = JsonSerializer.Deserialize<Creature>(file);
                
            }
            for (int i = 0; i < playerManualSaveNames.Length; i++)
            {
                if (File.Exists(playerManualSaveNames[i]))
                {
                    existingFiles[i+1] = true;
                    file = File.ReadAllText(playerManualSaveNames[i]);
                    saves[1+i] = JsonSerializer.Deserialize<Creature>(file);
                }
            }
            bool selecting = true;
            while (selecting)
            {
                Console.Clear();
                if (existingFiles[0])
                {
                    if (selectedSave == 0)
                    {
                        DrawBox(2, 2, saves[0]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Auto Save");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(2, 2, saves[0]);
                        Console.Write("Auto Save");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(2, 2, null);
                    Console.ForegroundColor = ConsoleColor.White;
                    notSaveFiles.Add(0);
                }
                if (existingFiles[1])
                {
                    if (selectedSave == 1)
                    {
                        DrawBox(35, 2, saves[1]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 1");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(35, 2, saves[1]);
                        Console.Write("Save 1");
                    }
                }
                else
                {
                    notSaveFiles.Add(1);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(35, 2, null);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (existingFiles[2])
                {
                    if (selectedSave == 2)
                    {
                        DrawBox(2, 17, saves[2]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 2");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(2, 17, saves[2]);
                        Console.Write("Save 2");
                    }
                }
                else
                {
                    notSaveFiles.Add(2);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(2, 17, null);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (existingFiles[3])
                {
                    if (selectedSave == 3)
                    {
                        DrawBox(35, 17, saves[3]);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(">Save 3");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        DrawBox(35, 17, saves[3]);
                        Console.Write("Save 3");
                    }
                }
                else
                {
                    notSaveFiles.Add(3);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    DrawBox(35, 17, null);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.SetCursorPosition(65, 29);
                if (selectedSave == 4)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(">Back");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write("Back");
                }
                //Draw stuff

                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D:
                        selectedSave++;
                        bool wrong = true;
                        while (wrong)
                        {
                            if (notSaveFiles.Contains(selectedSave)) 
                            {
                                selectedSave++;
                            }
                            else
                            {
                                if(selectedSave >= 6)
                                {
                                    selectedSave = 0;
                                }
                                else
                                {
                                    wrong = false;
                                }
                            }
                        }
                        break;
                    case ConsoleKey.A:
                        selectedSave--;
                        bool wrong2 = true;
                        while (wrong2)
                        {
                            if (notSaveFiles.Contains(selectedSave))
                            {
                                selectedSave--;
                            }
                            else
                            {
                                if (selectedSave < 0)
                                {
                                    selectedSave = 4;
                                }
                                else
                                {
                                    wrong2 = false;
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        for (int i = 0; i < saves[selectedSave].itemsInInv.Length; i++)
                        {
                            if (saves[selectedSave].itemsInInv[i] != null)
                            {
                                var serializedParent = JsonSerializer.Serialize(saves[selectedSave].itemsInInv[i]);
                                var it = new Item();
                                switch (saves[selectedSave].itemsInInv[i].itemName)
                                {
                                    case "Enchanted Chair":
                                        it = JsonSerializer.Deserialize<EnchChair>(serializedParent);
                                        saves[selectedSave].itemsInInv[i] = it;
                                        break;
                                    case "Reroll":
                                        it = JsonSerializer.Deserialize<SkillReroll>(serializedParent);
                                        saves[selectedSave].itemsInInv[i] = it;
                                        break;
                                }

                            }
                        }
                        switch (selectedSave)
                        {
                            case 0:
                                if (File.Exists(playerFileName))
                                {
                                    currentPlayer = saves[0];
                                    currentPlayingSave = 0;
                                }
                                else
                                {
                                    NewCharacter();
                                }
                                break;
                            case 1:
                                if (File.Exists(playerManualSaveNames[0]))
                                {
                                    currentPlayer = saves[1];
                                    currentPlayingSave = 1;
                                }
                                else
                                {
                                    NewCharacter();
                                }
                                break;
                            case 2:
                                if (File.Exists(playerManualSaveNames[1]))
                                {
                                    currentPlayer = saves[2];
                                    currentPlayingSave = 2;
                                }
                                else
                                {
                                    NewCharacter();
                                }
                                break;
                            case 3:
                                if (File.Exists(playerManualSaveNames[2]))
                                {
                                    currentPlayer = saves[3];
                                    currentPlayingSave = 3;
                                }
                                else
                                {
                                    NewCharacter();
                                }
                                break;
                            case 4: LoadPlayer();
                                break;
                        }
                        selecting = false;
                        selectedSave = 0;
                        break;
                }
                if (selectedSave < 0)
                {
                    selectedSave = playerManualSaveNames.Length + 1;
                }
                else if (selectedSave > playerManualSaveNames.Length + 1)
                {
                    selectedSave = 0;
                }
            }
        }
        static void LoadPlayer()
        {
            Start:
            try
            {
                Console.Clear();
                Console.SetCursorPosition(20,10);
                Console.WriteLine("Save file detected! Use it? Y/N");
                string allDices;
                Creature playerSave;
                bool hasSave = false;
                if (File.Exists(playerFileName) != false)
                {
                    allDices = File.ReadAllText(playerFileName);
                    playerSave = JsonSerializer.Deserialize<Creature>(allDices);
                    hasSave = true;
                }

                for (int i = 0; i < playerManualSaveNames.Length; i++)
                {
                    if (File.Exists(playerManualSaveNames[i]) != false)
                    {
                        allDices = File.ReadAllText(playerManualSaveNames[i]);
                        playerSave = JsonSerializer.Deserialize<Creature>(allDices);
                        hasSave = true;
                    }
                }

                if (hasSave)
                {
                    bool chose = false;
                    while (!chose)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.Y)
                        {
                            ManualLoadUI();
                            chose = true;
                        }
                        if (key == ConsoleKey.N)
                        {
                            chose = true;
                            NewCharacter();
                        }
                        if(key == ConsoleKey.S)
                        {
                            StartMenu();
                            goto Start;
                        }
                    }
                }
                else
                {
                    NewCharacter();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Thread.Sleep(5000);
                NewCharacter();
            }
        }
        static void DrawBox(int left, int top, Creature cret)
        {
            Console.SetCursorPosition(left, top);
            //⟋⟍
            Console.Write("⟋‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾⟍");
            for (int i = 0; i < 9; i++)
            {
                Console.SetCursorPosition(left, (top + 1) + i);
                Console.Write("|                           |");
            }
            Console.SetCursorPosition(left, top + 10);
            Console.Write("⟍___________________________⟋");
            if (cret != null)
            {
                string word = cret.currentClass.ToString();
                Console.SetCursorPosition(left + 15 - (word.Length / 2), top + 1);
                Console.Write(word);
                Console.SetCursorPosition(left + 1, top + 2);
                Console.Write("Attacks:");
                Console.SetCursorPosition(left + 1, top + 3);
                Console.Write(cret.characterAttacks[0]);
                Console.SetCursorPosition(left + 1, top + 4);
                Console.Write(cret.characterAttacks[1]);
                Console.SetCursorPosition(left + 1, top + 5);
                Console.Write(cret.characterAttacks[2]);
                Console.SetCursorPosition(left + 1, top + 6);
                Console.Write(cret.characterAttacks[3]);

                word = "Skills:";
                Console.SetCursorPosition(left + 28 - (word.Length), top + 2);
                Console.Write(word);
                word = cret.skills[0].ToString();
                Console.SetCursorPosition(left + 28 - (word.Length), top + 3);
                Console.Write(word);
                word = cret.skills[1].ToString();
                Console.SetCursorPosition(left + 28 - (word.Length), top + 4);
                Console.Write(word);
                word = cret.skills[2].ToString();
                Console.SetCursorPosition(left + 28 - (word.Length), top + 5);
                Console.Write(word);
                word = cret.skills[3].ToString();
                Console.SetCursorPosition(left + 28 - (word.Length), top + 6);
                Console.Write(word);

            }
            Console.SetCursorPosition(left + 12, top + 11);
        }
        #endregion
        static void StartMenu()
        {
            Console.Clear();
            int pos = 6;
            string[] icon =
            {
                "             ⟋⟍",
                "            / /‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾\\_",
                "   ⟋‾⟍     /  \\    ------ |‾\\ |‾\\ |‾\\  ⟋‾‾⟍      \\",
                "  |   |‾‾‾//\\ /      ||   |_/ |_/ |_/ /           \\",
                "  |   |___\\\\/ \\      ||   |‾\\ |\\  |   \\  __       /",
                "   ⟍_⟋     \\  /      ||   |_/ | \\ |    ⟍__|      /",
                "            \\ \\________________________________/‾",
                "             ⟍⟋"
            };
            for(int i = 0; i < icon.Length; i++)
            {
                Console.SetCursorPosition(7, pos + i);
                Console.WriteLine(icon[i]);
            }
            Console.SetCursorPosition(20, 17);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Press any button to continue");
            Console.ReadKey(true);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
    }
}