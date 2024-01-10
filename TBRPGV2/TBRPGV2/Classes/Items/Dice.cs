using System;
using System.Text.Json.Serialization;

namespace TBRPGV2
{
    class Dice : Item
    {
        #region jsonConstructor
        [JsonConstructor]
        public Dice()
        {

        }
        #endregion
        public Dice(float heals, float dodge, float dmg, string name) : base( heals, dodge, dmg, name)
        {
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
            itemName = name;
            description = new string[5] {"Wanted a skill","but didnt get", "it? Well the", "Potion of bad ", "RNG is for you!" };
            uses = 1000;
        }
        public override void UseItem(Creature owner, Creature enemy)
        {
            if(owner.currentClass == Creature.allClasses.RNG)
            {
                Random rnd = new Random();
                int numb = rnd.Next(1, 7);

                switch (numb)
                {
                    case 1:
                        owner.HealCreature(-20, false);
                        break;
                    case 2:
                        owner.HealCreature(-10, false);
                        break;
                    case 3:
                        owner.HealCreature(0, false);
                        break;
                    case 4:
                        owner.HealCreature(10, false);
                        break;
                    case 5:
                        owner.HealCreature(20, false);
                        break;
                    case 6:
                        owner.HealCreature(35, false);
                        int dodge;
                        int heal;
                        enemy.health -= owner.Attack(out dodge, out heal, 5);
                        break;
                }
            }
            else
            {
                uses = 0;
            }

        }
    }
}