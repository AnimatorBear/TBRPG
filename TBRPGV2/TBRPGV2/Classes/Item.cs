namespace TBRPGV2
{
    internal class Item
    {
        Creature owner;
        float healing;
        float tempDodge;
        float tempDamage;
        string[] icon = new string[4];
        string[] description = new string[2];
        public Item(Creature creature,float heals, float dodge,float dmg)
        {
            owner = creature;
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
        }

        public virtual void UseItem()
        {
            owner.HealCreature(healing);
        }
    }
}