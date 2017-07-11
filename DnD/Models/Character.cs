using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Models
{

    public enum Locations
    {
        Front = 0,
        Back = 1
    }

    public class AttributeSet
    {
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Con { get; set; }
        public int Int { get; set; }
        public int Wis { get; set; }
        public int Cha { get; set; }

        public AttributeSet()
        { }

        public AttributeSet(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            Str = strength;
            Dex = dexterity;
            Con = constitution;
            Int = intelligence;
            Wis = wisdom;
            Cha = charisma;
        }

        public static int GetBonusFromRaw(int raw)
        {
            return raw / 2 - 5;
        }

    }

    public class CharacterStats
    {
        public int AttacksAgainst { get; set; }
        public int HitsAgainst { get; set; }
        public int DamageTaken { get; set; }
        public int OverKillTaken { get; set; }
        public int HealingReceived { get; set; }
        public int CritsAgainst { get; set; }
        public int HitWhileConcentrating { get; set; }
        public int ConcentrationBroken { get; set; }
        public int AttacksWithAdvantageAgainst { get; set; }
        public int TempHPAbsorbed { get; set; }
        public int WardHPAbsorbed { get; set; }
        public int ShortRestHealing { get; set; }
        public int TurnsTaken { get; set; }
        public int TurnsActive { get; set; }
        public int TurnsConcentrating { get; set; }
        public int TimesDropped { get; set; }
        public int TimesKilled { get; set; }
        public int SneakAttacks { get; set; }
        public int SneakAttackDamageDealt { get; set; }
        public int HexAttacks { get; set; }
        public int HexAttackDamageDealt { get; set; }
    }

    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsPC { get; set; }
        public int BaseAC { get; set; }
        public int CurrentAC { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public Locations StartingLocation { get; set; }
        public CombatActionKeyedCollection Actions { get; set; }
        public CombatActionKeyedCollection Abilities { get; set; }
        public int InitiativeBonus { get; set; }
        public int Initiative { get; set; }
        public Locations Location { get; set; }
        public int DeathSaveSuccesses { get; set; }
        public int DeathSaveFailures { get; set; }
        public bool IsConcentrating { get; set; }
        public AttributeSet AttributeBonuses { get; set; }
        public AttributeSet Saves { get; set; }
        public int[] MaxSpellSlots { get; set; }
        public int[] RemainingSpellSlots { get; set; }
        public EffectKeyedCollection Effects { get; set; }
        public int TempHP { get; set; }
        public int WardHP { get; set; }
        public int WardMaxHP { get; set; }
        public bool HasReaction { get; set; }
        public bool HasBonusAction { get; set; }
        public bool HasAction { get; set; }
        public bool HasKillThisRound { get; set; }
        public bool HasCritThisRound { get; set; }
        public bool UsedSneakAttack { get; set; }
        public string SneakAttackDamage { get; set; }
        public int[] HitDice { get; set; }
        public bool CCTarget { get; set; }
        public CharacterStats Statistics { get; set; }
        public bool HasActedThisEncounter { get; set; }
        public string CreatureType { get; set; }

        public Character(Monster monster, string name)
        {
            Name = name;
            IsPC = false;
            BaseAC = monster.ArmorClass;
            CurrentAC = monster.ArmorClass;
            MaxHP = monster.HitPoints;
            CurrentHP = monster.HitPoints;
            CreatureType = monster.Type;
            //TODO: starting location, actions, abilities
            AttributeBonuses = new AttributeSet()
            {
                Str = AttributeSet.GetBonusFromRaw(monster.Strength),
                Dex = AttributeSet.GetBonusFromRaw(monster.Dexterity),
                Con = AttributeSet.GetBonusFromRaw(monster.Constitution),
                Int = AttributeSet.GetBonusFromRaw(monster.Intelligence),
                Wis = AttributeSet.GetBonusFromRaw(monster.Wisdom),
                Cha = AttributeSet.GetBonusFromRaw(monster.Charisma)
            };
            InitiativeBonus = AttributeBonuses.Dex;
            Saves = new AttributeSet()
            {
                Str = monster.StrengthSave == 0 ? AttributeBonuses.Str : monster.StrengthSave,
                Dex = monster.DexteritySave == 0 ? AttributeBonuses.Dex : monster.DexteritySave,
                Con = monster.ConstitutionSave == 0 ? AttributeBonuses.Con : monster.ConstitutionSave,
                Int = monster.IntelligenceSave == 0 ? AttributeBonuses.Int : monster.IntelligenceSave,
                Wis = monster.WisdomSave == 0 ? AttributeBonuses.Wis : monster.WisdomSave,
                Cha = monster.CharismaSave == 0 ? AttributeBonuses.Cha : monster.CharismaSave
            };
            Statistics = new CharacterStats();
        }

        public Character(string name, bool isPC, Locations startLoc, int ac, int maxHP, int initBonus, int strSave, int dexSave, int conSave, int intSave, int wisSave, int chaSave)
        {
            Name = name;
            IsPC = isPC;
            BaseAC = ac;
            CurrentAC = ac;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            StartingLocation = startLoc;
            Location = startLoc;
            InitiativeBonus = initBonus;
            Saves = new AttributeSet(strSave, dexSave, conSave, intSave, wisSave, chaSave);
            Actions = new CombatActionKeyedCollection();
            Abilities = new CombatActionKeyedCollection();
            Effects = new EffectKeyedCollection();
            Statistics = new CharacterStats();
        }

        public Character(string name, bool isPC, Locations startLoc, int ac, int maxHP, int initBonus, AttributeSet saves)
        {
            Name = name;
            IsPC = isPC;
            BaseAC = ac;
            CurrentAC = ac;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            StartingLocation = startLoc;
            Location = startLoc;
            InitiativeBonus = initBonus;
            Saves = saves;
            Actions = new CombatActionKeyedCollection();
            Abilities = new CombatActionKeyedCollection();
            Effects = new EffectKeyedCollection();
            Statistics = new CharacterStats();
        }

        public void AddHitDice(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10)
        {
            HitDice = new int[10];
            HitDice[0] = h1;
            HitDice[1] = h2;
            HitDice[2] = h3;
            HitDice[3] = h4;
            HitDice[4] = h5;
            HitDice[5] = h6;
            HitDice[6] = h7;
            HitDice[7] = h8;
            HitDice[8] = h9;
            HitDice[9] = h10;
        }

        public void AddSpellSlots(int l1, int l2, int l3, int l4, int l5, int l6, int l7, int l8, int l9)
        {
            MaxSpellSlots = new int[9];
            MaxSpellSlots[0] = l1;
            MaxSpellSlots[1] = l2;
            MaxSpellSlots[2] = l3;
            MaxSpellSlots[3] = l4;
            MaxSpellSlots[4] = l5;
            MaxSpellSlots[5] = l6;
            MaxSpellSlots[6] = l7;
            MaxSpellSlots[7] = l8;
            MaxSpellSlots[8] = l9;
            RemainingSpellSlots = new int[9];
            RemainingSpellSlots[0] = l1;
            RemainingSpellSlots[1] = l2;
            RemainingSpellSlots[2] = l3;
            RemainingSpellSlots[3] = l4;
            RemainingSpellSlots[4] = l5;
            RemainingSpellSlots[5] = l6;
            RemainingSpellSlots[6] = l7;
            RemainingSpellSlots[7] = l8;
            RemainingSpellSlots[8] = l9;
        }

        public void Reset()
        {
            IsConcentrating = false;
            CurrentHP = MaxHP;
            if (MaxSpellSlots != null)
                MaxSpellSlots.CopyTo(RemainingSpellSlots, 0);
            foreach (var a in Actions)
                a.RemainingUses = a.MaxUses;
            foreach (var a in Abilities)
                a.RemainingUses = a.MaxUses;
            Effects = new EffectKeyedCollection();
            WardHP = WardMaxHP;
        }

        public bool IsTargetable()
        {
            if (Effects == null)
                return true;
            return !Effects.Where(e => e.RemovesFromPlay).Any();
        }

        public int GetSave(SaveTypes saveType)
        {
            int save = 0;
            switch (saveType)
            {
                case SaveTypes.Cha:
                    save = Saves.Cha;
                    break;
                case SaveTypes.Con:
                    save = Saves.Con;
                    break;
                case SaveTypes.Dex:
                    save = Saves.Dex;
                    break;
                case SaveTypes.Int:
                    save = Saves.Int;
                    break;
                case SaveTypes.Str:
                    save = Saves.Str;
                    break;
                case SaveTypes.Wis:
                    save = Saves.Wis;
                    break;
            }
            return save;
        }

        public bool IsCursedByAttacker(Character attacker)
        {
            return Effects.Where(e => (e.Name == Conditions.HunterMarked || e.Name == Conditions.Hexed) && e.Source == attacker).Any();
        }

        public bool HasAbility(string abilityName)
        {
            if (Abilities == null)
                return false;
            return Abilities.Contains(abilityName);
        }


        public bool HasCondition(Conditions condition)
        {
            if (Effects == null)
                return false;
            return Effects.Contains(condition);
        }

        public void AddCondition(Conditions condition)
        {
            if (!Effects.Contains(condition))
            {
                Effects.Add(new Effect(condition));
                if (condition == Conditions.Shielded)
                    CurrentAC += 5;
            }
        }

        public void AddCondition(Conditions condition, Character source)
        {
            if (!Effects.Contains(condition))
            {
                Effect effect = new Effect(condition);
                effect.Source = source;
                Effects.Add(effect);
                if (condition == Conditions.Shielded)
                    CurrentAC += 5;
            }
        }

        public void AddCondition(Conditions condition, Character source, int saveDC)
        {
            if (!Effects.Contains(condition))
            {
                Effect effect = new Effect(condition);
                effect.SaveDC = saveDC;
                effect.Source = source;
                Effects.Add(effect);
                if (condition == Conditions.Shielded)
                    CurrentAC += 5;
            }
        }

        public void RemoveCondition(Conditions condition)
        {
            if (Effects == null)
                return;
            if (Effects.Contains(condition))
            {
                Effects.Remove(condition);
                if (condition == Conditions.Shielded)
                    CurrentAC -= 5;
            }
        }

        public void RemoveConditionBySource(Conditions condition, Character source)
        {
            if (Effects == null)
                return;
            if (Effects.Contains(condition))
            {
                var effect = Effects[condition];
                if (effect.Source == source)
                {
                    Effects.Remove(condition);
                    if (condition == Conditions.Shielded)
                        CurrentAC -= 5;
                }
            }
        }

        public bool CanMove()
        {
            if (Effects == null)
                return true;
            return !Effects.Any(e => e.ProhibitsMovement);
        }

        public bool CanAct()
        {
            if (Effects == null)
                return true;
            return !Effects.Any(e => e.ProhibitsActions);
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }

        public CombatAction GetAbility(string abilityName)
        {
            if (Abilities == null || !Abilities.Contains(abilityName))
                return null;
            return Abilities[abilityName];
        }

        public bool AdvantageAgainst(AttackTypes attackType)
        {
            return ((attackType == AttackTypes.Ranged && Effects.Where(e => e.GrantsAdvantageAgainstRanged).Any()) || (attackType == AttackTypes.Melee && Effects.Where(e => e.GrantsAdvantageAgainstMelee).Any()));
        }

        public bool DisadvantageAgainst(AttackTypes attackType)
        {
            return ((attackType == AttackTypes.Ranged && Effects.Where(e => e.GrantsDisadvantageAgainstRanged).Any()) || (attackType == AttackTypes.Melee && Effects.Where(e => e.GrantsDisadvantageAgainstMelee).Any()));
        }

        public bool AdvantageFor(AttackTypes attackType)
        {
            return ((attackType == AttackTypes.Ranged && Effects.Where(e => e.GrantsAdvantageRanged).Any()) || (attackType == AttackTypes.Melee && Effects.Where(e => e.GrantsAdvantageMelee).Any()));
        }

        public bool DisadvantageFor(AttackTypes attackType)
        {
            return ((attackType == AttackTypes.Ranged && Effects.Where(e => e.GrantsDisadvantageRanged).Any()) || (attackType == AttackTypes.Melee && Effects.Where(e => e.GrantsDisadvantageMelee).Any()));
        }
    }
}
