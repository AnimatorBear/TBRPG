using System;
using System.Text.Json.Serialization;

namespace TBRPGV2
{
    class SkillReroll : Item
    {

        [JsonConstructor]
        public SkillReroll()
        {

        }
        public SkillReroll(float heals, float dodge, float dmg, string name) : base( heals, dodge, dmg, name)
        {
            healing = heals;
            tempDodge = dodge;
            tempDamage = dmg;
            itemName = name;
            description = new string[5] { " The Chair of", "Binary Harmony,", "   Used by", "The Chair God: ", "    \"Mike\"" };
            uses = 1000;
        }
        public override void UseItem(Creature owner)
        {
            Creature.allSkills skill = Creature.allSkills.None;
            while (skill == Creature.allSkills.None)
            {
                int amount = Program.SelectSkill();
                skill = Program.MoveSelectedSkill(amount, 0);
            }
            owner.skills[0] = skill;
            Console.Clear();
            Program.DrawText(Program.currentEnemy, ConsoleColor.DarkGray);
            Program.DrawBattle(ConsoleColor.DarkGray);

        }
    }
}