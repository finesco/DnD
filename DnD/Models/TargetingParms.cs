using System.Collections.Generic;

namespace DnD.Models
{
    public enum HealthTargets { Undamaged, Highest, Lowest, BadlyHurt, Dying, Bloodied, AboveThreshold }

    public enum SpecialParms { GWMBonus, MoveHex, PostCombatHeal, ActionSurge, ArcaneWard, MoveMark, NoSneakAttack, Ambuscade }

    public class TargetingParms
    {
        public bool HasHealthTarget { get; set; }
        public HealthTargets HealthTarget { get; set; }
        public bool HasLocationTarget { get; set; }
        public Locations LocationTarget { get; set; }
        public bool HasCondition { get; set; }
        public Conditions Condition { get; set; }
        public bool HasMissingCondition { get; set; }
        public Conditions MissingCondition { get; set; }
        public bool TargetFriendly { get; set; }
        public bool HasSpecialTargeting { get; set; }
        public List<SpecialParms> SpecialTargeting { get; set; }
        public bool TargetSelfOnly { get; set; }
        public int MinimumTargets { get; set; }
        public EncounterDifficulties DifficultyRequirement { get; set; }
        public bool HasSpecificTarget { get; set; }
        public string SpecificTarget { get; set; }
        public bool HasWeakSaveTarget { get; set; }
        public SaveTypes WeakSaveTarget { get; set; }
        public int MinimumOpponents { get; set; }
        public bool IsCC { get; set; }
        public int HealthThreshold { get; set; }

        public TargetingParms()
        {
        }

        public bool HasSpecialParm(SpecialParms parm)
        {
            if (SpecialTargeting == null)
                return false;
            int i = 0;
            bool found = false;
            while (i < SpecialTargeting.Count && !found)
            {
                if (SpecialTargeting[i] == parm)
                    found = true;
                i++;
            }
            return found;
        }

        public void AddSpecialParm(SpecialParms parm)
        {
            HasSpecialTargeting = true;
            if (SpecialTargeting == null)
                SpecialTargeting = new List<SpecialParms>();
            SpecialTargeting.Add(parm);
        }
    }
}
