namespace TBRPGV2
{
    class EnchChair : Item
    {

        public EnchChair(Creature creature, float heals, float dodge, float dmg, string name) : base(creature, heals, dodge, dmg, name)
        {
            owner = creature;
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
            itemName = name;
            description = new string[5] { " The Chair of", "Binary Harmony,", "   Used by", "The Chair God: ", "    \"Mike\"" };
            uses = 1000;
        }

        /*
        
        Original story written by ChatGPT, Summary of the first part written by Bear.

        Enchanted Chair of Binary Harmony

        The chair of Binary Harmony is a enchanted chair sold 
        at Mystic Relics, A store which is run by Professor Smith.

        The chair brings peace to all computers and "the digital world" around you.
        It was first eaten by Mike, The chair god, to restore peace between
        him and the digital world. This was needed after he 
        ate the Chair of Techno-Mystical Wonders  which allowed him to 
        talk with computers, but made the digital world hate him.

        */
        public override void UseItem()
        {
            owner.HealCreature(healing,false);

            //2,3 + 69 , 15
            for(int i = 0; i < 500; i++) 
            {
                Random rnd = new Random();
                Console.ForegroundColor = ConsoleColor.Green;   
                int le = rnd.Next(2, 69);
                int to = rnd.Next(3, 16);
                int nu = rnd.Next(0,2);
                if(le < 20 || le > 49)
                {
                    if(to < 5 || to > 13)
                    {
                        if(le < 11 || le > 58)
                        {

                        }
                        else
                        {
                            Console.SetCursorPosition(le, to);
                            Console.WriteLine($" {nu} ");
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(le, to);
                        Console.WriteLine($" {nu} ");
                    }
                }
                else
                {
                    i--;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}