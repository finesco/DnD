using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Models
{
    public enum ActionTypes
    {
        Attack = 0, //attacks require an attack roll
        Heal = 1,
        Spell = 2 //spells do not have an attack roll
    }

    public enum AttackTypes
    {
        Melee = 0,
        Ranged = 1,
        AE = 2,
        Physical = 3
    }

    public enum SaveTypes
    { Str, Dex, Con, Wis, Int, Cha }

    public enum RefreshTypes
    { AtWill, ShortRest, LongRest, Spell, Recharge }

    public enum HealingPriorities
    { Low, Medium, High }

    public class CombatActionKeyedCollection : KeyedCollection<string, CombatAction>
    {
        protected override string GetKeyForItem(CombatAction action)
        {
            return action.Name;
        }
    }

    public class CombatAction
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public ActionTypes ActionType { get; set; }
        public string Damage { get; set; }
        public AttackTypes AttackType { get; set; }
        public int SaveDC { get; set; }
        public bool SaveForHalf { get; set; }
        public bool ApplyEffect;
        public Conditions EffectApplied { get; set; }
        public int NumAttacks { get; set; }
        public int AttacksMade { get; set; }
        public int Hits { get; set; }
        public int DamageDealt { get; set; }
        public int OverKillDealt { get; set; }
        public int Kills { get; set; }
        public int DamageBonus { get; set; }
        public SaveTypes SaveType { get; set; }
        public bool RequiresConcentration { get; set; }
        public bool AutomaticHit { get; set; }
        public bool GWF { get; set; }
        public bool GWM { get; set; }
        public bool SS { get; set; }
        public TargetingParms Targeting { get; set; }
        public bool IsBonusAction { get; set; }
        public bool IsReaction { get; set; }
        public int Crits { get; set; }
        public int AttacksWithAdvantage { get; set; }
        public int TimesUsed { get; set; }
        public int MaxUses { get; set; }
        public int RemainingUses { get; set; }
        public RefreshTypes RefreshType { get; set; }
        public int SpellLevel { get; set; }
        public int TargetLimit { get; set; }
        public int HealingDone { get; set; }
        public int OverHealingDone { get; set; }
        public HealingPriorities HealingPriority { get; set; }
        public int PowerAttacks { get; set; }
        public int RechargeNumber { get; set; }

        public CombatAction(string name)
        {
            Name = name;
            Description = name;
        }

        public CombatAction(string name, string dmg, int dmgBonus, int maxUses, RefreshTypes refreshType, int spellLevel, HealingPriorities priority)
        {
            //post combat healing constructor
            Name = name;
            Description = name;
            Damage = dmg;
            DamageBonus = dmgBonus;
            MaxUses = maxUses;
            RemainingUses = maxUses;
            RefreshType = refreshType;
            SpellLevel = spellLevel;
            HealingPriority = priority;
            Targeting = new TargetingParms();
            Targeting.AddSpecialParm(SpecialParms.PostCombatHeal);
        }

        public CombatAction(string name, int attackBonus, string damage, int dmgBonus, int numAttacks, AttackTypes attackType,
            bool usesGWF, bool usesGWM, bool usesSS, bool isBonus, bool isReaction, TargetingParms targeting)
        {
            //attack constructor
            Name = name;
            Description = name;
            AttackBonus = attackBonus;
            ActionType = ActionTypes.Attack;
            AttackType = attackType;
            Damage = damage;
            NumAttacks = numAttacks;
            DamageBonus = dmgBonus;
            GWF = usesGWF;
            GWM = usesGWM;
            SS = usesSS;
            IsBonusAction = isBonus;
            IsReaction = isReaction;
            Targeting = targeting;
            RemainingUses = 1;
            TargetLimit = 1;
        }

        public CombatAction(string name, ActionTypes actionType, string damage, int dmgBonus, bool isBonus, bool isReaction, TargetingParms targeting,
            int maxUses, RefreshTypes refreshType, int spellLevel, int targetLimit, AttackTypes attackType, SaveTypes saveType, int saveDC, bool saveForHalf, bool applyEffect,
            Conditions effectToApply, bool requiresConc, bool autoHit, int numAttacks, int attackBonus)
        {
            //spell constructor
            Name = name;
            Description = name;
            ActionType = actionType;
            Damage = damage;
            DamageBonus = dmgBonus;
            IsBonusAction = isBonus;
            IsReaction = isReaction;
            Targeting = targeting;
            MaxUses = maxUses;
            RemainingUses = maxUses;
            RefreshType = refreshType;
            SpellLevel = spellLevel;
            TargetLimit = targetLimit;
            AttackType = attackType;
            SaveDC = saveDC;
            SaveForHalf = saveForHalf;
            ApplyEffect = applyEffect;
            EffectApplied = effectToApply;
            RequiresConcentration = requiresConc;
            AutomaticHit = autoHit;
            SaveType = saveType;
            NumAttacks = numAttacks;
            AttackBonus = attackBonus;
        }

        public CombatAction(string name, ActionTypes actionType, string damage, int dmgBonus, bool isBonus, bool isReaction, TargetingParms targeting,
            int maxUses, RefreshTypes refreshType, int spellLevel, int targetLimit, AttackTypes attackType, SaveTypes saveType, int saveDC, bool saveForHalf, bool applyEffect,
            Conditions effectToApply, bool requiresConc, bool autoHit, int numAttacks, int attackBonus, int refresh)
        {
            //spell constructor
            Name = name;
            Description = name;
            ActionType = actionType;
            Damage = damage;
            DamageBonus = dmgBonus;
            IsBonusAction = isBonus;
            IsReaction = isReaction;
            Targeting = targeting;
            MaxUses = maxUses;
            RemainingUses = maxUses;
            RefreshType = refreshType;
            SpellLevel = spellLevel;
            TargetLimit = targetLimit;
            AttackType = attackType;
            SaveDC = saveDC;
            SaveForHalf = saveForHalf;
            ApplyEffect = applyEffect;
            EffectApplied = effectToApply;
            RequiresConcentration = requiresConc;
            AutomaticHit = autoHit;
            SaveType = saveType;
            NumAttacks = numAttacks;
            AttackBonus = attackBonus;
            RechargeNumber = refresh;
        }

        public CombatAction(string name, ActionTypes actionType, string damage, int dmgBonus, bool isBonus, bool isReaction, TargetingParms targeting,
            int maxUses, RefreshTypes refreshType, int spellLevel, int targetLimit)
        {
            //heal constructor
            Name = name;
            Description = name;
            ActionType = actionType;
            Damage = damage;
            DamageBonus = dmgBonus;
            IsBonusAction = isBonus;
            IsReaction = isReaction;
            Targeting = targeting;
            MaxUses = maxUses;
            RemainingUses = maxUses;
            RefreshType = refreshType;
            SpellLevel = spellLevel;
            TargetLimit = targetLimit;
        }

        public CombatAction(string name, int attackBonus, string damage, int dmgBonus, int numAttacks, AttackTypes attackType)
        {
            //basic attack constructor
            Name = name;
            Description = name;
            AttackBonus = attackBonus;
            ActionType = ActionTypes.Attack;
            AttackType = attackType;
            Damage = damage;
            NumAttacks = numAttacks;
            DamageBonus = dmgBonus;
            Targeting = new TargetingParms();
            TargetLimit = 1;
        }

        public bool IsAvailable(int[] charSpellSlots)
        {
            return RefreshType == RefreshTypes.AtWill || ((RefreshType == RefreshTypes.ShortRest || RefreshType == RefreshTypes.LongRest || RefreshType == RefreshTypes.Recharge) && RemainingUses > 0) || 
                (RefreshType == RefreshTypes.Spell && charSpellSlots[SpellLevel - 1] > 0);
        }
    }
}
