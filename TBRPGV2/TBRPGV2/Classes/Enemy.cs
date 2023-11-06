namespace TBRPGV2
{
    class Enemy
    {
        Creature enemy;
        Creature player;

        public Enemy(Creature _enemy,Creature _player)
        {
            enemy = _enemy;
            player = _player;
        }

        public void ChooseExtraSkill()
        {
            enemy.skills[3] = Creature.allSkills.Violent;
        }

        public Creature.allClasses ChooseClass()
        {
            Creature.allClasses currentClass = Creature.allClasses.Class_None;
            //DD, Tank, Healer, RNG, Charger , Bag
            int[] classValues = { 20, 20, 20, 20, 20,0 };
            switch (player.currentClass)
            {
                case Creature.allClasses.Class_None:
                    Console.WriteLine("ERROR: Player has no class");
                    break;
                case Creature.allClasses.DamageDealer:
                    classValues[0] -= 5;
                    break;
                case Creature.allClasses.Tank:
                    classValues[1] -= 5;
                    break;
                case Creature.allClasses.Healer:
                    classValues[2] -= 5;
                    break;
                case Creature.allClasses.RNG:
                    classValues[3] -= 5;
                    break;
                case Creature.allClasses.Charger:
                    classValues[4] -= 5;
                    break;
                case Creature.allClasses.Bag:
                    classValues[0] = 0;
                    classValues[1] = 0;
                    classValues[2] = 0;
                    classValues[3] = 0;
                    classValues[4] = 0;
                    classValues[5] = 100;
                    break;
            }

            Random rnd = new Random();
            int total = 0;
            for(int i = 0; i < classValues.Length; i++)
            {
                total += classValues[i];
            }
            int classNumber = rnd.Next(1, total);
            int numb = 0;
            for (int i = 0; i < 6; i++)
            {
                if (classNumber > numb  && classNumber <= numb + classValues[i])
                {
                    numb = i;
                    break;
                }
                numb += classValues[i];
            }
            switch (numb)
            {
                case 0:
                    currentClass = Creature.allClasses.DamageDealer;
                    break;
                case 1:
                    currentClass = Creature.allClasses.Tank;
                    break;
                case 2:
                    currentClass = Creature.allClasses.Healer;
                    break;
                case 3:
                    currentClass = Creature.allClasses.RNG;
                    break;
                case 4:
                    currentClass = Creature.allClasses.Charger;
                    break;
                case 5:
                    currentClass = Creature.allClasses.Bag;
                    break;

                default:
                    Console.WriteLine("Error! Cant decide class! Went to default DamageDealer");
                    currentClass = Creature.allClasses.DamageDealer;
                    Thread.Sleep(10000);
                    break;
            }
           //
            return currentClass;
        }
    }
}