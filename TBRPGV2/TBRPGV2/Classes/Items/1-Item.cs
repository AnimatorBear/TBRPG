using System.Text.Json.Serialization;

namespace TBRPGV2
{

    class Item
    {
        protected float healing { get; set; }
        protected float tempDodge { get; set; }
        protected float tempDamage { get; set; }
        public string itemName { get; set; }
        public string[] description { get; set; } = new string[2];
        public int uses { get; set; } = 1;

        #region jsonConstructor
        [JsonConstructor]
        public Item()
        {

        }
        #endregion
        public Item(float heals, float dodge,float dmg,string name)
        {
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
            itemName = name;
            description[0] = "Item";
            description[1] = "Heals too much";
        }

        public virtual void UseItem(Creature owner,Creature enemy)
        {
            owner.HealCreature(healing,false);
        }
    }
}