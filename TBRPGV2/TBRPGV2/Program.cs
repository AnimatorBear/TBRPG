namespace TBRPGV2
{
    internal class Program
    {
        //Player and starting class
        static player currentPlayer;
        static player.allClasses testingClass = player.allClasses.Class_None;
        static int classSelection = 0;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            currentPlayer = new player(testingClass,50);
            while(currentPlayer.currentClass == player.allClasses.Class_None)
            {
                SelectClass();
            }
            currentPlayer.RecalculateStats();
            Console.Clear();
            Console.WriteLine($"Class: {currentPlayer.currentClass}");
            StartBattle();
            Console.ReadKey();
        }

        static void StartBattle()
        {
            //Makes the enemy
            player enemy = new player(testingClass,10);
            bool activeBattle = true;
            while (activeBattle)
            {
                //Does automatic battle stuff that needs to be redone
                Console.WriteLine("-----");
                Console.WriteLine($"Player hp: {currentPlayer.health} , Enemy hp: {enemy.health}");
                enemy.health -= currentPlayer.Attack(5);
                currentPlayer.health -= enemy.Attack(5);
                if (currentPlayer.health < 1 || enemy.health < 1)
                {
                    activeBattle = false;
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
            Console.WriteLine("=================================================================================================\r\n\r\n\r\n\r\n\r\n\r\n");
            Console.WriteLine("=================================================================================================\r\n");
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