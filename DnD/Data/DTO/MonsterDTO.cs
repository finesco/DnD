using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DnD.DTO
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public string HitDice { get; set; }
        public string Speed { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int StrengthSave { get; set; }
        public int DexteritySave { get; set; }
        public int ConstitutionSave { get; set; }
        public int IntelligenceSave { get; set; }
        public int WisdomSave { get; set; }
        public int CharismaSave { get; set; }
        public int Athletics { get; set; }
        public int Acrobatics { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Arcana { get; set; }
        public int History { get; set; }
        public int Investigation { get; set; }
        public int Nature { get; set; }
        public int Religion { get; set; }
        public int AnimalHandling { get; set; }
        public int Insight { get; set; }
        public int Medicine { get; set; }
        public int Perception { get; set; }
        public int Survival { get; set; }
        public int Deception { get; set; }
        public int Intimidation { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageResistances { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string ChallengeRating { get; set; }
        public List<SpecialAbility> SpecialAbilities { get; set; }
        public List<Action> Actions { get; set; }
        public List<LegendaryAction> LegendaryActions { get; set; }
    }

    public class SpecialAbility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int AttackBonus { get; set; }

        public int MonsterId { get; set; }
        public Monster Monster { get; set; }
    }

    public class Action
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int AttackBonus { get; set; }
        public string DamageDice { get; set; }
        public int DamageBonus { get; set; }

        public int MonsterId { get; set; }
        public Monster Monster { get; set; }
    }

    public class LegendaryAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int AttackBonus { get; set; }
        public string DamageDice { get; set; }
        public int DamageBonus { get; set; }

        public int MonsterId { get; set; }
        public Monster Monster { get; set; }
    }

}
