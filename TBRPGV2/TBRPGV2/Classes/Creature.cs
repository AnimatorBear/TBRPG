namespace TBRPGV2
{
    internal class Creature
    {
        public int[,] classStats = { {90,13,4, 999, 8} , { 130, 9, 3, 999, 7 }, { 115, 7, 5, 3, 9 }, { 100, 10, 0, 999, 7 }, { 90, 10, 1, 999, 7 }, 
            //Bag Stats, For testing
            { 999, -5, 0, 999, 0 } };
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
        public enum allClasses { Class_None, DamageDealer, Tank, Healer, RNG, Charger,Bag }
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
        public enum allSkills { None, Healthy, Fast, Violent, Heavy_Hitter, Light_Hitter, Fast_Learner,Accurate,Glass_Cannon};
        public allSkills[] skills = { allSkills.None, allSkills.None, allSkills.None,allSkills.None };
        #endregion

        //DD = DamageDealer
        //TK = Tank 
        //HL = Healer
        //RG = Randomizer
        //CH = Charger
        //BG = Bag
        public enum attacks { None,Class_Ability,Heavy_Hit,Light_Hit,BG_NO, RG_Stats,HL_LifeSteal };
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
                    characterAttacks[2] = attacks.Light_Hit;
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
                    characterAttacks[2] = attacks.HL_LifeSteal;
                    break;
                case allClasses.RNG:
                    maxHealth = classStats[3, 0];
                    damage = (float)classStats[3, 1];
                    classAbilityRecharge = classStats[3, 2];
                    maxClassAbilityUses = classStats[3, 3];
                    speed = classStats[3, 4];
                    characterAttacks[2] = attacks.RG_Stats;
                    break;
                case allClasses.Charger:
                    maxHealth = classStats[4, 0];
                    damage = (float)classStats[4, 1];
                    classAbilityRecharge = classStats[4, 2];
                    maxClassAbilityUses = classStats[4, 3];
                    speed = classStats[4, 4];
                    break;
                case allClasses.Bag:
                    maxHealth = classStats[5, 0];
                    damage = (float)classStats[5, 1];
                    classAbilityRecharge = classStats[5, 2];
                    maxClassAbilityUses = classStats[5, 3];
                    speed = classStats[5, 4];
                    characterAttacks[2] = attacks.BG_NO;
                    characterAttacks[2] = attacks.BG_NO;
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
            bool glassCannon = false;
            for(int i = 0;i < skills.Length; i++)
            {
                if (skills[i] == allSkills.Healthy)
                {
                    maxHealth = maxHealth + 25;
                    healthSources[2] = 25;
                } else if (skills[i] == allSkills.Violent)
                {
                    damage = damage + 3;
                    damageSources[2] = 3;
                }else if (skills[i] == allSkills.Fast)
                {
                    speed = speed + 3;
                }else if (skills[i] == allSkills.Glass_Cannon)
                {
                    glassCannon = true;
                }
            }
            if (glassCannon)
            {
                maxHealth = maxHealth / 2;
                damage = damage * 2;
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
        public int Attack(out int dodge,int attack = 0)
        {
            attacks currentAttack = attacks.None;
            dodge = 0;
            RandomAttack:
            #region Select attack based on int
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
            #endregion
            float attackDamage = damage;
            #region Randomizer Random Attack Damage
            if (currentClass == allClasses.RNG)
            {
                Random rnd = new Random();
                int rand = rnd.Next(-5, 5);
                attackDamage += rand;
            }
            #endregion
            Console.WriteLine(currentAttack);
            switch (currentAttack)
            {
                //The actual attacks
                #region All Class Attacks
                    #region None
                case attacks.None:
                    roundsUntilAbilityRecharge -= 1;
                    return -100;
                #endregion
                    #region Class Ability
                case attacks.Class_Ability:
                    if (roundsUntilAbilityRecharge <= 0 && classAbilityUses < maxClassAbilityUses)
                    {
                        roundsUntilAbilityRecharge = classAbilityRecharge;
                        classAbilityUses++;
                        switch (currentClass)
                        {
                            case allClasses.DamageDealer:
                                int classDamage = (int)((attackDamage * damageMultiplier) * 1.5f);
                                Console.WriteLine(classDamage);
                                return classDamage;

                            case allClasses.Tank:
                                health = health + ((maxHealth / 10));
                                return (int)((damage * damageMultiplier) * 0.5f);

                            case allClasses.Healer:
                                health = health + ((maxHealth / 2));
                                Console.WriteLine("Heals: " + maxHealth / 2);
                                return 0;

                            case allClasses.RNG:
                                bool gotRND = false;
                                while (!gotRND)
                                {
                                    Random rnd2 = new Random();
                                    int rand2 = rnd2.Next(1, 5);
                                    if (characterAttacks[rand2 - 1] != attacks.None)
                                    {
                                        Program.amountOfClasses = 6;
                                        attack = rand2;
                                        gotRND = true;
                                    }
                                }
                                goto RandomAttack;

                            case allClasses.Charger:
                                classDamage = (int)((((attackDamage * 0.75f) + chargerCharge) * chargerCharge)*damageMultiplier);
                                Console.WriteLine(chargerCharge + " Damage: " + classDamage);
                                chargerCharge = 0;
                                return classDamage;

                            case allClasses.Bag:
                                return 1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cant use ability rn");
                    }
                    break;
                #endregion
                    #region Light Hit
                case attacks.Light_Hit:
                    dodge = 5;
                    roundsUntilAbilityRecharge -= 2;
                    float extraDamage = 0;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Light_Hitter)
                        {
                            extraDamage = extraDamage + (attackDamage * 1.2f);
                        }
                    }
                    return (int)((attackDamage + extraDamage) * damageMultiplier);
                #endregion
                    #region Heavy Hit
                case attacks.Heavy_Hit:
                    extraDamage = 0;
                    dodge = 15;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Heavy_Hitter)
                        {
                            extraDamage = extraDamage + (attackDamage * 1.2f);
                        }
                    }
                    roundsUntilAbilityRecharge -= 1;
                    return (int)(((attackDamage * 1.5f) + extraDamage) * damageMultiplier);
                #endregion
                #endregion

                #region ClassSpecificAttacks
                #region Damage Dealer
                #endregion
                #region Tank
                #endregion
                #region Healer
                    #region LifeSteal
                case attacks.HL_LifeSteal:
                    dodge = 5;
                    roundsUntilAbilityRecharge -= 2;
                    int dmg = (int)((attackDamage) * damageMultiplier);
                    health = health + (dmg / 2);
                    return dmg/2;
                #endregion
                #endregion
                #region RNG
                    #region RG_Stats
                case attacks.RG_Stats:
                    Random rnd = new Random();
                    int rand = rnd.Next(5, 15);
                    dodge = rand;
                    Console.WriteLine(dodge + "dodge");
                    rand = rnd.Next(-5, 10);
                    Console.WriteLine(rand + "HP");
                    health += rand;
                    rand = rnd.Next(5,20);
                    if(rand >= 18)
                    {
                        rand = rnd.Next(18, 30);
                    } 
                    Console.WriteLine(rand+"dmg");
                    return rand;
                #endregion
                #endregion
                #region Charger
                #endregion
                #region Bag
                case attacks.BG_NO:
                    return 0;
                #endregion
                #endregion
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