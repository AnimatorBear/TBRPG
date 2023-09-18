namespace TBRPGV2
{
    internal class player
    {
        #region Stats
        //  All stats
        //Health-Type stats
        public int maxHealth = 100;
        public int health = 100;
        //Damage-Type stats
        public float damage = 10;
        public float damageMultiplier = 1;
        //Misc Stats
        public int currentLevel = 0;
        #endregion
        #region Class
        //What Class
        public enum allClasses { Class0, Class1, Class2 }
        public allClasses currentClass = allClasses.Class1;
        //Every class has its own class ability they can use after a few rounds
        public int classAbilityRecharge = 100;
        public int roundsUntilAbilityRecharge = 0;
        #endregion
        #region Skills
        //Skills are small buffs , you starts with 3 and can unlock 1 later on
        public enum allSkills { None, Health, Speed, Damage, Test1, Test2, Test3, Test4 };
        public allSkills[] skills = { allSkills.None, allSkills.None, allSkills.None };
        #endregion

        public player(allClasses newClass, int startingLevel = 0)
        {
            //Adds starting stats and stuff
            currentClass = newClass;
            currentLevel = startingLevel;
            skills[0] = randomSkill();
            skills[1] = randomSkill();
            skills[2] = randomSkill();
            RecalculateStats(true);
            health = maxHealth;
        }
        public void RecalculateStats(bool showCalculation = false)
        {
            //Does all the math stuff for stats again (useful for if a new skill is unlocked or if you level up)

            //Testing things
            int[] healthSources = {0,0,0};
            float[] damageSources = { 0, 0, 0 };
            //Base Stats
            switch (currentClass)
            {
                case allClasses.Class0:
                    maxHealth = 1000;
                    classAbilityRecharge = 3;
                    break;
                case allClasses.Class1:
                    maxHealth = 100;
                    classAbilityRecharge = 2;
                    break;
            }
            healthSources[0] = maxHealth;
            damageSources[0] = damage;
            int startingHealth = maxHealth;
            float startingDamage = damage;
            //Level stats
            for (int i = 0; i < currentLevel; i++)
            {
                //If base health is 100 health , adds 5 health per level
                maxHealth = maxHealth + (startingHealth / 20);
                
                damage = damage + (startingDamage / 20);
                healthSources[1] = healthSources[1] + (startingHealth / 20);
                damageSources[1] = damageSources[1] + (startingDamage / 20);
            }
            //Skills stats
            for(int i = 0;i < skills.Length; i++)
            {
                if (skills[i] == allSkills.Health)
                {
                    maxHealth = maxHealth + 50;
                    healthSources[2] = 50;
                } else if (skills[i] == allSkills.Damage)
                {
                    damage = damage + 15;
                    damageSources[2] = 15;
                }
            }
            //Math if I wanna see it
            if (showCalculation)
            {
                Console.WriteLine($"=====\r\nMaxHealth: {maxHealth}\r\nDamage: {damage}\r\n  Health Sources: \r\nBase Health: {healthSources[0]}\r\nHealth from Levels: {healthSources[1]}\r\nHealth from Skills: {healthSources[2]}\r\n  Damage Sources: \r\nBase Damage: {damageSources[0]}\r\nDamage from Levels: {damageSources[1]}\r\nDamage from Skills: {damageSources[2]}\r\n=====");
            }
        }
        public bool[] Upgrades()
        {
            //Test
            bool[] result = new bool[10];
            if (health < 15)
            {
                result[0] = true;
            }
            return result;
        }
        public int Attack()
        {
            //Needs to be reworked.
            int attackDamage = (int)(damage * damageMultiplier);
            if(roundsUntilAbilityRecharge <= 0)
            {
                switch (currentClass)
                {
                    case allClasses.Class0:
                        attackDamage = (int)((damage * damageMultiplier) * 1.5f);
                        Console.WriteLine("Class 0 Ability!");
                        break;
                    case allClasses.Class1:
                        health = health + 50;
                        Console.WriteLine("Class 1 Ability!");
                        break;
                }
                roundsUntilAbilityRecharge = classAbilityRecharge;
            }
            else
            {
                roundsUntilAbilityRecharge -= 1;
            }
            return attackDamage;
        }
        public allSkills randomSkill()
        {
            //Chooses a random skill from the allskills enum
            Random rnd = new Random();
            allSkills skill = (allSkills)rnd.Next(Enum.GetNames(typeof(allSkills)).Length);
            if (skill == skills[0] || skill == skills[1] || skill == skills[2] || skill == allSkills.None)
            {
                skill = randomSkill();
                return skill;
            }
            else
            {
                return skill;
            }
        }
    }
}