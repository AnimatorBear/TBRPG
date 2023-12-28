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
            owner.skills[0] = Creature.allSkills.None;
            while (owner.skills[0] == Creature.allSkills.None)
            {
                int amount = Program.SelectSkill();
                Program.MoveSelectedSkill(amount, 0);
            }

        }
    }
}