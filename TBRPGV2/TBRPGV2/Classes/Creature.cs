namespace TBRPGV2
{
    class Creature
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
        public int currentXP = 0;
        public int speed = 0;
        public int rng_ExtraLuck = 0;
        #endregion
        #region Class
        //Al Classes and Current Class
        public enum allClasses { Class_None, DamageDealer, Tank, Healer, RNG, Charger,Bag }
        public allClasses currentClass = allClasses.Class_None;

        //Stats for each seperate class
        public int[,] classStats = { 
            //DamageDealer
            {90,12,5, 999, 8} , 
            //Tank
            { 130, 8, 4, 999, 7 }, 
            //Healer
            { 115, 6, 6, 3, 9 }, 
            //RNG
            { 100, 9, 0, 999, 7 }, 
            //Charger
            { 90, 9, 1, 999, 7 }, 
            //Bag Stats, For testing
            { 99, -6, 0, 999, 0 } };

        //Every class has its own class ability they can use after a few rounds
        public int classAbilityRecharge = 100;
        public int roundsUntilAbilityRecharge = 0;
        public int chargerCharge = 1;
        public int maxClassAbilityUses = 100;
        public int classAbilityUses = 0;
        #endregion
        #region Skills
        //Skills are small changes , you start with 3 and can unlock 1 at level 20
        public enum allSkills { None, Healthy, Fast, Violent, Heavy_Hitter, Light_Hitter, Fast_Learner,Accurate,Glass_Cannon, Stone_Wall,NotBag, rngLucky};
        public allSkills[] skills = new allSkills[Program.amountStartSkills + 1];

        //Not class specific skills
        allSkills[] allClassSkills = { allSkills.Healthy, allSkills.Fast, allSkills.Violent, allSkills.Heavy_Hitter, allSkills.Light_Hitter, allSkills.Fast_Learner, allSkills.Accurate};

        //Class specific skills
        allSkills[][] classSkills = { 
                //DamageDealer
                new allSkills[] {allSkills.Glass_Cannon},
                //Tank
                new allSkills[] {allSkills.Stone_Wall},
                //Healer
                new allSkills[] {allSkills.Stone_Wall},
                //RNG
                new allSkills[] {allSkills.rngLucky},
                //Charger
                new allSkills[] {allSkills.Glass_Cannon},
                //Bag
                new allSkills[] {allSkills.Glass_Cannon,allSkills.Stone_Wall,allSkills.NotBag,allSkills.rngLucky}
            };
        #endregion
        #region attacks
        //DD = DamageDealer
        //TK = Tank 
        //HL = Healer
        //RG = Randomizer
        //CH = Charger
        //BG = Bag
        public enum attacks { None,Class_Ability,Heavy_Hit,Light_Hit,BG_NO, RG_Stats,HL_LifeSteal };
        public int[] attackLevels = { -1, -1, -1, -1, -1,5,5};
        public attacks[] characterAttacks = { attacks.Light_Hit,attacks.Heavy_Hit,attacks.None,attacks.None};
        #endregion
        #region Inventory
        public Item[] itemsInInv = new Item[20];
        #endregion
        public Creature(allClasses newClass, int startingLevel = 0)
        {
            //Adds starting stats and stuff
            currentClass = newClass;
            currentLevel = startingLevel;
            health = maxHealth;
            itemsInInv[0] = new Item(this,10000000,0,0);
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
            bool stoneWall = false;
            for(int i = 0;i < skills.Length; i++)
            {
                switch (skills[i])
                {
                    case allSkills.Healthy:
                        maxHealth = maxHealth + 25;
                        break;
                    case allSkills.Violent:
                        damage = damage + 5;
                        break;
                    case allSkills.Fast:
                        speed = speed + 3;
                        break;
                    case allSkills.Glass_Cannon:
                        glassCannon = true;
                        break;
                    case allSkills.Stone_Wall:
                        stoneWall = true;
                        break;
                    case allSkills.NotBag:
                        damage = 0;
                        break;
                    case allSkills.rngLucky:
                        rng_ExtraLuck = 2;
                        break;
                }
            }
            if (glassCannon)
            {
                maxHealth = (int)((maxHealth) * 0.75f);
                damage = damage * 1.5f;
            }
            if (stoneWall)
            {
                maxHealth = (int)(maxHealth * 1.5f);
                damage = (damage) * 0.75f;
            }
            damage = (float)Math.Round(damage,1);
            //Math if I wanna see it
            if (showCalculation)
            {
                Console.Write($"=====\r\nMaxHealth: {maxHealth}\r\nDamage: {damage}\r\nCurrent Level: {currentLevel}\r\nCurrent XP: {currentXP} \r\n Skills: ");
                for (int i = 0; i < skills.Length; i++)
                {
                    if (i != 0 && i != skills.Length - 1)
                    {
                        Console.Write(", ");
                    } else if (i != 0)
                    {
                        Console.Write(" and ");
                    }
                    Console.Write(skills[i]);
                }
                Console.WriteLine("\r\n=====");
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
        public int Attack(out int dodge, out int healing, int attack = 0, bool doRandom = true,bool useEnumAttackInstead = false, attacks enumAttack = attacks.None,bool visual = false)
        {
            attacks currentAttack = attacks.None;
            dodge = 0;
            healing = 0;
            RandomAttack:
            #region Select attack based on int
            if (!useEnumAttackInstead)
            {
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
            }
            else
            {
                //or not
                currentAttack = enumAttack;
            }
            
            #endregion
            float attackDamage = damage;
            #region Randomizer Random Attack Damage
            if (currentClass == allClasses.RNG && doRandom)
            {
                Random rnd = new Random();
                int rand = rnd.Next(-5 + rng_ExtraLuck, 5 + rng_ExtraLuck);
                attackDamage += rand;
            }
            #endregion
            switch (currentAttack)
            {
                //The actual attacks
                #region All Class Attacks
                    #region None
                case attacks.None:
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 1;
                    }
                    return 0;
                #endregion
                    #region Class Ability
                case attacks.Class_Ability:
                    if (roundsUntilAbilityRecharge <= 0 && classAbilityUses < maxClassAbilityUses)
                    {
                        if (!visual)
                        {
                            roundsUntilAbilityRecharge = classAbilityRecharge;
                            classAbilityUses++;
                        }
                        switch (currentClass)
                        {
                            case allClasses.DamageDealer:
                                int classDamage = (int)((attackDamage * damageMultiplier) * 2f);
                                return classDamage;

                            case allClasses.Tank:
                                healing = (maxHealth / 10);
                                HealCreature((maxHealth / 10), visual);
                                return (int)((damage * damageMultiplier) * 0.5f);

                            case allClasses.Healer:
                                healing = (maxHealth / 2);
                                HealCreature((maxHealth / 2), visual);
                                return 0;

                            case allClasses.RNG:
                                if(doRandom == false)
                                {
                                    return 0;
                                }
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
                                classDamage = (int)((((attackDamage * 0.5f) + chargerCharge) * chargerCharge)*damageMultiplier);
                                if (!visual)
                                {
                                    chargerCharge = 0;
                                }
                                return classDamage;

                            case allClasses.Bag:
                                return 10 * (currentLevel + 1);
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
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 1;
                    }
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
                    dodge = 25;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Heavy_Hitter)
                        {
                            extraDamage = extraDamage + (attackDamage * 1.2f);
                        }
                    }
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 1;
                    }
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
                    dodge = 10;
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 2;
                    }
                    int dmg = (int)((attackDamage) * damageMultiplier);
                    healing = (int)(dmg / 1.5f);
                    HealCreature((dmg / 1.5f),visual);
                    return (int)(dmg / 1.5f);
                #endregion
                #endregion
                #region RNG
                    #region RG_Stats
                case attacks.RG_Stats:
                    float nerf = 0.05f;
                    if (!doRandom)
                    {
                        dodge = 12;
                        healing = 2 + rng_ExtraLuck;
                        return (int)(((12 + rng_ExtraLuck) * nerf) * damage);
                    }
                    else
                    {
                        Random rnd = new Random();
                        int rand = rnd.Next(5, 15);
                        dodge = rand;
                        rand = rnd.Next(-5 + rng_ExtraLuck, 10 + rng_ExtraLuck);
                        healing = rand;
                        HealCreature(rand,visual);
                        rand = rnd.Next((int)(((5 + rng_ExtraLuck) * nerf) * damage), (int)(((20 + rng_ExtraLuck) * nerf) * damage));
                        if (rand >= (int)(((18 + rng_ExtraLuck) * nerf) * damage))
                        {
                            rand = rnd.Next((int)(((18 + rng_ExtraLuck) * nerf) * damage), (int)(((30 + rng_ExtraLuck) * nerf) * damage));
                        }
                        return rand;
                    }
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
            List<allSkills> randomSkills = GetAllAvailableSkills();
            allSkills skill = randomSkills[rnd.Next(0,randomSkills.Count)];
            return skill;
        }

        //Returns a list of all skills a player doesnt have.
        public List<allSkills> GetAllAvailableSkills()
        {
            List<allSkills> allAvSkills = new List<allSkills>();
            #region Adding skills to available skills
            for (int i = 0; i < allClassSkills.Length; i++)
            {
                allAvSkills.Add(allClassSkills[i]);
            }
            #endregion
            #region Adding class skills to the available skills
            switch (currentClass)
            {
                case allClasses.DamageDealer:
                    for (int i = 0; i < classSkills[0].Length; i++)
                    {
                        allAvSkills.Add(classSkills[0][i]);
                    }
                    break;
                case allClasses.Tank:
                    for (int i = 0; i < classSkills[1].Length; i++)
                    {
                        allAvSkills.Add(classSkills[1][i]);
                    }
                    break;
                case allClasses.Healer:
                    for (int i = 0; i < classSkills[2].Length; i++)
                    {
                        allAvSkills.Add(classSkills[2][i]);
                    }
                    break;
                case allClasses.RNG:
                    for (int i = 0; i < classSkills[3].Length; i++)
                    {
                        allAvSkills.Add(classSkills[3][i]);
                    }
                    break;
                case allClasses.Charger:
                    for (int i = 0; i < classSkills[4].Length; i++)
                    {
                        allAvSkills.Add(classSkills[4][i]);
                    }
                    break;
                case allClasses.Bag:
                    for (int i = 0; i < classSkills[5].Length; i++)
                    {
                        allAvSkills.Add(classSkills[5][i]);
                    }
                    break;
            }
            #endregion

            #region Remove skills the player already has
            for (int i = 0; i < skills.Length; i++)
            {
                allAvSkills.Remove(skills[i]);
            }
            #endregion
            return allAvSkills;
        }
        public void HealCreature(float healing,bool visual)
        {
            if (!visual)
            {
                health += (int)healing;
            }
        }

        public void AddXP(int xp,bool levels = false)
        {
            currentXP += currentLevel * 100;
            if (!levels)
            {
                
                currentXP += xp;
            }
            else
            {
                currentXP += xp * 100;
            }
            float totalXP = currentXP / 100;
            currentLevel = (int)(Math.Floor(totalXP));
            currentXP -= currentLevel * 100;
            Console.WriteLine(totalXP);
            Console.WriteLine(currentLevel);
        }
    }
}