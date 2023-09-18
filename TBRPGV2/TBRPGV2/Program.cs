namespace TBRPGV2
{
    internal class Program
    {
        //Player and starting class
        static player currentPlayer;
        static player.allClasses testingClass = player.allClasses.Class0;
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            currentPlayer = new player(testingClass,50);
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
                Console.WriteLine($"Player hp: {currentPlayer.health}");
                Console.WriteLine($"Enemy hp: {enemy.health}");
                enemy.health -= currentPlayer.Attack();
                currentPlayer.health -= enemy.Attack();
                if (currentPlayer.health < 1 || enemy.health < 1)
                {
                    activeBattle = false;
                }
            }
        }
    }
}