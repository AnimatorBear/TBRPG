namespace TBRPGV2
{
    internal class generalClass
    {
        public enum allClasses{Class0,Class1,Class2}
        public allClasses currentClass = allClasses.Class1;
        public player currentPlayer;

        public generalClass(allClasses newClass, player newPlayer)
        {
            currentClass = newClass;
            currentPlayer=newPlayer;
            switch (currentClass)
            {
                case allClasses.Class0:
                    currentPlayer.health = 10;
                    break;
                case allClasses.Class1:
                    currentPlayer.health = 100;
                    break;
            }
        }

        public bool[] Upgrades()
        {
            bool[] result = new bool[10];
            if(currentPlayer.health < 15)
            {
                result[0] = true;
            }
            return result;
        }
    }
}