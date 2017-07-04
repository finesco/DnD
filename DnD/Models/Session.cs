using System.Collections.Generic;

namespace DnD.Models
{
    public enum TargetingStyle { Focus, Spread }

    public class Session
    {
        public int Level { get; set; }
        public List<Character> PCs { get; set; }
        public bool ShortRestTaken { get; set; }
        public List<Encounter> Encounters { get; set; }
        public int CurrentEncounter { get; set; }
        public TargetingStyle PCTargetingStyle { get; set; }
        public TargetingStyle NPCTargetingStyle { get; set; }
        public int ShortRestsTaken { get; set; }
        public int Trials { get; set; }

        public Session(int level)
        {
            Encounters = new List<Encounter>();
            PCs = new List<Character>();
            Level = level;
            switch (level)
            {
                case 1:
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
                    PCs.Add(char1);
                    char1 = new Character("Nyles", true, Locations.Back, 18, 10, 2, -1, 2, 2, 4, 5, -1);
                    char1.AddSpellSlots(2, 0, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Healing Word", ActionTypes.Heal, "d4", 2, true, false, targeting, 0, RefreshTypes.Spell, 1, 1));
                    /*targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Deadly;
                    targeting.MinimumTargets = 3;
                    targeting.TargetFriendly = true;
                    char1.Actions.Add(new CombatAction("Bless", ActionTypes.Spell, "", 0, false, false, targeting, 0, RefreshTypes.Spell, 1, 3, AttackTypes.AE, SaveTypes.Con, 0, false,
                        true, Conditions.Blessed, true, true, 1, 0)); */
                    char1.Actions.Add(new CombatAction("Short Sword", 4, "d6", 2, 1, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Healing Word (Postcombat)", "d4", 2, 1, RefreshTypes.Spell, 1, HealingPriorities.Low));
                    char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Sanchez", true, Locations.Back, 14, 10, 3, -1, 5, 2, 1, 1, 3);
                    char1.SneakAttackDamage = "d6";
                    char1.Actions.Add(new CombatAction("Short sword", 5, "d6", 3, 1, AttackTypes.Melee));
                    char1.Actions.Add(new CombatAction("Dagger (off hand)", 5, "d4", 0, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
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
                    PCs.Add(char1);
                    /* char1 = new Character("Morgrave", true, Locations.Back, 14, 12, 3, 1, 3, 4, 2, -1, 0);
                    targeting = new TargetingParms();
                    targeting.TargetSelfOnly = true;
                    targeting.HealthTarget = HealthTargets.Bloodied;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Second Wind", ActionTypes.Heal, "d10", 1, true, false, targeting, 1, RefreshTypes.ShortRest, 0, 1));
                    char1.Actions.Add(new CombatAction("Heavy Crossbow", 7, "d10", 3, 1, AttackTypes.Ranged, false, false, true, true, false, new TargetingParms()));
                    char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1); */
                    //4 goblins (hard)
                    Encounter enc1 = new Encounter();
                    Character char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 3", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 4", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Hard;
                    Encounters.Add(enc1);
                    //2 goblins (easy)
                    enc1 = new Encounter();
                    char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Easy;
                    Encounters.Add(enc1);
                    //6 goblins with leader (deadly)
                    enc1 = new Encounter();
                    char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 3", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 4", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 5", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin Leader", false, Locations.Back, 15, 12, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Deadly;
                    Encounters.Add(enc1);
                    //3 wolves (medium)
                    enc1 = new Encounter();
                    char2 = new Character("Wolf 1", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wolf 2", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wolf 3", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    //klarg (bugbear) + wolf + 2 goblins (deadly)
                    enc1 = new Encounter();
                    char2 = new Character("Klarg", false, Locations.Front, 16, 27, 2, 2, 2, 1, 0, -1, -1);
                    char2.Actions.Add(new CombatAction("Morningstar", 4, "2d8", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wolf", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Deadly;
                    Encounters.Add(enc1);
                    break;
                case 3:
                    char1 = new Character("Krym", true, Locations.Front, 17, 24, -1, 5, -1, 4, 0, -1, 2);
                    targeting = new TargetingParms();
                    targeting.TargetSelfOnly = true;
                    targeting.HealthTarget = HealthTargets.Bloodied;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Second Wind", ActionTypes.Heal, "d10", 2, true, false, targeting, 1, RefreshTypes.ShortRest, 0, 1));
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.GWMBonus);
                    char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, true, false, targeting));
                    /*targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.Hexed;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveHex);
                    char1.Actions.Add(new CombatAction("Move Hex", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.Hexed, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.Hexed;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Hex", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.Spell, 1, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.Hexed, true, true, 1, 0)); */
                    /*targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    targeting.AddSpecialParm(SpecialParms.ActionSurge);
                    char1.Actions.Add(new CombatAction("Greatsword swing (Action Surge)", ActionTypes.Attack, "2d6", 3, false, false, targeting, 1, RefreshTypes.ShortRest, 0,
                        1, AttackTypes.Melee, SaveTypes.Con, 0, false, false, Conditions.Unconscious, false, false, 1, 5)); */
                    char1.Actions.Add(new CombatAction("Greatsword swing", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, false, false, new TargetingParms()));
                    //char1.Abilities.Add(new CombatAction("Dark One's Blessing", ActionTypes.Heal, "", 3, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1));
                    //char1.Abilities.Add(new CombatAction("Warlock Recovery", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1));
                    //char1.Abilities.Add(new CombatAction("Fiendish Vigor", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1));
                    char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Strength of the Grave", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Shield", ActionTypes.Spell, "", 0, false, true, targeting, 2, RefreshTypes.LongRest, 1, 1, AttackTypes.Melee, SaveTypes.Con, 
                        0, false, true, Conditions.Shielded, false, true, 1, 0));
                    char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    char1.AddSpellSlots(1, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Nyles", true, Locations.Back, 18, 22, 2, -1, 2, 2, 4, 5, -1);
                    char1.WardMaxHP = 7;
                    char1.WardHP = 7;
                    char1.AddSpellSlots(4, 2, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Healing Word", ActionTypes.Heal, "d4", 2, true, false, targeting, 0, RefreshTypes.Spell, 1, 1));
                    targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Deadly;
                    targeting.MinimumTargets = 4;
                    targeting.TargetFriendly = true;
                    char1.Actions.Add(new CombatAction("Bless", ActionTypes.Spell, "", 0, false, false, targeting, 0, RefreshTypes.Spell, 2, 4, AttackTypes.AE, SaveTypes.Con, 0, false,
                        true, Conditions.Blessed, true, true, 1, 0)); 
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Acid Splash", ActionTypes.Spell, "d6", 0, false, false, targeting, 1, RefreshTypes.AtWill, 0, 2, AttackTypes.AE, SaveTypes.Dex,
                        13, false, false, Conditions.Blinded, false, false, 1, 0));
                    char1.Actions.Add(new CombatAction("Firebolt", 5, "d10", 0, 1, AttackTypes.Ranged));
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.ArcaneWard);
                    char1.Abilities.Add(new CombatAction("Shield", ActionTypes.Spell, "", 0, false, true, targeting, 1, RefreshTypes.Spell, 1, 1, AttackTypes.Melee, SaveTypes.Con, 
                        0, false, true, Conditions.Shielded, false, true, 1, 0));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "2d8", 2, 1, RefreshTypes.Spell, 2, HealingPriorities.Medium));
                    char1.Abilities.Add(new CombatAction("Arcane Recovery", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.LongRest, 1, 1));
                    char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
/*                    char1 = new Character("Sanchez", true, Locations.Back, 15, 24, 3, -1, 5, 2, 1, 1, 3);
                    char1.SneakAttackDamage = "2d6";
                    char1.Actions.Add(new CombatAction("Short sword", 5, "d6", 3, 1, AttackTypes.Melee));
                    char1.Actions.Add(new CombatAction("Short sword (off hand)", 5, "d6", 0, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    //char1.Actions.Add(new CombatAction("Cunning Action (Hide)", ActionTypes.Spell, "", 0, true, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1));
                    char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0); */
                    char1 = new Character("Won Ton Slaughter", true, Locations.Front, 14, 18, 2, 4, 4, 0, -1, 0, 2);
                    char1.AddSpellSlots(4, 2, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Healing Word", ActionTypes.Heal, "d4", 2, true, false, targeting, 0, RefreshTypes.Spell, 1, 1));
                    //char1.Actions.Add(new CombatAction("Maul", 6, "2d6", 4, 1, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "2d8", 2, 1, RefreshTypes.Spell, 2, HealingPriorities.Medium));
                    /*targeting = new TargetingParms();
                    targeting.TargetSelfOnly = true;
                    targeting.HasMissingCondition = true;
                    targeting.MissingCondition = Conditions.Enhanced;
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    char1.Actions.Add(new CombatAction("Enhance Ability", ActionTypes.Spell, "", 0, false, false, targeting, 1, RefreshTypes.Spell, 2, 1, AttackTypes.AE, SaveTypes.Cha,
                        0, false, true, Conditions.Enhanced, true, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.HasCondition = true;
                    targeting.Condition = Conditions.Brawled;
                    char1.Actions.Add(new CombatAction("Grapple (Tavern Brawl)", ActionTypes.Attack, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Physical,
                        SaveTypes.Cha, 0, false, true, Conditions.Grappled, false, false, 1, 6)); */
                    targeting = new TargetingParms();
                    //targeting.HasCondition = true;
                    //targeting.Condition = Conditions.Grappled;
                    targeting.HasMissingCondition = true;
                    targeting.MissingCondition = Conditions.Prone;
                    char1.Actions.Add(new CombatAction("Take Down", ActionTypes.Attack, "", 0, false, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Physical, SaveTypes.Cha,
                        0, false, true, Conditions.Prone, false, false, 1, 6));
                    /*targeting = new TargetingParms();
                    targeting.HasCondition = true;
                    targeting.Condition = Conditions.Prone;
                    char1.Actions.Add(new CombatAction("Power Bomb", ActionTypes.Attack, "d4", 4, false, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Cha,
                        0, false, false, Conditions.Enhanced, false, false, 1, 6));
                    char1.Actions.Add(new CombatAction("Brawl", ActionTypes.Attack, "d4", 4, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Cha,
                        0, false, true, Conditions.Brawled, false, true, 1, 6)); */ 
                    char1.AddHitDice(8, 0, 0, 0, 0, 0, 0, 0, 0, 0); 
                    PCs.Add(char1);
                    char1 = new Character("Jennifer", true, Locations.Front, 17, 28, -1, 3, -1, 2, 1, -1, 2);
                    char1.AddSpellSlots(3, 0, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    targeting.HasSpecificTarget = true;
                    targeting.SpecificTarget = "Nyles";
                    char1.Actions.Add(new CombatAction("Lay on Hands", ActionTypes.Heal, "", 15, false, false, targeting, 1, RefreshTypes.LongRest, 0, 1));
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.GWMBonus);
                    char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, true, false, targeting));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveMark);
                    char1.Actions.Add(new CombatAction("Move Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Hunter's Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.Spell, 1, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, true, true, 1, 0)); 
                    char1.Actions.Add(new CombatAction("Greatsword swing", 5, "2d6", 3, 1, AttackTypes.Melee, true, true, false, false, false, new TargetingParms()));
                    char1.Abilities.Add(new CombatAction("Lay on Hands", "", 15, 1, RefreshTypes.LongRest, 0, HealingPriorities.High));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "1d8", 2, 1, RefreshTypes.Spell, 1, HealingPriorities.Medium));
                    char1.Abilities.Add(new CombatAction("Divine Smite", ActionTypes.Spell, "2d8", 0, false, false, new TargetingParms(), 1, RefreshTypes.Spell, 1, 1));
                    char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("DW Ranger", true, Locations.Back, 16, 28, 4, 1, 5, 2, 2, -1, 0);
                    char1.AddSpellSlots(3, 0, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.Ambuscade);
                    char1.Actions.Add(new CombatAction("Heavy Crossbow (ambuscade)", 7, "d10", 3, 1, AttackTypes.Ranged, false, false, true, false, false, targeting)); 
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveMark);
                    char1.Actions.Add(new CombatAction("Move Mark (HB)", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveMark);
                    char1.Actions.Add(new CombatAction("Move Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    char1.Actions.Add(new CombatAction("Hunter's Mark (HB)", ActionTypes.Spell, "", 0, true, false, targeting, 2, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, true, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    char1.Actions.Add(new CombatAction("Hunter's Mark", ActionTypes.Spell, "", 0, true, false, targeting, 2, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, true, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    //char1.Actions.Add(new CombatAction("Hordebreaker", ActionTypes.Attack, "d10", 3, false, false, targeting, 1, RefreshTypes.AtWill, 0, 2, AttackTypes.AE,
                    //    SaveTypes.Con, 0, false, false, Conditions.Prone, false, false, 1, 7));
                    char1.Actions.Add(new CombatAction("Heavy Crossbow", 7, "d10", 3, 1, AttackTypes.Ranged, false, false, true, false, false, new TargetingParms()));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "1d8", 2, 1, RefreshTypes.Spell, 1, HealingPriorities.Medium));
//                    char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Ambuscade", 0, "", 0, 0, AttackTypes.Melee));
                    char1.AddHitDice(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    //4 goblins (easy)
                    enc1 = new Encounter();
                    char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 3", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 4", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Easy;
                    Encounters.Add(enc1);
                    //4 hobgoblins (medium)
                    enc1 = new Encounter();
                    char2 = new Character("Hobgoblin 1", false, Locations.Front, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longsword", 3, "d8", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hobgoblin 2", false, Locations.Front, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longsword", 3, "d8", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hobgoblin 3", false, Locations.Back, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longbow", 3, "d8", 1, 1, AttackTypes.Ranged));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hobgoblin 4", false, Locations.Back, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longbow", 3, "d8", 1, 1, AttackTypes.Ranged));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    //8 goblins with leader (hard)
                    enc1 = new Encounter();
                    char2 = new Character("Goblin 1", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 2", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 3", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 4", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 5", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 6", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin 7", false, Locations.Back, 15, 7, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Shortbow", 4, "d6", 2, 1, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Goblin Leader", false, Locations.Back, 15, 12, 2, -1, 2, 0, -1, 0, -1);
                    char2.Actions.Add(new CombatAction("Scimitar", 4, "d6", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Hard;
                    Encounters.Add(enc1);
                    enc1 = new Encounter();
                    char2 = new Character("Owlbear", false, Locations.Front, 13, 59, 1, 5, 1, 3, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Beak", 7, "d10", 5, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char2.Actions.Add(new CombatAction("Claws", 7, "2d8", 5, 1, AttackTypes.Melee, false, false, false, false, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    enc1 = new Encounter();
                    char2 = new Character("Hobgoblin 1", false, Locations.Front, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longsword", 3, "d8", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hobgoblin 2", false, Locations.Front, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longsword", 3, "d8", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hobgoblin 3", false, Locations.Back, 18, 11, 1, 1, 1, 1, 0, 0, -1); 
                    char2.Actions.Add(new CombatAction("Longbow", 3, "d8", 1, 1, AttackTypes.Ranged));
                    char2.Abilities.Add(new CombatAction("Martial Advantage", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wolf 1", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wolf 2", false, Locations.Front, 13, 11, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    enc1 = new Encounter();
                    char2 = new Character("King Grol", false, Locations.Front, 16, 45, 2, 2, 2, 1, 0, -1, -1);
                    char2.Actions.Add(new CombatAction("Morningstar", 4, "2d8", 2, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Snarl", false, Locations.Front, 13, 18, 2, 1, 2, 1, 1, -4, -2);
                    char2.Actions.Add(new CombatAction("Bite", ActionTypes.Attack, "2d4", 2, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Str, 11, false, true, Conditions.Prone, false, false, 1, 4));
                    char2.Abilities.Add(new CombatAction("Pack Tactics", 0, "", 0, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Doppleganger", false, Locations.Front, 14, 52, 4, 0, 4, 2, 1, 0, 2);
                    char2.Actions.Add(new CombatAction("Slam", 6, "d6", 4, 2, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Deadly;
                    Encounters.Add(enc1);
                    break;
                case 8:
                    char1 = new Character("Ozy", true, Locations.Front, 19, 59, -1, 10, 2, 8, 3, 2, 6);
                    targeting = new TargetingParms();
                    targeting.TargetSelfOnly = true;
                    targeting.HealthTarget = HealthTargets.Bloodied;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Second Wind", ActionTypes.Heal, "d10", 1, true, false, targeting, 1, RefreshTypes.ShortRest, 0, 1));
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.GWMBonus);
                    char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 8, "2d6", 5, 1, AttackTypes.Melee, true, true, false, true, false, targeting));
                    /* targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    targeting.AddSpecialParm(SpecialParms.ActionSurge);
                    char1.Actions.Add(new CombatAction("Greatsword+1 swing (Action Surge)", ActionTypes.Attack, "2d6", 5, false, false, targeting, 1, RefreshTypes.ShortRest, 0,
                        1, AttackTypes.Melee, SaveTypes.Con, 0, false, false, Conditions.Unconscious, false, false, 2, 8)); */
                    char1.Actions.Add(new CombatAction("Greatsword+1 swing", 8, "2d6", 5, 2, AttackTypes.Melee, true, true, false, false, false, new TargetingParms()));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.Hexed;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveHex);
                    char1.Actions.Add(new CombatAction("Move Hex", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.Hexed, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.Hexed;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Hex", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.Spell, 3, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.Hexed, true, true, 1, 0));
                    char1.Abilities.Add(new CombatAction("Dark One's Blessing", ActionTypes.Heal, "", 8, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1));
                    char1.Abilities.Add(new CombatAction("Warlock Recovery", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1));
                    char1.Abilities.Add(new CombatAction("Fiendish Vigor", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1));
                    //char1.Abilities.Add(new CombatAction("Precision Attack", ActionTypes.Spell, "d8", 0, false, false, new TargetingParms(), 4, RefreshTypes.ShortRest, 0, 0));
                    char1.Abilities.Add(new CombatAction("Strength of the Grave", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Shield", ActionTypes.Spell, "", 0, false, true, targeting, 3, RefreshTypes.LongRest, 1, 1, AttackTypes.Melee, SaveTypes.Con, 
                        0, false, true, Conditions.Shielded, false, true, 1, 0));
                    char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.AddHitDice(10, 8, 8, 8, 0, 0, 0, 0, 0, 0);
                    char1.AddSpellSlots(0, 0, 2, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Nyles", true, Locations.Back, 19, 52, 2, 2, 5, 5, 8, 7, 5);
                    char1.WardMaxHP = 18;
                    char1.WardHP = 18;
                    char1.AddSpellSlots(4, 3, 3, 2, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    char1.Actions.Add(new CombatAction("Healing Word", ActionTypes.Heal, "d4", 2, true, false, targeting, 0, RefreshTypes.Spell, 1, 1));
                    targeting = new TargetingParms();
                    targeting.HasHealthTarget = true;
                    targeting.HealthTarget = HealthTargets.Undamaged;
                    targeting.IsCC = true;
                    targeting.MinimumOpponents = 2;
                    targeting.HasWeakSaveTarget = true;
                    targeting.WeakSaveTarget = SaveTypes.Cha;
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    char1.Actions.Add(new CombatAction("Banish", ActionTypes.Spell, "", 0, false, false, targeting, 0, RefreshTypes.Spell, 4, 1, AttackTypes.AE, SaveTypes.Cha,
                        15, false, true, Conditions.Banished, true, false, 0, 0)); 
                    targeting = new TargetingParms();
                    targeting.HasHealthTarget = true;
                    targeting.HealthTarget = HealthTargets.Undamaged;
                    targeting.IsCC = true;
                    targeting.MinimumOpponents = 2;
                    targeting.HasWeakSaveTarget = true;
                    targeting.WeakSaveTarget = SaveTypes.Wis;
                    targeting.DifficultyRequirement = EncounterDifficulties.Medium;
                    char1.Actions.Add(new CombatAction("Tasha's Hideous Laughter", ActionTypes.Spell, "", 0, false, false, targeting, 0, RefreshTypes.Spell, 1, 1, AttackTypes.AE, SaveTypes.Wis,
                        15, false, true, Conditions.TashasHideousLaughter, true, false, 0, 0));
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 4;
                    targeting.DifficultyRequirement = EncounterDifficulties.Medium;
                    targeting.TargetFriendly = true;
                    char1.Actions.Add(new CombatAction("Bless", ActionTypes.Spell, "", 0, false, false, targeting, 0, RefreshTypes.Spell, 2, 4, AttackTypes.AE, SaveTypes.Con, 0, false,
                        true, Conditions.Blessed, true, true, 1, 0)); 
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Acid Splash", ActionTypes.Spell, "2d6", 0, false, false, targeting, 1, RefreshTypes.AtWill, 0, 2, AttackTypes.AE, SaveTypes.Dex,
                        15, false, false, Conditions.Blinded, false, false, 1, 0));
                    char1.Actions.Add(new CombatAction("Firebolt", 7, "2d10", 0, 1, AttackTypes.Ranged));
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.ArcaneWard);
                    char1.Abilities.Add(new CombatAction("Shield", ActionTypes.Spell, "", 0, false, true, targeting, 1, RefreshTypes.Spell, 1, 1, AttackTypes.Melee, SaveTypes.Con, 
                        0, false, true, Conditions.Shielded, false, true, 1, 0));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "3d8", 2, 1, RefreshTypes.Spell, 3, HealingPriorities.Medium));
                    char1.Abilities.Add(new CombatAction("Arcane Recovery", ActionTypes.Spell, "", 0, false, false, new TargetingParms(), 1, RefreshTypes.LongRest, 4, 1));
                    char1.AddHitDice(8, 6, 6, 6, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Jesse", true, Locations.Front, 18, 62, -1, 2, -1, 5, 4, 1, 6);
                    targeting = new TargetingParms();
                    targeting.TargetSelfOnly = true;
                    targeting.DifficultyRequirement = EncounterDifficulties.Deadly;
                    char1.Actions.Add(new CombatAction("Greater Invisibility", ActionTypes.Spell, "", 0, false, false, targeting, 1, RefreshTypes.Spell, 4, 1, AttackTypes.AE,
                        SaveTypes.Cha, 0, false, true, Conditions.Invisible, true, true, 0, 0));
                    //char1.Actions.Add(new CombatAction("Long Sword", 6, "d8", 3, 2, AttackTypes.Melee));
                    char1.Actions.Add(new CombatAction("Maul", 7, "2d6", 4, 2, AttackTypes.Melee, true, false, false, false, false, new TargetingParms()));
                    targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    targeting.TargetFriendly = true;
                    targeting.HasMissingCondition = true;
                    targeting.MissingCondition = Conditions.CombatInspired;
                    char1.Actions.Add(new CombatAction("Bardic Inspiration", ActionTypes.Spell, "d8", 0, true, false, targeting, 3, RefreshTypes.ShortRest, 0, 1, AttackTypes.AE,
                        SaveTypes.Wis, 0, false, true, Conditions.CombatInspired, false, true, 0, 0));
                    char1.Abilities.Add(new CombatAction("Song of Rest", 0, "", 0, 0, AttackTypes.AE));
                    char1.Abilities.Add(new CombatAction("Lay on Hands", "", 10, 1, RefreshTypes.LongRest, 0, HealingPriorities.High));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "3d8", 3, 1, RefreshTypes.Spell, 3, HealingPriorities.Medium));
                    targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Hard;
                    char1.Abilities.Add(new CombatAction("Divine Smite", ActionTypes.Spell, "4d8", 0, false, false, targeting, 1, RefreshTypes.Spell, 3, 1));
                    targeting = new TargetingParms();
                    targeting.DifficultyRequirement = EncounterDifficulties.Medium;
                    char1.Abilities.Add(new CombatAction("Divine Smite", ActionTypes.Spell, "3d8", 0, false, false, targeting, 1, RefreshTypes.Spell, 2, 1));
                    char1.Abilities.Add(new CombatAction("Divine Smite", ActionTypes.Spell, "2d8", 0, false, false, new TargetingParms(), 1, RefreshTypes.Spell, 1, 1));
                    char1.AddSpellSlots(4, 3, 3, 1, 0, 0, 0, 0, 0);
                    char1.AddHitDice(10, 10, 8, 8, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Joselyn", true, Locations.Front, 22, 68, -1, 8, 2, 5, 6, 3, 8);
                    char1.AddSpellSlots(4, 3, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.TargetFriendly = true;
                    targeting.HealthTarget = HealthTargets.Dying;
                    targeting.HasHealthTarget = true;
                    targeting.HasSpecificTarget = true;
                    targeting.SpecificTarget = "Nyles";
                    char1.Actions.Add(new CombatAction("Lay on Hands", ActionTypes.Heal, "", 40, false, false, targeting, 1, RefreshTypes.LongRest, 0, 1));
                    /*targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.GWMBonus);
                    char1.Actions.Add(new CombatAction("Greatsword swing (GWM Bonus)", 9, "2d6", 6, 1, AttackTypes.Melee, true, true, false, true, false, targeting)); */
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveMark);
                    char1.Actions.Add(new CombatAction("Move Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasLocationTarget = true;
                    targeting.LocationTarget = Locations.Front;
                    char1.Actions.Add(new CombatAction("Hunter's Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.Spell, 1, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, true, true, 1, 0)); 
                    char1.Actions.Add(new CombatAction("Warhammer", 9, "d8", 6, 2, AttackTypes.Melee, false, false, false, false, false, new TargetingParms()));
                    char1.Abilities.Add(new CombatAction("Lay on Hands", "", 40, 1, RefreshTypes.LongRest, 0, HealingPriorities.High));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "2d8", 3, 1, RefreshTypes.Spell, 2, HealingPriorities.Medium));
                    //char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Divine Smite", ActionTypes.Spell, "3d8", 0, false, false, new TargetingParms(), 1, RefreshTypes.Spell, 2, 1));
                    char1.AddHitDice(10, 10, 10, 10, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    char1 = new Character("Gavin", true, Locations.Back, 18, 68, 5, 2, 8, 2, 3, -1, 0);
                    char1.AddSpellSlots(4, 3, 0, 0, 0, 0, 0, 0, 0);
                    targeting = new TargetingParms();
                    targeting.AddSpecialParm(SpecialParms.Ambuscade);
                    char1.Actions.Add(new CombatAction("Longbow (ambuscade)", 11, "d8", 6, 1, AttackTypes.Ranged, false, false, true, false, false, targeting));  
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    targeting.HasSpecialTargeting = true;
                    targeting.AddSpecialParm(SpecialParms.MoveMark);
                    char1.Actions.Add(new CombatAction("Move Mark", ActionTypes.Spell, "", 0, true, false, targeting, 1, RefreshTypes.AtWill, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, false, true, 1, 0));
                    targeting = new TargetingParms();
                    targeting.MissingCondition = Conditions.HunterMarked;
                    targeting.HasMissingCondition = true;
                    char1.Actions.Add(new CombatAction("Hunter's Mark", ActionTypes.Spell, "", 0, true, false, targeting, 2, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee, SaveTypes.Con, 0, false, true,
                        Conditions.HunterMarked, true, true, 1, 0));
                    char1.Actions.Add(new CombatAction("Longbow", 11, "d8", 6, 2, AttackTypes.Ranged, false, false, true, false, false, new TargetingParms()));
                    char1.Abilities.Add(new CombatAction("Cure Wounds (Postcombat)", "1d8", 2, 1, RefreshTypes.Spell, 1, HealingPriorities.Medium));
//                    char1.Abilities.Add(new CombatAction("Dorn", 0, "", 0, 0, AttackTypes.Melee));
                    char1.Abilities.Add(new CombatAction("Ambuscade", 0, "", 0, 0, AttackTypes.Melee));
                    char1.AddHitDice(10, 10, 10, 10, 0, 0, 0, 0, 0, 0);
                    PCs.Add(char1);
                    //12 magma mephits (med)
                    enc1 = new Encounter();
                    /*char2 = new Character("Magma Mephit 1", false, Locations.Front, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 2", false, Locations.Front, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 3", false, Locations.Front, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 4", false, Locations.Front, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 5", false, Locations.Front, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 6", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 7", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 8", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 9", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 10", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 11", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Magma Mephit 12", false, Locations.Back, 11, 22, 1, -1, 1, 1, 0, -2, -0);
                    char2.Actions.Add(new CombatAction("Claws", 3, "2d4", 1, 1, AttackTypes.Melee));
                    char2.Abilities.Add(new CombatAction("Death Burst", ActionTypes.Spell, "2d6", 0, false, false, new TargetingParms(), 1, RefreshTypes.AtWill, 0, 1,
                        AttackTypes.Melee, SaveTypes.Dex, 11, true, false, Conditions.HunterMarked, false, false, 1, 0));
                    enc1.Opponents.Add(char2); */
                    char2 = new Character("Manticore 1", false, Locations.Front, 14, 68, 3, 3, 3, 3, 1, -2, -1);
                    char2.Actions.Add(new CombatAction("Tail Spike", 5, "d8", 3, 3, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Manticore 2", false, Locations.Front, 14, 68, 3, 3, 3, 3, 1, -2, -1);
                    char2.Actions.Add(new CombatAction("Tail Spike", 5, "d8", 3, 3, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Manticore 3", false, Locations.Back, 14, 68, 3, 3, 3, 3, 1, -2, -1);
                    char2.Actions.Add(new CombatAction("Tail Spike", 5, "d8", 3, 3, AttackTypes.Ranged));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    //2 gargoyle (easy)
                    enc1 = new Encounter();
                    char2 = new Character("Gargoyle 1", false, Locations.Front, 15, 52, 0, 2, 1, 3, 0, -2, -2);
                    char2.Actions.Add(new CombatAction("Bite", 4, "d6", 2, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Claws", 4, "d6", 2, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Gargoyle 1", false, Locations.Front, 15, 52, 0, 2, 1, 3, 0, -2, -2);
                    char2.Actions.Add(new CombatAction("Bite", 4, "d6", 2, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Claws", 4, "d6", 2, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Easy;
                    Encounters.Add(enc1);
                    //4 blue dragon wyrmlings (hard)
                    enc1 = new Encounter();
                    char2 = new Character("Blue Dragon Wyrmling 1", false, Locations.Front, 17, 52, 0, 3, 2, 4, 2, 1, 4);
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    char2.Actions.Add(new CombatAction("Lightning Breath", ActionTypes.Spell, "4d10", 0, false, false, targeting, 1, RefreshTypes.Recharge, 0, 2, AttackTypes.AE,
                        SaveTypes.Dex, 12, true, false, Conditions.HunterMarked, false, false, 0, 0, 5));
                    char2.Actions.Add(new CombatAction("Bite", 5, "2d8", 3, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Blue Dragon Wyrmling 2", false, Locations.Front, 17, 52, 0, 3, 2, 4, 2, 1, 4);
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    char2.Actions.Add(new CombatAction("Lightning Breath", ActionTypes.Spell, "4d10", 0, false, false, targeting, 1, RefreshTypes.Recharge, 0, 2, AttackTypes.AE,
                        SaveTypes.Dex, 12, true, false, Conditions.HunterMarked, false, false, 0, 0, 5));
                    char2.Actions.Add(new CombatAction("Bite", 5, "2d8", 3, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Blue Dragon Wyrmling 3", false, Locations.Back, 17, 52, 0, 3, 2, 4, 2, 1, 4);
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    char2.Actions.Add(new CombatAction("Lightning Breath", ActionTypes.Spell, "4d10", 0, false, false, targeting, 1, RefreshTypes.Recharge, 0, 2, AttackTypes.AE,
                        SaveTypes.Dex, 12, true, false, Conditions.HunterMarked, false, false, 0, 0, 5));
                    char2.Actions.Add(new CombatAction("Bite", 5, "2d8", 3, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Blue Dragon Wyrmling 4", false, Locations.Back, 17, 52, 0, 3, 2, 4, 2, 1, 4);
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    char2.Actions.Add(new CombatAction("Lightning Breath", ActionTypes.Spell, "4d10", 0, false, false, targeting, 1, RefreshTypes.Recharge, 0, 2, AttackTypes.AE,
                        SaveTypes.Dex, 12, true, false, Conditions.HunterMarked, false, false, 0, 0, 5));
                    char2.Actions.Add(new CombatAction("Bite", 5, "2d8", 3, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Blue Dragon Wyrmling 5", false, Locations.Back, 17, 52, 0, 3, 2, 4, 2, 1, 4);
                    targeting = new TargetingParms();
                    targeting.MinimumTargets = 2;
                    char2.Actions.Add(new CombatAction("Lightning Breath", ActionTypes.Spell, "4d10", 0, false, false, targeting, 1, RefreshTypes.Recharge, 0, 2, AttackTypes.AE,
                        SaveTypes.Dex, 12, true, false, Conditions.HunterMarked, false, false, 0, 0, 5));
                    char2.Actions.Add(new CombatAction("Bite", 5, "2d8", 3, 1, AttackTypes.Melee));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Hard;
                    Encounters.Add(enc1);
                    //7 hippogriffs (med)
                    enc1 = new Encounter();
                    char2 = new Character("Hippogriff 1", false, Locations.Front, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 2", false, Locations.Front, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 3", false, Locations.Front, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 4", false, Locations.Back, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 5", false, Locations.Back, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 6", false, Locations.Back, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Hippogriff 7", false, Locations.Back, 11, 19, 1, 3, 1, 1, 1, -4, -1);
                    char2.Actions.Add(new CombatAction("Claws", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Beak", 5, "d10", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Medium;
                    Encounters.Add(enc1);
                    //3 wereboars (deadly)
                    enc1 = new Encounter();
                    char2 = new Character("Wereboar 1", false, Locations.Front, 11, 78, 0, 3, 0, 2, 0, 0, -1);
                    char2.Actions.Add(new CombatAction("Charge", ActionTypes.Attack, "4d6", 3, true, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee,
                        SaveTypes.Str, 13, false, true, Conditions.Prone, false, false, 1, 5));
                    char2.Actions.Add(new CombatAction("Maul", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Tusks", 5, "2d6", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char2.Abilities.Add(new CombatAction("Relentless", "", 14, 1, RefreshTypes.ShortRest, 0, HealingPriorities.High));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wereboar 2", false, Locations.Front, 11, 78, 0, 3, 0, 2, 0, 0, -1);
                    char2.Actions.Add(new CombatAction("Charge", ActionTypes.Attack, "4d6", 3, true, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee,
                        SaveTypes.Str, 13, false, true, Conditions.Prone, false, false, 1, 5));
                    char2.Actions.Add(new CombatAction("Maul", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Tusks", 5, "2d6", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char2.Abilities.Add(new CombatAction("Relentless", "", 14, 1, RefreshTypes.ShortRest, 0, HealingPriorities.High));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wereboar 3", false, Locations.Front, 11, 78, 0, 3, 0, 2, 0, 0, -1);
                    char2.Actions.Add(new CombatAction("Charge", ActionTypes.Attack, "4d6", 3, true, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee,
                        SaveTypes.Str, 13, false, true, Conditions.Prone, false, false, 1, 5));
                    char2.Actions.Add(new CombatAction("Maul", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Tusks", 5, "2d6", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char2.Abilities.Add(new CombatAction("Relentless", "", 14, 1, RefreshTypes.ShortRest, 0, HealingPriorities.High));
                    enc1.Opponents.Add(char2);
                    char2 = new Character("Wereboar 4", false, Locations.Back, 11, 78, 0, 3, 0, 2, 0, 0, -1);
                    char2.Actions.Add(new CombatAction("Charge", ActionTypes.Attack, "4d6", 3, true, false, new TargetingParms(), 1, RefreshTypes.ShortRest, 0, 1, AttackTypes.Melee,
                        SaveTypes.Str, 13, false, true, Conditions.Prone, false, false, 1, 5));
                    char2.Actions.Add(new CombatAction("Maul", 5, "2d6", 3, 1, AttackTypes.Melee));
                    char2.Actions.Add(new CombatAction("Tusks", 5, "2d6", 3, 1, AttackTypes.Melee, false, false, false, true, false, new TargetingParms()));
                    char2.Abilities.Add(new CombatAction("Relentless", "", 14, 1, RefreshTypes.ShortRest, 0, HealingPriorities.High));
                    enc1.Opponents.Add(char2);
                    enc1.Difficulty = EncounterDifficulties.Deadly;
                    Encounters.Add(enc1);
                    break;
            }
        }

        public void Reset()
        {
            ShortRestTaken = false;
            foreach (var c in PCs)
                c.Reset();
            foreach (var e in Encounters)
                foreach (var c in e.Opponents)
                    c.Reset();
        }
    }
}
