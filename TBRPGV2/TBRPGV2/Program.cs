namespace TBRPGV2
{
    internal class Program
    {
        //Player and starting class
        static player currentPlayer = new player(player.allClasses.Class_None, 10);
        static player.allClasses testingEnemyClass = player.allClasses.Healer;
        static int classSelection = 0;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            while(currentPlayer.currentClass == player.allClasses.Class_None)
            {
                SelectClass();
            }
            currentPlayer.RecalculateStats();
            Console.WriteLine($"Class: {currentPlayer.currentClass}");
            StartBattle();
            Console.ReadKey();
        }

        static void StartBattle()
        {
            //Makes the enemy
            player enemy = new player(testingEnemyClass,10);
            Console.WriteLine($"Enemy Class: {enemy.currentClass}");
            enemy.RecalculateStats();
            currentPlayer.RecalculateStats() ;
            enemy.RecalculateStats();
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
                            AddChargerCharge(enemy);
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D2:
                            attack = 2;
                            AddChargerCharge(enemy);
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D3:
                            attack = 3;
                            AddChargerCharge(enemy);
                            selectingAttack = false;
                            break;
                        case ConsoleKey.D4:
                            attack = 4;
                            AddChargerCharge(enemy);
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
        static void AddChargerCharge(player enemy,bool remove = false)
        {
            if(currentPlayer.currentClass == player.allClasses.Charger)
            {
                currentPlayer.chargerCharge++;
            }
            if (enemy.currentClass == player.allClasses.Charger)
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
                Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop - 8);
                Console.WriteLine(classNames[classSelection]);
                for(int j = 0; j < 5; j++)
                {
                    center = classDescriptions[classSelection][j].Length / 2;
                    Console.SetCursorPosition(Console.CursorLeft + 35 - center, Console.CursorTop);
                    Console.WriteLine(classDescriptions[classSelection][j]);
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
                            currentPlayer.currentClass = player.allClasses.DamageDealer;
                            break;
                        case 1:
                            currentPlayer.currentClass = player.allClasses.Tank;
                            break;
                        case 2:
                            currentPlayer.currentClass = player.allClasses.Healer;
                            break;
                        case 3:
                            currentPlayer.currentClass = player.allClasses.FireGuy;
                            break;
                        case 4:
                            currentPlayer.currentClass = player.allClasses.Charger;
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