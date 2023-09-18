namespace TBRPGV2
{
    internal class Program
    {
        //Note to self: Race is cancelled
        //Another note to self: Enums for everything.
        static generalClass genClass;
        static player currentPlayer = new player();
        static generalClass.allClasses testingClass = generalClass.allClasses.Class1;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            genClass = new generalClass(testingClass,currentPlayer);
            StartBattle();
            Console.ReadKey();
        }

        static void StartBattle()
        {
            player enemy = new player();
            //Assign random class
            generalClass enemyClass = new generalClass(testingClass,enemy);
            bool activeBattle = true;
            while (activeBattle)
            {
                //insert actual battle shit
                enemy.health -= currentPlayer.Attack();
                currentPlayer.health -= enemy.Attack();
                Console.WriteLine($"Player hp: {currentPlayer.health}");
                Console.WriteLine($"Enemy hp: {enemy.health}");
                if (currentPlayer.health < 1 || enemy.health < 1)
                {
                    activeBattle = false;
                }
            }
        }
    }
}