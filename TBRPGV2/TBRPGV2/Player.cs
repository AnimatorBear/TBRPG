namespace TBRPGV2
{
    internal class player
    {
        public int maxHealth = 100;
        public int health = 100;
        public float damage = 10;
        public float damageMultiplier = 1;

        public int Attack()
        {
            return (int)(damage * damageMultiplier);
        }
    }
}