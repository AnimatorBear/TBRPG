using System.Text.Json.Serialization;

namespace TBRPGV2
{
    class Creature
    {
        readonly float attackSpamNerf = 0.50f;
        #region Stats
        //  All stats
        //Health-Type stats
        public int maxHealth { get; set; }
        public int health { get; set; }
        //Damage-Type stats
        public float damage  { get; set; }
        public float damageMultiplier { get; set; } = 1;
        //Misc Stats
        public int currentLevel { get; set; }
        public int currentXP { get; set; }
        public int speed;
        public int rng_ExtraLuck { get; set; }
        #endregion
        #region Class
        //Al Classes and Current Class
        public enum allClasses { Class_None, DamageDealer, Tank, Healer, RNG, Charger,Bag }
        public allClasses currentClass { get; set; }

        //Stats for each seperate class
        public int[,] classStats = { 
            //DamageDealer
            {90,12,5, 999, 8} , 
            //Tank
            { 130, 8, 4, 999, 7 }, 
            //Healer
            { 115, 6, 6, 3, 9 }, 
            //RNG
            { 100, 9, 3, 999, 7 }, 
            //Charger
            { 90, 9, 1, 999, 7 }, 
            //Bag Stats, For testing
            { 99, -6, 0, 999, 0 } };

        //Every class has its own class ability they can use after a few rounds
        public int abilityRecharge = 100;
        public int roundsUntilAbilityRecharge = 0;
        public int chargerCharge = 1;
        public int maxAbilityUses = 100;
        public int abilityUses = 0;
        #endregion
        #region Skills
        //Skills are small changes , you start with 3 and can unlock 1 at level 20
        public enum allSkills { None, Healthy, Fast, Violent, Heavy_Hitter, Light_Hitter, Fast_Learner,Accurate,Glass_Cannon, Stone_Wall,NotBag, rngLucky};
        public allSkills[] skills { get; set; } = new allSkills[Program.amountStartSkills + 1];

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
        #region Attacks
        //DD = DamageDealer
        //TK = Tank 
        //HL = Healer
        //RG = Randomizer
        //CH = Charger
        //BG = Bag
        public enum attacks { None,Class_Ability,Heavy_Hit,Light_Hit,DD_HPSacrifice, HL_LifeSteal, RG_Stats, BG_NO };
        public int[] attackLevels = { -1, -1, -1, -1, -1,5,5};
        public attacks[] characterAttacks { get; set; } = { attacks.None,attacks.None,attacks.None,attacks.None};
        public attacks prevAttack { get; set; }
        #endregion
        #region Inventory
        public Item[] itemsInInv = new Item[6];
        #endregion

        [JsonConstructor]
        public Creature()
        {

        }
        public Creature(allClasses newClass = allClasses.DamageDealer, int startingLevel = 0)
        {
            //Adds starting stats and stuff
            currentClass = newClass;
            currentLevel = startingLevel;
            health = maxHealth;
            itemsInInv[0] = new Item(this,10000000,0,0,"Health Pot");
            itemsInInv[0].description = new string[4] { "Heals you","For like","a billion ","health" };
            itemsInInv[1] = new Item(this, 10000000, 0, 0, "God Pot");
            itemsInInv[2] = new Item(this, 10000000, 0, 0, "Potion of life");
            itemsInInv[3] = new EnchChair(this, 0, 0, 0, "Enchanted Chair");
            itemsInInv[4] = new Item(this, 10000000, 0, 0, "Bandage");
            itemsInInv[5] = new Item(this, 10000000, 0, 0, "Med kit");
        }

