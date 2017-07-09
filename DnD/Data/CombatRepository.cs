using DnD.Models;
using log4net;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DnD.Data
{
    public class CombatRepository : ICombatRepository
    {
        private ILog _log;
        private readonly DnDContext _context;

        public CombatRepository(ILog logger, DnDContext context)
        {
            _log = logger;
            _context = context;
        }

        public Encounter LoadEncounter(int encounterId)
        {
            throw new NotImplementedException();
        }

        public Session LoadSession(int sessionId)
        {
            Session session = new Session(0) { Trials = 10000, Level = 1, NPCTargetingStyle = TargetingStyle.Spread, PCTargetingStyle = TargetingStyle.Focus };
            Character char1 = new Character("Krym", true, Locations.Front, 16, 12, -1, 5, -1, 4, 0, -1, 2);
            TargetingParms targeting = new TargetingParms();
            targeting.TargetSelfOnly = true;
            targeting.HealthTarget = HealthTargets.Bloodied;
            targeting.HasHealthTarget = true;
            char1.Actions.Add(new CombatAction("Second Wind", ActionTypes.Heal, "d10", 1, true, false, targeting, 1, RefreshTypes.ShortRest, 0, 1));
            targeting = new TargetingParms();
            targeting.AddSpecialParm(SpecialParms.GWMBonus);
            char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, true, false, targeting));
            char1.Actions.Add(new CombatAction("Greatsword swing", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, false, false, new TargetingParms()));
            char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
            char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            session.PCs.Add(char1);
            char1 = new Character("Nyles", true, Locations.Back, 18, 10, 2, -1, 2, 2, 4, 5, -1);
            char1.AddSpellSlots(2, 0, 0, 0, 0, 0, 0, 0, 0);
            targeting = new TargetingParms();
            targeting.TargetFriendly = true;
            targeting.HealthTarget = HealthTargets.Dying;
            targeting.HasHealthTarget = true;
            char1.Actions.Add(new CombatAction("Healing Word", ActionTypes.Heal, "d4", 2, true, false, targeting, 0, RefreshTypes.Spell, 1, 1));
            char1.Actions.Add(new CombatAction("Short Sword", 4, "d6", 2, 1, AttackTypes.Melee));
            char1.Abilities.Add(new CombatAction("Healing Word (Postcombat)", "d4", 2, 1, RefreshTypes.Spell, 1, HealingPriorities.Low));
            char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            session.PCs.Add(char1);
            char1 = new Character("Sanchez", true, Locations.Back, 14, 10, 3, -1, 5, 2, 1, 1, 3);
            char1.SneakAttackDamage = "d6";
            char1.Actions.Add(new CombatAction("Short sword", 5, "d6", 3, 1, AttackTypes.Melee));
            char1.Actions.Add(new CombatAction("Dagger (off hand)", 5, "d4", 0, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
            char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            session.PCs.Add(char1);
            char1 = new Character("Jennifer", true, Locations.Front, 16, 12, -1, 3, -1, 2, 1, -1, 4);
            targeting = new TargetingParms();
            targeting.TargetFriendly = true;
            targeting.HealthTarget = HealthTargets.Dying;
            targeting.HasHealthTarget = true;
            targeting.HasSpecificTarget = true;
            targeting.SpecificTarget = "Nyles";
            char1.Actions.Add(new CombatAction("Lay on Hands", ActionTypes.Heal, "", 5, false, false, targeting, 1, RefreshTypes.LongRest, 0, 1));
            targeting = new TargetingParms();
            targeting.AddSpecialParm(SpecialParms.GWMBonus);
            char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 5, "2d6", 3, 1, AttackTypes.Melee, false, true, false, true, false, targeting));
            char1.Actions.Add(new CombatAction("Greatsword swing", 5, "2d6", 3, 1, AttackTypes.Melee, false, true, false, false, false, new TargetingParms()));
            char1.Abilities.Add(new CombatAction("Lay on Hands", "", 5, 1, RefreshTypes.LongRest, 0, HealingPriorities.High));
            char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
            session.PCs.Add(char1);
            Encounter enc1 = new Encounter();
            for (int i = 0; i < 4; i++)
            {
                enc1.Opponents.Add(getGenericMonster("1/4").Result);
                enc1.Opponents[i].Name += " " + (i + 1).ToString();
            }
            enc1.Difficulty = EncounterDifficulties.Hard;
            session.Encounters.Add(enc1);

            return session;
        }

        public void SaveResults()
        {
            throw new NotImplementedException();
        }

        public async Task<Character> getGenericMonster(string cr)
        {
            var genericMonsterStats = await _context.GenericMonsterStats.SingleOrDefaultAsync(m => m.CR == cr);
            if (genericMonsterStats == null)
                return null;
            Character c = new Character($"Generic CR {cr}", false, Locations.Front, genericMonsterStats.ArmorClass, genericMonsterStats.HitPoints, 1,
                2 + genericMonsterStats.ProficiencyBonus, 1, 2 + genericMonsterStats.ProficiencyBonus, 1, 1, 1)
            { Level = 0 };
            switch (cr)
            {
                case "0":
                    c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "d2", -1, 1, AttackTypes.Melee));
                    break;
                case "1/8":
                    c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "d4", 0, 1, AttackTypes.Melee));
                    break;
                case "1/4":
                    c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "d8", 0, 1, AttackTypes.Melee));
                    break;
                case "1/2":
                    c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "2d6", 0, 1, AttackTypes.Melee));
                    break;
                default:
                    c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "d10", (genericMonsterStats.Damage - 11) / 2, 2, AttackTypes.Melee));
                    c.Level = Convert.ToInt32(cr);
                    break;
            }
            return c;
        }
    }
}
