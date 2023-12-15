namespace TBRPGV2
{
    class Item
    {
        protected Creature owner;
        protected float healing;
        protected float tempDodge;
        protected float tempDamage;
        public string itemName;
        public string[] description = new string[2];
        public int uses = 1;
        public Item(Creature creature,float heals, float dodge,float dmg,string name)
        {
            owner = creature;
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
            itemName = name;
            description[0] = "Item";
            description[1] = "Heals too much";
        }

        public virtual void UseItem()
        {
            owner.HealCreature(healing,false);
        }
    }
}