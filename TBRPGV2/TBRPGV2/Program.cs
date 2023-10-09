namespace TBRPGV2
{
    internal class Program
    {
        //Change so no empty
        //Player and starting class
        static Creature currentPlayer = new Creature(Creature.allClasses.Class_None, 0);
        static Creature.allClasses testingEnemyClass = Creature.allClasses.FireGuy;
        static int classSelection = 0;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            while(currentPlayer.currentClass == Creature.allClasses.Class_None)
            {
                SelectClass();
            }
            ShowRandomSkills();
            Console.ReadKey();
            Console.Clear();
            currentPlayer.RecalculateStats();
            currentPlayer.health = currentPlayer.maxHealth;
            Console.WriteLine($"Class: {currentPlayer.currentClass}");
            StartBattle(true);
            Console.ReadKey();
        }

        static void StartBattle(bool showStatsAtStart)
        {
            //Makes the enemy
            Creature enemy = new Creature(testingEnemyClass,0);
            Console.WriteLine($"Enemy Class: {enemy.currentClass}");
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
                if (enemy.health < 1)
                {
                    Console.WriteLine($"Enemy died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    activeBattle = false;
                }
                else if (enemy.health > enemy.maxHealth)
                {
                    enemy.health = enemy.maxHealth;
                }
                damage = enemy.Attack(out dodge, selectedAttack);
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
                if (currentPlayer.health < 1)
                {
                    Console.WriteLine($"Player died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    activeBattle = false;
                } else if(currentPlayer.health > currentPlayer.maxHealth)
                {
                    currentPlayer.health = currentPlayer.maxHealth;
                }
            }
        }
        static bool CalculateDodge(Creature creature, Creature creature2,int startingDodge)
        {
            int totalDodge = startingDodge;
            for(int i = 0; i < creature.skills.Length; i++)
            {
                if (creature.skills[i] == Creature.allSkills.Fast)
                {
                    totalDodge += 5;
                }
            }
            for (int i = 0; i < creature2.skills.Length; i++)
            {
                //Accurate Skill
            }

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
            int startingSelection = classSelection;
            const int amountOfClasses = 5;
            Console.Clear();
            ConsoleColor selectionColor = ConsoleColor.White;
            ConsoleColor selectionTextColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("========================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("========================================================================\r\n");
            Console.ForegroundColor = ConsoleColor.White;
            string[] classNames = { "Damage Dealer", "Tank","Healer", "REMOVED", "Charger" };
            string[][] classDescriptions = new string[amountOfClasses][]
            {
                new string[5]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Do an attack that does like 1.5x your damage","Recharges after 4 attacks"
                },
                    new string[5]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Heal 10% of your max health , Still do a light attack afterwards","Recharges after 3 attacks"
                },
                    new string[5]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Heal 20% of your health","Recharges after 3 attacks"
                },
                    new string[5]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","No Class Ability","Recharges after 3 attacks , Even tho it doesnt exist"
                },
                    new string[5]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Every time you dont use your class ability ","your class ability will attack more times when you use it"
                }
            };
            string[][] iconArray = new string[amountOfClasses][]{
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
                    "          ",
                    "   w.i.p  ",
                    "   w.i.p  ",
                    "          " },
                new string[4]{
                    " ======== ",
                    " |████  | ",
                    " |██    | ",
                    " ======== "}

            };
            for (int i = 0; i < amountOfClasses; i++)
            {
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
                //Thread.Sleep(100);
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 7);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("========================================================================");
            string sentence = "Press ENTER to select a class";
            int center2 = sentence.Length / 2;
            Console.SetCursorPosition(Console.CursorLeft + 35 - center2, Console.CursorTop + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(sentence);
            Console.ForegroundColor = ConsoleColor.White;
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
                            currentPlayer.currentClass = Creature.allClasses.FireGuy;
                            break;
                        case 4:
                            currentPlayer.currentClass = Creature.allClasses.Charger;
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
            string[][] iconArray = new string[5][]{
                new string[4]{
                    "     /\\   ",
                    "    / /   ",
                    "   / /   ",
                    "  / /     "},
                new string[4]{
                    "   _  _   ",
                    "  | \\/ |  ",
                    "   \\  /   ",
                    "    \\/    "},
                new string[4]{
                    "    ||    ",
                    "----╝╚----",
                    "----╗╔----",
                    "    ||    "},
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
                            activeSkillsIconArray[i][j] = iconArray[3][j];
                        }
                        skillNames[i] = "Violent";
                        skillDescriptions[i][0] = "+5 Damage";
                        skillDescriptions[i][1] = "";
                        break;
                }
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 10);
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