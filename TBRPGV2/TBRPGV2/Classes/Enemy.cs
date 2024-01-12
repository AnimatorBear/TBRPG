namespace TBRPGV2
{
    class Enemy
    {
        Creature enemy;
        Creature player;
        int prevAttack = 0;

        public enum sprites { None,Tri,Healer,Bag }
        public enum spriteType {Idle,Dead}
        string[,][] enemySprites;

        public Enemy(Creature _enemy,Creature _player)
        {
            enemy = _enemy;
            player = _player;
            SelectSprite(sprites.None);
        }
        //Makes the enemy choose a random skill that fits its playstyle
        public void ChooseExtraSkill()
        {
            //Not used yet
        }

        public void SelectSprite(sprites sprite)
        {
            switch (sprite)
            {
                case sprites.None:
                    string[,][] testSprites = { {
                    //Idle
                    new string[]{
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            "},
                    //Idk
                    new string[]{
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            "} } };
                    enemySprites = testSprites;
                    break;
                case sprites.Tri:
                    string[,][]  triSprites = { {
                    //Idle
                    new string[]{
                    "                            ",
                    "             ___            ",
                    "            /   \\           ",
                    "           / o o \\          ",
                    "           \\  ^  /          ",
                    "            \\ ‾ /           ",
                    "             | |            ",
                    "         /‾‾‾\\_/‾‾‾\\        ",
                    "        /_/       \\_\\       ",
                    "        | |       | |       ",
                    "        | |       | |       ",
                    "        \\_|=={=}==|_/       ",
                    "          |   |   |         "}
                    //Dying
                    ,
                    new string[]{
                    "                            ",
                    "             ___            ",
                    "            /   \\           ",
                    "           / X X \\          ",
                    "           \\  ^  /          ",
                    "            \\ ‾ /           ",
                    "             | |            ",
                    "   |‾‾‾‾‾‾‾‾‾\\_/‾‾‾‾‾‾‾‾‾|  ",
                    "   |_______       _______|  ",
                    "          |       |         ",
                    "          |       |         ",
                    "          |=={=}==|         ",
                    "          |   |   |         "}
                        } };
                    enemySprites = triSprites;
                    break;
                case sprites.Healer:
                    string[,][] hlSprites = { {
                    //Idle
                    new string[]{
                    "                            ",
                    "                            ",
                    "            -----           ",
                    "           | o o |          ",
                    "           |  v  |          ",
                    "            \\ ⏝ /           ",
                    "             | |            ",
                    "         /‾‾‾\\_/‾‾‾\\        ",
                    "        /_/   |   \\_\\       ",
                    "        | | --+-- | |       ",
                    "        | |   |   | |       ",
                    "        \\_|-------|_/       ",
                    "          |   |   |         "}
                    //Dying
                    ,
                    new string[]{
                    "                            ",
                    "                            ",
                    "            -\\-/-           ",
                    "           | X X |          ",
                    "           |  v  |          ",
                    "            \\ - /           ",
                    "             | |            ",
                    "         /‾‾‾\\_/‾‾‾\\        ",
                    "        /_/   |   \\_\\       ",
                    "        | | --+-- | |       ",
                    "        | |   |   | |       ",
                    "        \\_|-------|_/       ",
                    "          |   |   |         "}
                        } };
                    enemySprites = hlSprites;
                    break;
                case sprites.Bag:
                    string[,][] bgSprites = { {
                    //Idle
                    new string[]{
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "             _--_           ",
                    "           /‾    \\_         ",
                    "          /        \\        ",
                    "          \\        /        ",
                    "           \\______/         "}
                    //Dying
                    ,
                    new string[]{
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "                            ",
                    "             _--_           ",
                    "           /‾    \\_         ",
                    "          /   >:(  \\        ",
                    "          \\        /        ",
                    "           \\______/         "}
                        } };
                    enemySprites = bgSprites;
                    break;
            }
        }
        public void WriteEnemySprite(spriteType sp, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            int[] spots = { 0, 0 };
            switch (sp)
            {
                case spriteType.Idle:
                    spots = new int[] { 0, 0 };
                    break;
                case spriteType.Dead:
                    spots = new int[] { 0, 1 };
                    break;
            }
            for (int i = 0; i < enemySprites[spots[0], spots[1]].Length; i++)
            {
                Console.SetCursorPosition(22, 3 + i);
                Console.WriteLine(enemySprites[spots[0], spots[1]][i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        //Makes the enemy choose a random class
        public Creature.allClasses ChooseClass()
        {
            Creature.allClasses currentClass;
            //DD, Tank, Healer, RNG, Charger , Bag
            int[] classValues = { 20, 20, 20, 20, 20,0 };
            switch (player.currentClass)
            {
                case Creature.allClasses.Class_None:
                    Console.WriteLine("ERROR: Player has no class");
                    break;
                case Creature.allClasses.DamageDealer:
                    classValues[0] -= 10;
                    break;
                case Creature.allClasses.Tank:
                    classValues[1] -= 10;
                    break;
                case Creature.allClasses.Healer:
                    classValues[2] -= 10;
                    break;
                case Creature.allClasses.RNG:
                    classValues[3] -= 10;
                    break;
                case Creature.allClasses.Charger:
                    classValues[4] -= 10;
                    break;
                case Creature.allClasses.Bag:
                    classValues[0] = 0;
                    classValues[1] = 0;
                    classValues[2] = 0;
                    classValues[3] = 0;
                    classValues[4] = 0;
                    classValues[5] = 100;
                    break;
            }

            Random rnd = new Random();
            int total = 0;
            for(int i = 0; i < classValues.Length; i++)
            {
                total += classValues[i];
            }
            int classNumber = rnd.Next(1, total);
            int numb = 0;
            for (int i = 0; i < 6; i++)
            {
                if (classNumber > numb  && classNumber <= numb + classValues[i])
                {
                    numb = i;
                    break;
                }
                numb += classValues[i];
            }
            switch (numb)
            {
                case 0:
                    currentClass = Creature.allClasses.DamageDealer;
                    break;
                case 1:
                    currentClass = Creature.allClasses.Tank;
                    break;
                case 2:
                    currentClass = Creature.allClasses.Healer;
                    break;
                case 3:
                    currentClass = Creature.allClasses.RNG;
                    break;
                case 4:
                    currentClass = Creature.allClasses.Charger;
                    break;
                case 5:
                    currentClass = Creature.allClasses.Bag;
                    break;

                default:
                    Console.WriteLine("Error! Cant decide class! Went to default DamageDealer");
                    currentClass = Creature.allClasses.DamageDealer;
                    Thread.Sleep(10000);
                    break;
            }
            switch (currentClass)
            {
                case Creature.allClasses.DamageDealer:
                    SelectSprite(sprites.Tri);
                    break;
                case Creature.allClasses.Tank:
                    SelectSprite(sprites.Tri);
                    break;
                case Creature.allClasses.Healer:
                    SelectSprite(sprites.Healer);
                    break;
                case Creature.allClasses.RNG:
                    SelectSprite(sprites.Tri);
                    break;
                case Creature.allClasses.Charger:
                    SelectSprite(sprites.Tri);
                    break;
                case Creature.allClasses.Bag:
                    SelectSprite(sprites.Bag);
                    break;
            }
            return currentClass;
        }
        //Chooses what attack the enemy will use next
        public int ChooseAttack()
        {
            int[] attackValue = new int[5];
            int[] healingValue = new int[5];
            int[] dodgeValue = new int[5];
            for (int i = 0;i < enemy.characterAttacks.Length - 1; i++)
            {
                switch(enemy.characterAttacks[i])
                {
                    case Creature.attacks.Light_Hit:
                        Random rnd = new Random();

                        attackValue[i] = 13 + rnd.Next(0,5);
                        healingValue[i] = 0;
                        dodgeValue[i] = 5;
                        break;
                    case Creature.attacks.Heavy_Hit:
                        Random rand = new Random();
                        attackValue[i] = 10 + rand.Next(0, 5);
                        healingValue[i] = 0;
                        dodgeValue[i] = 15;
                        break;
                    case Creature.attacks.RG_Stats:
                        attackValue[i] = 12;
                        healingValue[i] = 0;
                        dodgeValue[i] = 10;
                        break;
                    case Creature.attacks.HL_LifeSteal:
                        attackValue[i] = 10;
                        healingValue[i] = 15;
                        dodgeValue[i] = 10;
                        break;
                    case Creature.attacks.DD_HPSacrifice:
                        attackValue[i] = 13;
                        healingValue[i] = 0;
                        dodgeValue[i] = 10;
                        break;
                }
            }
            switch (enemy.currentClass)
            {
                case Creature.allClasses.DamageDealer:
                    attackValue[4] = 30;
                    healingValue[4] = 0;
                    dodgeValue[4] = 0;
                    break;
                case Creature.allClasses.Tank:
                    attackValue[4] = 15;
                    healingValue[4] = 15;
                    dodgeValue[4] = 0;
                    break;
                case Creature.allClasses.Healer:
                    attackValue[4] = 0;
                    healingValue[4] = 40;
                    dodgeValue[4] = 0;
                    break;
                case Creature.allClasses.RNG:
                    attackValue[4] = 20;
                    healingValue[4] = 0;
                    dodgeValue[4] = 0;
                    break;
                case Creature.allClasses.Charger:
                    attackValue[4] = 3 * enemy.chargerCharge;
                    healingValue[4] = 0;
                    dodgeValue[4] = 0;
                    break;
                case Creature.allClasses.Bag:
                    attackValue[4] = 1;
                    healingValue[4] = 0;
                    dodgeValue[4] = 0;
                    break;
            }

            int attackChance = 0;
            int healChance = 0;
            switch (enemy.currentClass)
            {
                case Creature.allClasses.DamageDealer:
                    attackChance = 5;
                    healChance = 0;
                    break;
                case Creature.allClasses.Tank:
                    attackChance = 5;
                    healChance = 5;
                    break;
                case Creature.allClasses.Healer:
                    attackChance = 0;
                    healChance = 5;
                    break;
                case Creature.allClasses.RNG:
                    attackChance = 1;
                    healChance = 3;
                    break;
                case Creature.allClasses.Charger:
                    attackChance = 5;
                    healChance = 0;
                    break;
                case Creature.allClasses.Bag:
                    attackChance = 5;
                    healChance = 0;
                    break;
            }

            for(int i = 0; i < enemy.skills.Length; i++)
            {
                switch(enemy.skills[i])
                {
                    case Creature.allSkills.Healthy:
                        healChance += 1;
                        break;
                    case Creature.allSkills.Violent:
                        attackChance += 1;
                        break;
                }
            }

            attackChance += (int)((enemy.maxHealth + enemy.health) * 0.1f);
            if (enemy.health > enemy.maxHealth * 0.8f)
            {
                attackChance += 20;
                healChance -= 20;
            }
            else
            {
                healChance += (int)((enemy.maxHealth - enemy.health) * 0.4f);
            }

            if(player.health < player.maxHealth * 0.3f)
            {
                attackChance += 10;
                healChance -= 10;
            }
            int bestOption = -1;
            float bestOptionDifference = 10000;
            for (int i = 0; i < attackValue.Length; i++)
            {
                if (attackValue[i] == 0 && healingValue[i] == 0)
                {
                    continue;
                }
                if(healChance < 0 && healingValue[i] > 0)
                {
                    continue;
                }
                if (enemy.roundsUntilAbilityRecharge > 0 && i == 4)
                {
                    continue;
                }

                int difference;
                int attackdifference = attackValue[i] - attackChance;
                if(attackdifference < 0) { attackdifference = -attackdifference; }
                difference = attackdifference;

                int hpdifference = healingValue[i] - healChance;
                if (hpdifference < 0) { hpdifference = -hpdifference; }
                difference += hpdifference;
                difference += (int)(dodgeValue[i] * 0.3f);
                if (i == prevAttack)
                {
                    Random rnd = new Random();
                    difference += rnd.Next(70, 100);
                }
                if (bestOption == -1 || difference < bestOptionDifference)
                {
                    bestOption = i;
                    bestOptionDifference = difference;
                }

            }
            return bestOption + 1;
        }
    }
}