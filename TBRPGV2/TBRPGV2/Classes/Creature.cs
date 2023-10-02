namespace TBRPGV2
{
    internal class Creature
    {
        public int[,] classStats = { {90,17,4, 999, 100} , { 130, 10, 3, 999, 100 }, { 115, 9, 5, 3, 100 }, { 999, 999, 0, 999, 100 }, { 90, 13, 1, 999, 100 } };
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
        public int speed = 0;
        #endregion
        #region Class
        //What Class
        public enum allClasses { Class_None, DamageDealer, Tank, Healer, FireGuy, Charger }
        public allClasses currentClass = allClasses.DamageDealer;
        //Every class has its own class ability they can use after a few rounds
        public int classAbilityRecharge = 100;
        public int roundsUntilAbilityRecharge = 0;
        public int chargerCharge = 1;
        public int maxClassAbilityUses = 100;
        public int classAbilityUses = 0;
        #endregion
        #region Skills
        //Skills are small buffs , you starts with 3 and can unlock 1 later on
        public enum allSkills { None, Healthy, Fast, Violent, Heavy_Hitter, Light_Hitter, Fast_Learner,Lucky};
        public allSkills[] skills = { allSkills.None, allSkills.None, allSkills.None,allSkills.None };
        #endregion
        public enum attacks { None,Class_Ability,Heavy_Hit,Light_Hit};
        public attacks[] characterAttacks = { attacks.Light_Hit,attacks.Heavy_Hit,attacks.None,attacks.None};

        public Creature(allClasses newClass, int startingLevel = 0)
        {
            //Adds starting stats and stuff
            currentClass = newClass;
            currentLevel = startingLevel;
            skills[0] = randomSkill();
            skills[1] = randomSkill();
            skills[2] = randomSkill();
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
                case allClasses.DamageDealer:
                    maxHealth = classStats[0,0];
                    damage = (float)classStats[0,1];
                    classAbilityRecharge = classStats[0, 2];
                    maxClassAbilityUses = classStats[0, 3];
                    speed = classStats[0, 4];
                    break;
                case allClasses.Tank:
                    maxHealth = classStats[1, 0];
                    damage = (float)classStats[1, 1];
                    classAbilityRecharge = classStats[1, 2];
                    maxClassAbilityUses = classStats[1, 3];
                    speed = classStats[1, 4];
                    break;
                case allClasses.Healer:
                    maxHealth = classStats[2, 0];
                    damage = (float)classStats[2, 1];
                    classAbilityRecharge = classStats[2, 2];
                    maxClassAbilityUses = classStats[2, 3];
                    speed = classStats[2, 4];
                    break;
                case allClasses.FireGuy:
                    maxHealth = classStats[3, 0];
                    damage = (float)classStats[3, 1];
                    classAbilityRecharge = classStats[3, 2];
                    maxClassAbilityUses = classStats[3, 3];
                    speed = classStats[3, 4];
                    break;
                case allClasses.Charger:
                    maxHealth = classStats[4, 0];
                    damage = (float)classStats[4, 1];
                    classAbilityRecharge = classStats[4, 2];
                    maxClassAbilityUses = classStats[4, 3];
                    speed = classStats[4, 4];
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
                if (skills[i] == allSkills.Healthy)
                {
                    maxHealth = maxHealth + 25;
                    healthSources[2] = 25;
                } else if (skills[i] == allSkills.Violent)
                {
                    damage = damage + 5;
                    damageSources[2] = 5;
                }else if (skills[i] == allSkills.Fast)
                {
                    speed = speed + 5;
                }
            }
            damage = (float)Math.Round(damage,1);
            //Math if I wanna see it
            if (showCalculation)
            {
                Console.WriteLine($"=====\r\nMaxHealth: {maxHealth}\r\nDamage: {damage}\r\nCurrent Level: {currentLevel} \r\n Skills: {skills[0]}, {skills[1]}, {skills[2]} and {skills[3]}\r\n  Health Sources: \r\nBase Health: {healthSources[0]}\r\nHealth from Levels: {healthSources[1]}\r\nHealth from Skills: {healthSources[2]}\r\n  Damage Sources: \r\nBase Damage: {damageSources[0]}\r\nDamage from Levels: {damageSources[1]}\r\nDamage from Skills: {damageSources[2]}\r\n=====");
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
        public int Attack(int attack = 0)
        {
            attacks currentAttack = attacks.None;
            switch (attack)
            {
                case 1:
                    currentAttack = characterAttacks[0];
                    break;
                case 2:
                    currentAttack = characterAttacks[1];
                    break;
                case 3:
                    currentAttack = characterAttacks[2];
                    break;
                case 4:
                    currentAttack = characterAttacks[3];
                    break;
                case 5:
                    currentAttack = attacks.Class_Ability;
                    break;
                case -1:
                    return 0;
            }
            switch (currentAttack)
            {
                case attacks.None:
                    Console.WriteLine("ERROR, No attack");
                    roundsUntilAbilityRecharge -= 1;
                    return 0;

                case attacks.Light_Hit:
                    Console.WriteLine("Light Attack");
                    roundsUntilAbilityRecharge -= 2;
                    float extraDamage = 0;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Light_Hitter)
                        {
                            extraDamage = extraDamage + (damage * 1.2f);
                        } 
                    }
                    return (int)(damage + extraDamage);
                case attacks.Heavy_Hit:
                    Console.WriteLine("Heavy Attack");
                    extraDamage = 0;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Heavy_Hitter)
                        {
                            extraDamage = extraDamage + (damage * 1.2f);
                        }
                    }
                    roundsUntilAbilityRecharge -= 1;
                    return (int)((damage * 1.5f) + extraDamage);
                case attacks.Class_Ability:
                    if(roundsUntilAbilityRecharge <= 0 && classAbilityUses < maxClassAbilityUses)
                    {
                        Console.WriteLine("Class Ability!");
                        roundsUntilAbilityRecharge = classAbilityRecharge;
                        classAbilityUses++;
                        switch (currentClass)
                        {
                            case allClasses.DamageDealer:
                                int attackDamage = (int)((damage * damageMultiplier) * 1.5f);
                                Console.WriteLine(attackDamage);
                                return attackDamage;
                            case allClasses.Tank:
                                health = health + ((maxHealth/10));
                                return (int)((damage * damageMultiplier) * 0.5f);
                            case allClasses.Healer:
                                health = health + ((maxHealth / 2));
                                Console.WriteLine("Heals: " + maxHealth / 2);
                                return 0;
                            case allClasses.FireGuy:
                                Console.WriteLine("Unfortunately REMOVED doesnt have an ability yet");
                                return 0;
                            case allClasses.Charger:
                                attackDamage = (int)((damage *0.75f)*chargerCharge );
                                Console.WriteLine(chargerCharge + " Damage: " + attackDamage);
                                chargerCharge = 0;
                                return attackDamage;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cant use ability rn");
                    }
                    break;
            }
            return 0;
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