        public void GiveBaseAttacks()
        {
            switch (currentClass)
            {
                case allClasses.DamageDealer:
                    characterAttacks[0] = attacks.Light_Hit;
                    characterAttacks[1] = attacks.Heavy_Hit;
                    characterAttacks[2] = attacks.DD_HPSacrifice;
                    characterAttacks[3] = attacks.HL_LifeSteal;
                    break;
                case allClasses.Tank:
                    characterAttacks[0] = attacks.Light_Hit;
                    characterAttacks[1] = attacks.Heavy_Hit;
                    break;
                case allClasses.Healer:
                    characterAttacks[0] = attacks.Light_Hit;
                    characterAttacks[1] = attacks.Heavy_Hit;
                    break;
                case allClasses.RNG:
                    characterAttacks[0] = attacks.Light_Hit;
                    characterAttacks[1] = attacks.Heavy_Hit;
                    break;
                case allClasses.Charger:
                    characterAttacks[0] = attacks.Light_Hit;
                    characterAttacks[1] = attacks.Heavy_Hit;
                    break;
                default:
                    break;
            }
        }
        public void RecalculateStats()
        {
            //Does all the math stuff for stats again (useful for if a new skill is unlocked or if you level up)

            //Base Stats
            switch (currentClass)
            {
                case allClasses.DamageDealer:

                    maxHealth = classStats[0,0];
                    damage = (float)classStats[0,1];
                    abilityRecharge = classStats[0, 2];
                    maxAbilityUses = classStats[0, 3];
                    speed = classStats[0, 4];
                    break;
                case allClasses.Tank:
                    maxHealth = classStats[1, 0];
                    damage = (float)classStats[1, 1];
                    abilityRecharge = classStats[1, 2];
                    maxAbilityUses = classStats[1, 3];
                    speed = classStats[1, 4];
                    break;
                case allClasses.Healer:
                    maxHealth = classStats[2, 0];
                    damage = (float)classStats[2, 1];
                    abilityRecharge = classStats[2, 2];
                    maxAbilityUses = classStats[2, 3];
                    speed = classStats[2, 4];
                    break;
                case allClasses.RNG:
                    maxHealth = classStats[3, 0];
                    damage = (float)classStats[3, 1];
                    abilityRecharge = classStats[3, 2];
                    maxAbilityUses = classStats[3, 3];
                    speed = classStats[3, 4];
                    break;
                case allClasses.Charger:
                    maxHealth = classStats[4, 0];
                    damage = (float)classStats[4, 1];
                    abilityRecharge = classStats[4, 2];
                    maxAbilityUses = classStats[4, 3];
                    speed = classStats[4, 4];
                    break;
                case allClasses.Bag:
                    maxHealth = classStats[5, 0];
                    damage = (float)classStats[5, 1];
                    abilityRecharge = classStats[5, 2];
                    maxAbilityUses = classStats[5, 3];
                    speed = classStats[5, 4];
                    break;
            }

            int startingHealth = maxHealth;
            float startingDamage = damage;
            //Level stats
            for (int i = 0; i < currentLevel; i++)
            {
                //If base health is 100 health , adds 5 health per level
                maxHealth += startingHealth / 20;
                
                damage += startingDamage / 20;
            }
            //Skills stats
            bool glassCannon = false;
            bool stoneWall = false;
            for(int i = 0;i < skills.Length; i++)
            {
                switch (skills[i])
                {
                    case allSkills.Healthy:
                        maxHealth += 25;
                        break;
                    case allSkills.Violent:
                        damage += 5;
                        break;
                    case allSkills.Fast:
                        speed += 3;
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
                damage *= 1.5f;
            }
            if (stoneWall)
            {
                maxHealth = (int)(maxHealth * 1.5f);
                damage *= 0.75f;
            }
            damage = (float)Math.Round(damage,1);
        }
        public int Attack(out int dodge, out int healing, int attack = 0, bool doRandom = true,bool useEnumAttackInstead = false, attacks enumAttack = attacks.None,bool visual = false)
        {
            attacks currentAttack = attacks.None;
            dodge = 0;
            healing = 0;
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
                //Makes the attack a specific attack from the attacks enum
                currentAttack = enumAttack;
            }
            
            #endregion
            float attackDamage = damage;
            #region Randomizer Random Damage
            if (currentClass == allClasses.RNG && doRandom)
            {
                Random rnd = new Random();
                int extraRNGDamage = rnd.Next(-5 + rng_ExtraLuck, 5 + rng_ExtraLuck);
                attackDamage += extraRNGDamage;
            }
            #endregion
            int totalDamage = 0;
            float extraDamage = 0;
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
                    #region Ability
                case attacks.Class_Ability:
                    if (roundsUntilAbilityRecharge <= 0 && abilityUses < maxAbilityUses)
                    {
                        if (!visual)
                        {
                            roundsUntilAbilityRecharge = abilityRecharge;
                            abilityUses++;
                        }
                        switch (currentClass)
                        {
                            case allClasses.DamageDealer:
                                totalDamage = (int)((attackDamage * damageMultiplier) * 2f);
                                return totalDamage;

                            case allClasses.Tank:
                                healing = (maxHealth / 10);
                                HealCreature((maxHealth / 10), visual);
                                return (int)((damage * damageMultiplier) * 0.5f);

                            case allClasses.Healer:
                                healing = (maxHealth / 2);
                                HealCreature((maxHealth / 2), visual);
                                return 0;

                            case allClasses.RNG:
                                Random rnd2 = new Random();
                                int rand2 = rnd2.Next(30, 60);
                                int rand3 = rnd2.Next(10, 30);
                                HealCreature(-(maxHealth / 100) * rand3,false);
                                return (int)((maxHealth/100) * rand2);

                            case allClasses.Charger:
                                totalDamage = (int)((((attackDamage * 0.5f) + chargerCharge) * chargerCharge)*damageMultiplier);
                                if (!visual)
                                {
                                    chargerCharge = 0;
                                }
                                return totalDamage;

                            case allClasses.Bag:
                                return 10 * (currentLevel + 1);
                        }
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
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Light_Hitter)
                        {
                            extraDamage = extraDamage + (attackDamage * 1.1f);
                        }
                    }
                    totalDamage = (int)((attackDamage + extraDamage) * damageMultiplier);
                    if(prevAttack == attacks.Light_Hit)
                    {
                        totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                    }
                    if (!visual)
                    {
                        prevAttack = attacks.Light_Hit;
                    }
                    return totalDamage;
                #endregion
                    #region Heavy Hit
                case attacks.Heavy_Hit:
                    extraDamage = 0;
                    dodge = 25;
                    for (int i = 0; i < skills.Length; i++)
                    {
                        if (skills[i] == allSkills.Heavy_Hitter)
                        {
                            extraDamage +=attackDamage * 1.1f;
                        }
                    }
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 1;
                    }
                    totalDamage = (int)(((attackDamage * 1.5f) + extraDamage) * damageMultiplier);
                    if (prevAttack == attacks.Heavy_Hit)
                    {
                        totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                    }
                    if (!visual)
                    {
                        prevAttack = attacks.Heavy_Hit;
                    }
                    return totalDamage;
                #endregion
                #endregion

