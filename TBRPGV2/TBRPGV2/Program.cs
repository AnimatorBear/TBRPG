namespace TBRPGV2
{
    internal class Program
    {
        //Player and starting class
        static player currentPlayer;
        static player.allClasses testingEnemyClass = player.allClasses.Class1;
        static int classSelection = 0;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            currentPlayer = new player(player.allClasses.Class_None,50);
            while(currentPlayer.currentClass == player.allClasses.Class_None)
            {
                SelectClass();
            }
            currentPlayer.RecalculateStats(false);
            Console.WriteLine($"Class: {currentPlayer.currentClass}");
            StartBattle();
            Console.ReadKey();
        }

        static void StartBattle()
        {
            //Makes the enemy
            player enemy = new player(testingEnemyClass,50);
            enemy.RecalculateStats(true);
            Console.WriteLine("Stats Recalculated:");
            currentPlayer.RecalculateStats(true) ;
            enemy.RecalculateStats(true);
            currentPlayer.health = currentPlayer.maxHealth;
            enemy.health = enemy.maxHealth;
            bool activeBattle = true;
            while (activeBattle)
            {
                Console.Title = "TBRPG";
                //Does automatic battle stuff that needs to be redone
                Console.WriteLine("-----");
                Console.WriteLine($"Player hp: {currentPlayer.health} , Enemy hp: {enemy.health}");
                Console.WriteLine($"Player dmg: {currentPlayer.damage} , Enemy dmg: {enemy.damage}");
                bool selectingAttack = true;
                int attack = 0;
                while (selectingAttack)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            attack = 1;
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D2:
                            attack = 2;
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D3:
                            attack = 3;
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D4:
                            attack = 4;
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D5:
                            attack = 5;
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D0:
                            currentPlayer.health = 0;
                            attack = -1;
                            selectingAttack = false;
                            break;
                    }
                }
                enemy.health -= currentPlayer.Attack(attack);
                currentPlayer.health -= enemy.Attack(attack);
                if (currentPlayer.health < 1)
                {
                    Console.WriteLine($"Player died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    activeBattle = false;
                } else if(currentPlayer.health > currentPlayer.maxHealth)
                {
                    currentPlayer.health = currentPlayer.maxHealth;
                }

                if(enemy.health < 1)
                {
                    Console.WriteLine($"Enemy died! Player HP: {currentPlayer.health}\r\nEnemy HP: {enemy.health}");
                    activeBattle = false;
                }
                else if(enemy.health > enemy.maxHealth)
                {
                    enemy.health = enemy.maxHealth;
                }
            }
        }
        static void SelectClass()
        {
            int startingSelection = classSelection;
            const int amountOfClasses = 5;
            Console.Clear();
            ConsoleColor selectionColor = ConsoleColor.White;
            ConsoleColor selectionTextColor = ConsoleColor.Black;
            Console.WriteLine("========================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("========================================================================\r\n");
            string[] classNames = { "Class 1","Class 2","Class 3","Class 4","Class 5" };
            string[][] classDescriptions = new string[amountOfClasses][]
            {
                new string[4]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Do an attack that does like 1.5x your damage"
                },
                    new string[4]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Heal 10% of your max health"
                },
                    new string[4]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Class Ability"
                },
                    new string[4]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Class Ability"
                },
                    new string[4]
                {
                    "Well arent descriptions cool","More Description","maybe some stats","Class Ability"
                }
            };
            string[][] iconArray = new string[amountOfClasses][]{
                new string[4]{ 
                    "  ======  ",
                    "    ||    ",
                    "    ||    ",
                    "    ||    "},
                new string[4]{
                    "  /----\\  ",
                    "  |    |  ",
                    "  |    |  ",
                    "    ||    " },
                new string[4]{
                    "     |    ",
                    "     |    ",
                    "     |    ",
                    "     |    "},
                new string[4]{ 
                    "     |    ",
                    "     |    ",
                    "     |    ",
                    "     |    " },
                new string[4]{
                    "     |    ",
                    "     |    ",
                    "     |    ",
                    "     |    "}

            };
            for (int i = 0; i < amountOfClasses; i++)
            {
                Console.CursorVisible = false;
                int center = classNames[classSelection].Length / 2;
                Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop - 8);
                Console.WriteLine(classNames[classSelection]);
                for(int j = 0; j < 4; j++)
                {
                    center = classDescriptions[classSelection][j].Length / 2;
                    Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop);
                    Console.WriteLine(classDescriptions[classSelection][j]);
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);
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
                            currentPlayer.currentClass = player.allClasses.Class0;
                            break;
                        case 1:
                            currentPlayer.currentClass = player.allClasses.Class1;
                            break;
                        case 2:
                            currentPlayer.currentClass = player.allClasses.Class2;
                            break;
                        case 3:
                            currentPlayer.currentClass = player.allClasses.Class3;
                            break;
                        case 4:
                            currentPlayer.currentClass = player.allClasses.Class4;
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
    }
}