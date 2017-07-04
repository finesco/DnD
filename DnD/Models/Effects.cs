using System.Collections.ObjectModel;

namespace DnD.Models
{

    public enum Conditions
    {
        Stunned,
        Dying,
        Dead,
        Unconscious,
        Prone,
        Blinded,
        Grappled,
        Poisoned,
        Restrained,
        Hexed,
        Blessed,
        Shielded,
        TashasHideousLaughter,
        Webbed,
        HunterMarked,
        Banished,
        CombatInspired,
        Brawled,
        Enhanced,
        Invisible
    }

    public enum ExpiryTypes { None, Save, AbilityCheck, BeginningSourceTurn }

    public class EffectKeyedCollection : KeyedCollection<Conditions, Effect>
    {
        protected override Conditions GetKeyForItem(Effect effect)
        {
            return effect.Name;
        }
    }


    public class Effect
    {
        public Conditions Name { get; set; }
        public bool ProhibitsActions { get; set; }
        public SaveTypes SaveType { get; set; }
        public bool GrantsAdvantageMelee { get; set; }
        public bool GrantsAdvantageRanged { get; set; }
        public bool GrantsDisadvantageMelee { get; set; }
        public bool GrantsDisadvantageRanged { get; set; }
        public bool GrantsAdvantageAgainstMelee { get; set; }
        public bool GrantsAdvantageAgainstRanged { get; set; }
        public bool GrantsDisadvantageAgainstMelee { get; set; }
        public bool GrantsDisadvantageAgainstRanged { get; set; }
        public bool GrantsAdvantagePhysical { get; set; }
        public bool CanEndWithMove { get; set; }
        public bool CanEndWithAction { get; set; }
        public bool RemovesFromPlay { get; set; }
        public bool ProhibitsMovement { get; set; }
        public Character Source { get; set; }
        public ExpiryTypes ExpiryType { get; set; }
        public bool ConcentrationRequired { get; set; }
        public int SaveDC { get; set; }

        public Effect(Conditions condition)
        {
            Name = condition;
            switch (condition)
            {
                case Conditions.Blinded:
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    GrantsDisadvantageMelee = true;
                    GrantsDisadvantageRanged = true;
                    break;
                case Conditions.Enhanced:
                    GrantsAdvantagePhysical = true;
                    break;
                case Conditions.Banished:
                    ConcentrationRequired = true;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    RemovesFromPlay = true;
                    break;
                case Conditions.Dead:
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    break;
                case Conditions.Dying:
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    break;
                case Conditions.Grappled:
                    ProhibitsMovement = true;
                    break;
                case Conditions.Poisoned:
                    GrantsDisadvantageMelee = true;
                    GrantsDisadvantageRanged = true;
                    break;
                case Conditions.Prone:
                    GrantsDisadvantageRanged = true;
                    GrantsDisadvantageMelee = true;
                    GrantsDisadvantageAgainstRanged = true;
                    GrantsAdvantageAgainstMelee = true;
                    ProhibitsMovement = true;
                    CanEndWithMove = true;
                    break;
                case Conditions.Restrained:
                    ProhibitsMovement = true;
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    GrantsDisadvantageMelee = true;
                    GrantsDisadvantageRanged = true;
                    break;
                case Conditions.Stunned:
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    break;
                case Conditions.Unconscious:
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    break;
                case Conditions.Hexed:
                    ConcentrationRequired = true;
                    break;
                case Conditions.HunterMarked:
                    ConcentrationRequired = true;
                    break;
                case Conditions.Blessed:
                    ConcentrationRequired = true;
                    break;
                case Conditions.TashasHideousLaughter:
                    ConcentrationRequired = true;
                    SaveType = SaveTypes.Wis;
                    ExpiryType = ExpiryTypes.Save;
                    ProhibitsActions = true;
                    ProhibitsMovement = true;
                    break;
                case Conditions.Shielded:
                    ExpiryType = ExpiryTypes.BeginningSourceTurn;
                    break;
                case Conditions.Webbed:
                    ProhibitsMovement = true;
                    GrantsAdvantageAgainstMelee = true;
                    GrantsAdvantageAgainstRanged = true;
                    GrantsDisadvantageMelee = true;
                    GrantsDisadvantageRanged = true;
                    ConcentrationRequired = true;
                    CanEndWithAction = true;
                    break;
                case Conditions.Invisible:
                    GrantsAdvantageMelee = true;
                    GrantsAdvantageRanged = true;
                    GrantsDisadvantageAgainstMelee = true;
                    GrantsDisadvantageAgainstRanged = true;
                    ConcentrationRequired = true;
                    break;
            }
        }
    }
}