                #region ClassSpecificAttacks
                #region Damage Dealer
                    #region HP Sacrifice
                case attacks.DD_HPSacrifice:
                    dodge = 10;
                    if (!visual)
                    {
                        roundsUntilAbilityRecharge -= 2;
                    }
                    extraDamage = (int)((attackDamage) * damageMultiplier);
                    totalDamage = (int)(extraDamage * 2);
                    if (prevAttack == attacks.DD_HPSacrifice)
                    {
                        totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                    }
                    if (!visual)
                    {
                        prevAttack = attacks.DD_HPSacrifice;
                    }
                    HealCreature(-(maxHealth * 0.1f), visual);
                    healing = (int)-(maxHealth * 0.1f);
                    return totalDamage;
                #endregion
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
                    extraDamage = (int)((attackDamage) * damageMultiplier);
                    totalDamage = (int)(extraDamage / 1.5f);
                    if (prevAttack == attacks.HL_LifeSteal)
                    {
                        totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                    }
                    if (!visual)
                    {
                        prevAttack = attacks.HL_LifeSteal;
                    }
                    HealCreature(totalDamage, visual);
                    healing = totalDamage;
                    return totalDamage;
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
                        if (prevAttack == attacks.RG_Stats)
                        {
                            healing = (int)(totalDamage * (1 - attackSpamNerf));
                        }
                        totalDamage = (int)(((12 + rng_ExtraLuck) * nerf) * damage);
                        if (prevAttack == attacks.RG_Stats)
                        {
                            totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                        }
                        return totalDamage;
                    }
                    else
                    {
                        //Yeah.
                        Random rnd = new Random();
                        int rand = rnd.Next(5, 15);
                        dodge = rand;
                        rand = rnd.Next(-5 + rng_ExtraLuck, 10 + rng_ExtraLuck);
                        totalDamage = rand;
                        if (prevAttack == attacks.RG_Stats)
                        {
                            totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                        }
                        healing = totalDamage;
                        HealCreature(totalDamage,visual);
                        rand = rnd.Next((int)(((5 + rng_ExtraLuck) * nerf) * damage), (int)(((20 + rng_ExtraLuck) * nerf) * damage));
                        if (rand >= (int)(((18 + rng_ExtraLuck) * nerf) * damage))
                        {
                            rand = rnd.Next((int)(((18 + rng_ExtraLuck) * nerf) * damage), (int)(((30 + rng_ExtraLuck) * nerf) * damage));
                        }
                        totalDamage = rand;
                        if (prevAttack == attacks.RG_Stats)
                        {
                            totalDamage = (int)(totalDamage * (1 - attackSpamNerf));
                        }
                        if (!visual)
                        {
                            prevAttack = attacks.RG_Stats;
                        }
                        return totalDamage;
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
        public allSkills GetRandomSkill()
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
        public void HealCreature(float healing,bool doesntHeal)
        {
            if (!doesntHeal)
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
        }
    }
}