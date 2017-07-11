using DnD.Data;
using DnD.Helpers;
using DnD.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnD.Controllers
{
    public class CombatController : Controller
    {
        private readonly IDiceRoller _roll;
        private readonly ILog _log;
        private readonly ICombatRepository _repo;
        private int ggLeft;
        private int bgLeft;
        private Session session;

        public CombatController(IDiceRoller roller, ILog logger, ICombatRepository repo)
        {
            _roll = roller;
            _log = logger;
            _repo = repo;
        }

        [Route("api/Combat/{sessionId}")]
        public IActionResult RunSimulation(int sessionId)
        {
            _log.Info("beginning sim");
            session = _repo.LoadSession(2);
            _log.Debug("test:" + sessionId);
            for (int j = 0; j < session.Trials; j++)
            {
                for (int i = 0; i < session.Encounters.Count; i++)
                {
                    ggLeft = session.PCs.Where(c => c.IsAlive()).Count();
                    if (ggLeft > 0)
                    {
                        session.CurrentEncounter = i;
                        simEncounter();
                        //if (ggLeft > 0)
                        //{
                        //    if (shortRestCheck())
                        //        shortRest();
                        //    postCombatHealing();
                        //}
                    }
                }
                _log.Debug("Resetting session " + j);
                session.Reset();
            }
            fileReport();
            _log.Info("ending sim");
            return Ok();
        }

        private void simEncounter()
        {
            //get count of each side -- if either side has no people, return
            //for each person, prep for combat, roll initiative
            //begin loop

            _log.Debug("=========================================");
            _log.Debug($"Beginning combat: {session.Encounters[session.CurrentEncounter]}");
            Encounter enc = session.Encounters[session.CurrentEncounter];
            List<Character> initOrder;
            initOrder = getInitiative(session.PCs, enc.Opponents);
            enc.Round = 1;
            enc.TimesRun++;
            ggLeft = session.PCs.Where(p => p.CurrentHP > 0).Count();
            enc.PCStartCount += ggLeft;
            bgLeft = enc.Opponents.Count;
            while (ggLeft > 0 && bgLeft > 0)
            {
                _log.Debug("Round " + enc.Round);
                int i = 0;
                while (i < initOrder.Count && ggLeft > 0 && bgLeft > 0)
                {
                    Character c = initOrder[i];
                    if (enc.Round > 0)
                    {
                        c.Statistics.TurnsTaken++;
                        if (c.IsPC)
                            enc.PCTurnsTaken++;
                        else
                            enc.NPCTurnsTaken++;
                        foreach (var ch in session.PCs)
                            if (ch.Effects != null)
                                foreach (var effect in ch.Effects.Where(e => e.Source == c && e.ExpiryType == ExpiryTypes.BeginningSourceTurn))
                                {
                                    _log.Debug("Expiring effect " + effect.Name + " from " + ch.Name);
                                    ch.Effects.Remove(effect.Name);
                                }
                        foreach (var ch in session.Encounters[session.CurrentEncounter].Opponents)
                            if (ch.Effects != null)
                                foreach (var effect in ch.Effects.Where(e => e.Source == c && e.ExpiryType == ExpiryTypes.BeginningSourceTurn))
                                {
                                    _log.Debug("Expiring effect " + effect.Name + " from " + ch.Name);
                                    ch.Effects.Remove(effect.Name);
                                }
                    }
                    c.HasAction = true;
                    c.HasBonusAction = true;
                    c.HasReaction = true;
                    c.HasCritThisRound = false;
                    c.HasKillThisRound = false;
                    c.UsedSneakAttack = false;
                    _log.Debug(c.Name + "'s turn");
                    if (c.HasCondition(Conditions.Dying))
                    {
                        _log.Debug("Death saving throw for " + c.Name);
                        int roll = _roll.Roll20();
                        bool isCrit = roll == 20;
                        if (!isCrit && roll > 1)
                            if (c.HasCondition(Conditions.Blessed))
                                roll += _roll.RollDice("d4", false);
                        if (roll > 9)
                        {
                            if (isCrit)
                            {
                                _log.Debug("Critical success -- " + c.Name + " regains 1 HP!");
                                c.RemoveCondition(Conditions.Dying);
                                ggLeft++;
                                c.DeathSaveSuccesses = 0;
                                c.DeathSaveFailures = 0;
                                c.CurrentHP = 1;
                            }
                            else
                            {
                                _log.Debug("Success #" + (c.DeathSaveSuccesses + 1));
                                if (c.DeathSaveSuccesses > 1)
                                {
                                    _log.Debug("Stabilized");
                                    c.RemoveCondition(Conditions.Dying);
                                    c.AddCondition(Conditions.Unconscious);
                                    c.DeathSaveSuccesses = 0;
                                    c.DeathSaveFailures = 0;
                                }
                                else
                                    c.DeathSaveSuccesses++;
                            }
                        }
                        else
                        {
                            int failures = 1;
                            if (roll == 1)
                            {
                                _log.Debug("Critical failure (counts as 2 failures)!");
                                failures = 2;
                            }
                            _log.Debug("Failure #" + (c.DeathSaveFailures + failures));
                            if (c.DeathSaveFailures + failures > 2)
                            {
                                _log.Debug(c.Name + " is dead :(");
                                c.RemoveCondition(Conditions.Dying);
                                c.AddCondition(Conditions.Dead);
                                c.Statistics.TimesKilled++;
                                enc.PCDeaths++;
                                c.DeathSaveSuccesses = 0;
                                c.DeathSaveFailures = 0;
                            }
                            else
                                c.DeathSaveFailures += failures;
                        }
                    }
                    else if (c.CanAct())
                        takeAction(c);
                    if (c.Effects != null)
                        foreach(var e in c.Effects.Where(ef => ef.ExpiryType == ExpiryTypes.Save))
                        {
                            _log.Debug("Attempting to save off " + e.Name);
                            if (makeSave(c, e.SaveType, e.SaveDC))
                                c.RemoveCondition(e.Name);
                        }
                    i++;
                }
                enc.Round++;
                _log.Debug("--------------------------------"); //end of round
            }
            enc.Rounds += enc.Round;
            enc.PCEndCount += ggLeft;
            if (ggLeft == 0)
                enc.TPKs++;

        }

        private Tuple<CombatAction, List<Character>> selectActionAndTargets(Character c)
        {
            Tuple<CombatAction, List<Character>> selection = new Tuple<CombatAction, List<Character>>(null, null);
            //examine priority list of actions to find first one that meets criteria
            foreach (var action in c.Actions.Where(a => a.IsAvailable(c.RemainingSpellSlots) && ((a.IsBonusAction && c.HasBonusAction) || (!a.IsBonusAction && c.HasAction))))
            {
                _log.Debug("Considering action " + action.Name);
                if (action.Targeting.DifficultyRequirement > session.Encounters[session.CurrentEncounter].Difficulty)
                    continue;
                if (action.Targeting.HasSpecialParm(SpecialParms.NoSneakAttack) && !c.UsedSneakAttack)
                    continue;
                if (action.RequiresConcentration && c.IsConcentrating) //does not allow that anyone would voluntarily stop concentrating on one thing to use another
                    continue;
                //select target(s)
                List<Character> targetList = new List<Character>();
                if (action.Targeting.TargetSelfOnly)
                {
                    targetList.Add(c);
                }
                else if ((c.IsPC && !action.Targeting.TargetFriendly) || (!c.IsPC && action.Targeting.TargetFriendly))
                    foreach(var chr in session.Encounters[session.CurrentEncounter].Opponents)
                        targetList.Add(chr);
                else
                    foreach (var chr in session.PCs)
                        targetList.Add(chr);
                //if (grappled && action.ActionType == ActionTypes.Attack && action.AttackType == AttackTypes.Melee)
                //    targetList = grappler;
                targetList.RemoveAll(t => !t.IsTargetable());
                if (action.ActionType == ActionTypes.Attack && action.AttackType == AttackTypes.Melee && !c.CanMove())
                    if (c.Location == Locations.Back)
                        continue;
                    else
                    {
                        targetList.RemoveAll(t => t.Location != Locations.Front);
                        if (targetList.Count == 0)
                            continue;
                    }
                if (action.Targeting.HasSpecificTarget)
                {
                    targetList.RemoveAll(t => t.Name != action.Targeting.SpecificTarget);
                    if (targetList.Count == 0)
                        continue;
                }
                if (action.Targeting.HasSpecialTargeting)
                {
                    if (action.Targeting.HasSpecialParm(SpecialParms.GWMBonus))
                        if (!c.HasKillThisRound && !c.HasCritThisRound)
                            continue;
                    if (action.Targeting.HasSpecialParm(SpecialParms.MoveHex) || action.Targeting.HasSpecialParm(SpecialParms.MoveMark))
                    {
                        //a clause to allow moving the hex in round 1 of a combat if still concentrating from previous combat
                        //this would not work if actions allowed on round 0
                        bool qual = (session.Encounters[session.CurrentEncounter].Round == 1 && c.IsConcentrating);
                        int ix = 0;
                        while (!qual && ix < session.Encounters[session.CurrentEncounter].Opponents.Count)
                        {
                            if (session.Encounters[session.CurrentEncounter].Opponents[ix].HasCondition(Conditions.Dead) && session.Encounters[session.CurrentEncounter].Opponents[ix].IsCursedByAttacker(c))
                                qual = true;
                            else
                                ix++;
                        }
                        //if at this point we find that the action will work, we assume that it will be used, and so we remove hex from the previous target
                        if (!qual)
                            continue;
                        else
                        {
                            if (session.Encounters[session.CurrentEncounter].Round == 1)
                                _log.Debug("Moving hex/mark from previous combat");
                            else
                            {
                                _log.Debug("Removing hex/mark from " + targetList[ix].Name);
                                if (action.Targeting.HasSpecialParm(SpecialParms.MoveMark))
                                    session.Encounters[session.CurrentEncounter].Opponents[ix].RemoveConditionBySource(Conditions.HunterMarked, c);
                                else //assume it is hex
                                    session.Encounters[session.CurrentEncounter].Opponents[ix].RemoveConditionBySource(Conditions.Hexed, c);
                            }
                        }
                    }
                }
                if (action.Targeting.HasCondition)
                {
                    targetList.RemoveAll(t => !t.HasCondition(action.Targeting.Condition));
                    if (targetList.Count == 0)
                        continue;
                }
                if (action.Targeting.HasMissingCondition)
                {
                    targetList.RemoveAll(t => t.HasCondition(action.Targeting.MissingCondition));
                    if (targetList.Count == 0)
                        continue;
                }
                if (action.Targeting.HasWeakSaveTarget)
                {
                    targetList.RemoveAll(t => t.GetSave(action.Targeting.WeakSaveTarget) > c.Level / 4);
                    if (targetList.Count == 0)
                        continue;
                }
                if (action.Targeting.IsCC)
                {
                    targetList.RemoveAll(t => !t.CCTarget);
                    if (targetList.Count == 0)
                        continue;
                }
                if (action.Targeting.HasLocationTarget)
                    if (!(action.Targeting.LocationTarget == Locations.Front && action.AttackType == AttackTypes.Melee && !targetList.Where(t => t.Location == Locations.Front && t.IsAlive()).Any()))
                    {
                        targetList.RemoveAll(t => t.Location != action.Targeting.LocationTarget);
                        if (targetList.Count == 0)
                            continue;
                    }
                if (action.Targeting.HasHealthTarget)
                {
                    switch (action.Targeting.HealthTarget)
                    {
                        case HealthTargets.Undamaged:
                            targetList.RemoveAll(t => t.MaxHP != t.CurrentHP);
                            break;
                        case HealthTargets.Dying:
                            targetList.RemoveAll(t => !t.HasCondition(Conditions.Dying));
                            break;
                        case HealthTargets.BadlyHurt:
                            targetList.RemoveAll(t => t.CurrentHP > t.MaxHP / 4);
                            break;
                        case HealthTargets.Bloodied:
                            targetList.RemoveAll(t => t.CurrentHP > t.MaxHP / 2);
                            break;
                        case HealthTargets.AboveThreshold:
                            targetList.RemoveAll(t => t.CurrentHP < action.Targeting.HealthThreshold);
                            break;
                        default:
                            targetList.RemoveAll(t => !t.IsAlive());
                            break;
                    }
                    if (targetList.Count == 0)
                        continue;
                }
                else //if there are no specific instructions otherwise, we only want to target living opponents
                {
                    targetList.RemoveAll(t => !t.IsAlive());
                    if (targetList.Count == 0)
                        continue;
                }
                if (targetList.Where(t => t.CanAct()).Any()) //only target active opponents, unless the only ones left are CCd
                {
                    targetList.RemoveAll(t => !t.CanAct());
                }
                if (targetList.Count >= action.Targeting.MinimumTargets)
                    return new Tuple<CombatAction, List<Character>>(action, targetList); //legal action found -- return it
            }
            //if we've gotten to here, we couldn't find a legal action to take
            return selection;
        }

        private void takeAction(Character c)
        {
            bool finished = false;
            c.Statistics.TurnsActive++;
            c.HasActedThisEncounter = true;
            foreach (var a in c.Actions.Where(a => a.RefreshType == RefreshTypes.Recharge && a.RemainingUses == 0))
            {
                _log.Debug("Trying to recharge " + a.Name);
                if (_roll.RollDice("d6", false) >= a.RechargeNumber)
                    a.RemainingUses++;
            }
            if (c.HasCondition(Conditions.Prone))
            {
                _log.Debug("Standing up");
                c.RemoveCondition(Conditions.Prone);
            }
            while (!finished)             //while loop that proceeds until the player has exhausted all actions or until no choices available for situation
            {
                bool actionSelected = false;
                //bool grappled = false;
                //List<Character> grappler = new List<Character>();
                //if (c.Effects != null)
                //{
                //    int idx = 0;
                //    while (!grappled && idx < c.Effects.Count)
                //    {
                //        if (c.Effects[idx].Name == Conditions.Grappled)
                //        {
                //            grappled = true;
                //            grappler.Add(c.Effects[idx].Source);
                //        }
                //        idx++;
                //    }
                //}
                while (!actionSelected) //while loop for selecting a single action from the list
                {
                    Tuple<CombatAction, List<Character>> selection = selectActionAndTargets(c);
                    CombatAction action = selection.Item1;
                    if (action == null)
                    {
                        _log.Debug("No legal actions found -- ending turn");
                        finished = true;
                        break;
                    }
                    List<Character> targetList = selection.Item2;
                    actionSelected = true;
                    if (action.IsBonusAction)
                        c.HasBonusAction = false;
                    else if (!action.Targeting.HasSpecialParm(SpecialParms.ActionSurge)) //this should be redesigned such that a.s. is a standalone ability that restores an action
                        c.HasAction = false;
                    if (action.RefreshType == RefreshTypes.LongRest || action.RefreshType == RefreshTypes.ShortRest || action.RefreshType == RefreshTypes.Recharge)
                    {
                        action.RemainingUses--;
                        _log.Debug("Burning use of " + action.Name + " (" + action.RemainingUses + " remaining)");
                    }
                    else if (action.RefreshType == RefreshTypes.Spell)
                    {
                        c.RemainingSpellSlots[action.SpellLevel - 1]--;
                        _log.Debug("Casting " + action.Name + " (" + c.RemainingSpellSlots[action.SpellLevel - 1] + " remaining level " + action.SpellLevel + " slots)");
                    }
                    action.TimesUsed++;
                    int remainingAttacks = action.NumAttacks;
                    if (action.ActionType != ActionTypes.Attack) //assumes only attacks can have mulitple attacks
                        remainingAttacks = 1;
                    else if (session.Encounters[session.CurrentEncounter].Round == 1 && c.HasAbility("UnderdarkScout"))
                        remainingAttacks++;
                    while (remainingAttacks > 0)
                    {
                        if (action.ActionType == ActionTypes.Attack && action.AttackType == AttackTypes.Melee)
                        {
                            //melee attacks must hit front row targets if there are any
                            //they also move the attacker to the front row
                            //if the attacker or the defender is in the back row, the attacker must be able to move
                            if (c.Location == Locations.Back)
                            {
                                //we should never get to this point if attacker can't move as selection would've been illegal
                                _log.Debug("Moving to front for melee attack");
                                c.Location = Locations.Front;
                            }
                            //in the case of multi-attacks, it's possible that opponent was dropped by previous hit
                            //so we want to refilter the list to targets that are still alive
                            targetList.RemoveAll(t => !t.IsAlive());
                            if (targetList.Count == 0)
                            {
                                _log.Debug("Can't continue attack -- no remaining targets");
                                break;
                            }
                            List<Character> front = targetList.Where(t => t.Location == Locations.Front).ToList();
                            if (front.Count == 0)
                            {
                                if (!c.CanMove())
                                {
                                    _log.Debug("Can't attack -- no targets in front row and immobilized");
                                    break;
                                }
                            }
                            else
                                targetList = front;
                        }
                        resolveAction(c, action, targetList);
                        remainingAttacks--;
                    }
                }
                if (!c.HasAction && !c.HasBonusAction) //remove this clause if there are ever actions that are "free" -- only way to complete then would be exhausting the search
                {
                    _log.Debug("Bonus and regular action used -- turn complete");
                    finished = true;
                }
            }
            if (c.IsConcentrating)
                c.Statistics.TurnsConcentrating++;
        }

        private void resolveAction(Character c, CombatAction action, List<Character> targetList)
        {
            if (targetList.Count > action.TargetLimit)
            {
                TargetingStyle tstyle;
                if (c.IsPC)
                    tstyle = session.PCTargetingStyle;
                else
                    tstyle = session.NPCTargetingStyle;
                if (tstyle == TargetingStyle.Focus) //todo: review this
                    targetList = targetList.OrderBy(t => t.CurrentHP).ThenBy(t => t.CurrentAC).ToList();
                else //todo: review this (should consider advantage)
                    targetList = targetList.OrderBy(t => _roll.Roll20()).ToList();
            }
            for (int i=0; i < Math.Min(targetList.Count, action.TargetLimit); i++)
            {
                switch (action.ActionType)
                {
                    case ActionTypes.Attack:
                        resolveAttack(c, action, targetList[i], session);
                        break;
                    case ActionTypes.Heal:
                        resolveHeal(c, action, targetList[i]);
                        break;
                    case ActionTypes.Spell:
                        resolveSpell(c, action, targetList[i]);
                        break;
                }
            }
        }

        private void resolveSpell(Character caster, CombatAction action, Character target)
        {
            _log.Debug("Casting " + action.Name + " on " + target.Name);
            bool success = action.AutomaticHit;
            if (action.RequiresConcentration)
                caster.IsConcentrating = true;
            if (!success)
                success = !makeSave(target, action.SaveType, action.SaveDC);
            if (!success && !action.SaveForHalf)
            {
                _log.Debug("No effect");
                return;
            }
            if (success && action.ApplyEffect)
            {
                _log.Debug(target.Name + " is " + action.EffectApplied);
                target.AddCondition(action.EffectApplied, caster, action.SaveDC);
                if (action.Name == "Tasha's Hideous Laughter")
                    target.AddCondition(Conditions.Prone);
            }
            int dmg = 0;
            if (action.Damage != "")
                dmg = _roll.RollDice(action.Damage, false);
            dmg += action.DamageBonus;
            if (!success)
                dmg /= 2;
            if (dmg > 0)
                applyDamage(caster, target, action, dmg);
            if (action.Targeting.HasSpecialParm(SpecialParms.ArcaneWard) && caster.WardHP < caster.WardMaxHP) //ideally this should be special ability of character, keyed off the class of spell
            {
                int ward = action.SpellLevel + action.SpellLevel;
                if (caster.WardHP + ward > caster.WardMaxHP)
                    caster.WardHP = caster.WardMaxHP;
                else
                    caster.WardHP += ward;
                _log.Debug("Recharging arcane ward (now at " + caster.WardHP + ")");
            }
        }

        private void resolveHeal(Character healer, CombatAction action, Character target)
        {
            _log.Debug("Healing " + target.Name + " with " + action.Name);
            if (target.CurrentHP == 0)
            {
                if (target.HasCondition(Conditions.Dead))
                {
                    _log.Debug("No effect on dead target!!");
                    return;
                }
                else
                {
                    target.DeathSaveFailures = 0;
                    target.DeathSaveSuccesses = 0;
                    target.RemoveCondition(Conditions.Dying);
                    ggLeft++;
                    _log.Debug(target.Name + " restored to consciousness!");
                }
            }
            int healing = _roll.RollDice(action.Damage, false);
            healing += action.DamageBonus;
            if (target.CurrentHP + healing > target.MaxHP)
            {
                _log.Debug(target.Name + " healed for " + healing + " HP (" + (target.CurrentHP + healing - target.MaxHP).ToString() + " overheal)");
                target.Statistics.HealingReceived += healing; //healing received is the total amount of the heal
                action.HealingDone += target.MaxHP - target.CurrentHP;
                action.OverHealingDone += target.CurrentHP + healing - target.MaxHP;
                target.CurrentHP = target.MaxHP;
            }
            else
            {
                target.Statistics.HealingReceived += healing;
                action.HealingDone += healing;
                target.CurrentHP += healing;
                _log.Debug(target.Name + " healed for " + healing + " HP (now at " + target.CurrentHP + ")");
            }
            return;
        }

        private bool checkPowerAttack(Character attacker, CombatAction action, Character target, bool hasAdv, bool hasDis)
        {
            if (hasDis && !hasAdv)
                return false;
            double regHit = 0.05 * (20 + action.AttackBonus - target.CurrentAC);
            if (attacker.HasCondition(Conditions.Blessed))
                regHit += 0.125;
            if (regHit <= 0)
                return true;
            double dmgCut;
            if (hasAdv && !hasDis)
                if (regHit <= 0.25)
                    dmgCut = 0.975 / (1.9 * regHit - Math.Pow(regHit, 2));
                else
                    dmgCut = (Math.Pow(regHit, 2) * -10 + 24 * regHit - 4.4) / (0.5375 - regHit / 2);
            else
                if (regHit <= 0.25)
                dmgCut = 0.5 / regHit;
            else
                dmgCut = 40.0 * regHit - 8;
            double dmg = getAverageDamage(action.Damage, action.GWF);
            if (target.IsCursedByAttacker(attacker))
                dmg += getAverageDamage("d6", false);
            if (attacker.SneakAttackDamage != null & attacker.SneakAttackDamage != "" && !attacker.UsedSneakAttack)
                dmg += getAverageDamage(attacker.SneakAttackDamage, false);
            dmg += action.DamageBonus;
            _log.Debug("PA check reg hit: " + regHit + " dmg: " + dmg + " cutoff: " + dmgCut);
            return dmg < dmgCut && dmg < target.CurrentHP;
        }

        private void applyDamage(Character attacker, Character target, CombatAction action, int dmg)
        {
            if (dmg <= 0)
            {
                _log.Debug($"Ignoring damage of {dmg}");
                return;
            }
            target.Statistics.DamageTaken += dmg;
            action.DamageDealt += dmg;
            if (attacker.IsPC)
                session.Encounters[session.CurrentEncounter].PCDamageDealt += dmg;
            else
                session.Encounters[session.CurrentEncounter].PCDamageTaken += dmg;
            _log.Debug("Total Damage = " + dmg);
            if (target.WardHP > 0)
            {
                if (target.WardHP >= dmg)
                {
                    _log.Debug("Blow completely absorbed by ward ");
                    target.Statistics.WardHPAbsorbed += dmg;
                    target.WardHP -= dmg;
                    dmg = 0;
                }
                else
                {
                    _log.Debug("Blow partially absorbed by ward (" + target.WardHP + " absorbed)");
                    target.Statistics.WardHPAbsorbed += target.WardHP;
                    dmg -= target.WardHP;
                    target.WardHP = 0;
                }
            }
            if (dmg > 0)
            {
                int dmgRem = dmg;
                if (target.TempHP > 0)
                {
                    if (target.TempHP >= dmg)
                    {
                        _log.Debug("Blow absorbed into temp HP ");
                        target.Statistics.TempHPAbsorbed += dmg;
                        target.TempHP -= dmg;
                        dmgRem = 0;
                    }
                    else
                    {
                        _log.Debug("Blow partially absorbed into temp HP (" + target.TempHP + " absorbed)");
                        target.Statistics.TempHPAbsorbed += target.TempHP;
                        dmgRem -= target.TempHP;
                        target.TempHP = 0;
                    }
                }
                bool kill = false;
                if (target.CurrentHP <= dmgRem)
                {
                    kill = true;
                    CombatAction ability = target.GetAbility("Relentless");
                    if (ability != null && ability.RemainingUses > 0 && dmg <= ability.DamageBonus)
                    {
                        _log.Debug(target.Name + " is relentless and avoids a killing blow!");
                        dmgRem = target.CurrentHP - 1;
                        kill = false;
                    }
                    if (kill)
                    {
                        ability = target.GetAbility("Strength of the Grave");
                        if (ability != null && makeSave(target, SaveTypes.Con, dmg))
                        {
                            _log.Debug(target.Name + " uses Strength of the Grave and avoids a killing blow!");
                            dmgRem = target.CurrentHP - 1;
                            ability.TimesUsed++;
                            kill = false;
                        }
                    }
                }
                if (kill)
                {
                    attacker.HasKillThisRound = true;
                    CombatAction ability = attacker.GetAbility("Dark One's Blessing");
                    if (ability != null)
                        if (attacker.TempHP < ability.DamageBonus)
                        {
                            _log.Debug(attacker.Name + " has " + ability.DamageBonus + " temp HP (Dark One's Blessing)");
                            attacker.TempHP = ability.DamageBonus;
                            ability.TimesUsed++;
                        }
                    //if (target.Name == "Won Ton Slaughter")
                    //    breakGrapple(target);
                    if (target.IsConcentrating)
                    {
                        _log.Debug(target.Name + " loses concentration due to being dropped");
                        breakConcentration(target);
                        target.Statistics.HitWhileConcentrating++;
                    }
                    target.Statistics.OverKillTaken += dmgRem - target.CurrentHP;
                    action.OverKillDealt += dmgRem - target.CurrentHP;
                    if (target.IsPC)
                    {
                        ggLeft--;
                        session.Encounters[session.CurrentEncounter].PCDrops++;
                        if (dmgRem > target.CurrentHP + target.MaxHP)
                        {
                            _log.Debug(target.Name + " is killed outright from a massive blow!!!");
                            session.Encounters[session.CurrentEncounter].PCDeaths++;
                            target.AddCondition(Conditions.Dead);
                        }
                        else
                        {
                            _log.Debug(target.Name + " is knocked out and dying");
                            target.AddCondition(Conditions.Dying);
                            target.AddCondition(Conditions.Prone);
                        }
                    }
                    else
                    {
                        _log.Debug(target.Name + " is dead");
                        target.AddCondition(Conditions.Dead);
                        bgLeft--;
                    }
                    if (action.ActionType == ActionTypes.Attack && action.AttackType == AttackTypes.Melee)
                    {
                        ability = target.GetAbility("Death Burst");
                        if (ability != null)
                        {
                            _log.Debug("Death burst on " + attacker.Name);
                            int dbdmg = _roll.RollDice(ability.Damage, false);
                            if (makeSave(attacker, ability.SaveType, ability.SaveDC))
                                dbdmg /= 2;
                            applyDamage(target, attacker, ability, dbdmg);
                        }
                    }
                    target.CurrentHP = 0;
                    target.Statistics.TimesDropped++;
                    action.Kills++;
                }
                else
                {
                    target.CurrentHP -= dmgRem;
                    _log.Debug(target.Name + " " + target.CurrentHP + " HP remaining");
                    if (target.IsConcentrating)
                    {
                        target.Statistics.HitWhileConcentrating++;
                        if (!checkConcentration(target, dmg))
                            breakConcentration(target);
                    }
                }
            }
        }

        private bool checkConcentration(Character target, int damageDealt)
        {
            int DC = 10;
            if (damageDealt > 21)
                DC = damageDealt / 2;
            _log.Debug(target.Name + " making concentration check (DC " + DC + ")");
            bool success = makeSave(target, SaveTypes.Con, DC);
            if (success)
                _log.Debug("Success");
            else
                _log.Debug("Failure!");
            return success;
        }

        //private void breakGrapple(Character target)
        //{
        //    //for now this is only for WT Slaughter
        //    foreach (var c in session.Encounters[session.CurrentEncounter].Opponents)
        //        if (c.Effects != null)
        //            c.RemoveCondition(Conditions.Grappled);
        //}

        private void breakConcentration(Character target)
        {
            foreach (var c in session.PCs)
                if (c.Effects != null)
                    foreach(Effect effect in c.Effects.Where(e => e.Source == target && e.ConcentrationRequired))
                    {
                        _log.Debug("Removing " + effect.Name + " from " + c.Name);
                        c.RemoveCondition(effect.Name);
                    }
            foreach (var c in session.Encounters[session.CurrentEncounter].Opponents)
                if (c.Effects != null)
                    foreach (Effect effect in c.Effects.Where(e => e.Source == target && e.ConcentrationRequired))
                    {
                        _log.Debug("Removing " + effect.Name + " from " + c.Name);
                        c.RemoveCondition(effect.Name);
                    }
            target.IsConcentrating = false;
            target.Statistics.ConcentrationBroken++;
        }

        private double getAverageDamage(string dice, bool gwf)
        {
            int intdloc = dice.IndexOf("d");
            int numDice = 1;
            if (intdloc > 0)
                numDice = Convert.ToInt32(dice.Substring(0, intdloc));
            int val = Convert.ToInt32(dice.Substring(intdloc + 1, dice.Length - intdloc - 1));
            double baseDamage;
            if (gwf)
                baseDamage = (double)(val + 1) / 2 + (double)(val - 2) / val;
            else
                baseDamage = (double)(val + 1) / 2;
            baseDamage *= numDice;
            return baseDamage;
        }

        private void resolveAttack(Character attacker, CombatAction action, Character target, Session session)
        {
            _log.Debug("Attacking " + target.Name + " with " + action.Name);
            bool powerAtt = false;
            action.AttacksMade++;
            target.Statistics.AttacksAgainst++;
            int attRoll;
            bool hasAdv = attacker.AdvantageFor(action.AttackType) || target.AdvantageAgainst(action.AttackType) || (session.Encounters[session.CurrentEncounter].Round == 1 && !target.HasActedThisEncounter && attacker.HasAbility("Natural Explorer"));
            bool hasDis = attacker.DisadvantageFor(action.AttackType) || target.DisadvantageAgainst(action.AttackType);
            if (hasAdv && !hasDis)
            {
                action.AttacksWithAdvantage++;
                attRoll = _roll.Roll20WithAdv();
            }
            else if (hasDis && !hasAdv) 
                attRoll = _roll.Roll20WithDis();
            else
                attRoll = _roll.Roll20();
            bool isCrit = attRoll == 20;
            bool isFumble = attRoll == 1;
            if (action.GWM || action.SS)
                powerAtt = checkPowerAttack(attacker, action, target, hasAdv, hasDis);
            if (!isFumble && !isCrit && attacker.HasCondition(Conditions.Blessed))
                attRoll += _roll.RollDice("d4", false);
            if (powerAtt)
            {
                action.PowerAttacks++;
                _log.Debug("Power Attacking");
                attRoll -= 5;
            }
            //this only works for valor bard inspiration
            //if (!isCrit && !isFumble && attRoll + action.AttackBonus >= target.CurrentAC && attRoll + action.AttackBonus < target.CurrentAC + 2)
            //    if (target.HasReaction && target.HasCondition(Conditions.CombatInspired))
            //    {
            //        {
            //            _log.Debug("Using bardic inspiration");
            //            attRoll -= _roll.RollDice("d6", false);
            //            target.RemoveCondition(Conditions.CombatInspired);
            //        }
            //    }
            if (!isCrit && !isFumble && attRoll + action.AttackBonus >= target.CurrentAC && attRoll + action.AttackBonus < target.CurrentAC + 5)
                if (target.HasReaction && !target.HasCondition(Conditions.Shielded))
                {
                    CombatAction ability = target.GetAbility("Shield");
                    if (ability != null)
                    {
                        if (ability.IsAvailable(target.RemainingSpellSlots))
                        {
                            _log.Debug(target.Name + " uses Shield");
                            resolveSpell(target, ability, target); //for shield this should apply the effect and increase the current AC
                            target.HasReaction = false;
                            ability.TimesUsed++;
                            if (ability.RefreshType == RefreshTypes.Spell)
                                target.RemainingSpellSlots[ability.SpellLevel - 1]--;
                            else if (ability.RefreshType == RefreshTypes.LongRest || ability.RefreshType == RefreshTypes.ShortRest || ability.RefreshType == RefreshTypes.Recharge)
                                ability.RemainingUses--;
                        }
                    }
                }
            if (!isCrit && !isFumble && attRoll + action.AttackBonus < target.CurrentAC && attRoll + action.AttackBonus + 2 >= target.CurrentAC)
            {
                CombatAction ability = attacker.GetAbility("Precision Attack");
                if (ability != null && ability.RemainingUses > 0)
                {
                    ability.RemainingUses--;
                    ability.TimesUsed++;
                    _log.Debug("Using precision attack (" + ability.RemainingUses + " uses remaining)");
                    attRoll += _roll.RollDice(ability.Damage, false);
                }
            }
            if (!isCrit && !isFumble && attRoll + action.AttackBonus < target.CurrentAC && attRoll + action.AttackBonus + 2 >= target.CurrentAC)
            {
                if (attacker.HasCondition(Conditions.CombatInspired))
                {
                    _log.Debug("Using bardic inspiration");
                    attRoll += _roll.RollDice("d6", false);
                    attacker.RemoveCondition(Conditions.CombatInspired);
                }
            }
            if (isCrit || (attRoll + action.AttackBonus >= target.CurrentAC && !isFumble))
            {
                action.Hits++;
                target.Statistics.HitsAgainst++;
                int dmg = 0;
                if (isCrit)
                {
                    target.Statistics.CritsAgainst++;
                    action.Crits++;
                    attacker.HasCritThisRound = true;
                    _log.Debug("Critical Hit!");
                    CombatAction ability = attacker.GetAbility("Divine Smite");
                    if (ability != null && target.CurrentHP > 25 && attacker.RemainingSpellSlots[ability.SpellLevel - 1] > 0)
                    {
                        int bonusdmg;
                        _log.Debug("Using Divine Smite");
                        bonusdmg = _roll.RollDice(ability.Damage, isCrit);
                        dmg += bonusdmg;
                        ability.TimesUsed++;
                        attacker.RemainingSpellSlots[ability.SpellLevel - 1]--;
                    }
                }
                else
                    _log.Debug("Hit");
                //need to add different coding if anyone would use smiting outside of crits
                //if (!isCrit && attacker.Name == "Jesse")
                //{
                //    CombatAction ability = attacker.GetAbility("Divine Smite", session.Encounters[session.CurrentEncounter].Difficulty);
                //    if (ability != null && attacker.RemainingSpellSlots[ability.SpellLevel - 1] > 0)
                //    {
                //        int bonusdmg;
                //        _log.Debug("Using Divine Smite");
                //        if (action.GWF)
                //            bonusdmg = _roll.RollDice(ability.Damage, isCrit, 2);
                //        else
                //            bonusdmg = _roll.RollDice(ability.Damage, isCrit);
                //        dmg += bonusdmg;
                //        ability.TimesUsed++;
                //        attacker.RemainingSpellSlots[ability.SpellLevel - 1]--;
                //    }
                //}
                if (action.GWF)
                    dmg += _roll.RollDice(action.Damage, isCrit, 2) + action.DamageBonus;
                else
                    dmg += _roll.RollDice(action.Damage, isCrit) + action.DamageBonus;
                CombatAction abil = attacker.GetAbility("Favored Enemy");
                if (abil != null && abil.Targeting != null && abil.Targeting.SpecificTarget == target.CreatureType)
                {
                    dmg += abil.DamageBonus;
                }
                if (target.IsCursedByAttacker(attacker))
                {
                    int bonusdmg;
                    attacker.Statistics.HexAttacks++;
                    bonusdmg = _roll.RollDice("d6", isCrit);
                    attacker.Statistics.HexAttackDamageDealt += bonusdmg;
                    dmg += bonusdmg;
                }
                //this assumes that all of a rogue's attacks are eligible for sneak
                if (attacker.SneakAttackDamage != null && attacker.SneakAttackDamage != "" && !attacker.UsedSneakAttack)
                {
                    int bonusdmg;
                    _log.Debug("Sneak attack");
                    bonusdmg = _roll.RollDice(attacker.SneakAttackDamage, isCrit);
                    attacker.UsedSneakAttack = true;
                    attacker.Statistics.SneakAttacks++;
                    attacker.Statistics.SneakAttackDamageDealt += bonusdmg;
                    dmg += bonusdmg;
                }
                if (attacker.HasAbility("Martial Advantage"))
                    if (target.Location == Locations.Front)
                    {
                        int allies = session.Encounters[session.CurrentEncounter].Opponents.Where(o => o.Location == Locations.Front && o.IsAlive()).Count();
                        if (attacker.Location == Locations.Front)
                            allies--;
                        if (allies > 0)
                        {
                            _log.Debug("Martial Advantage");
                            dmg += _roll.RollDice("2d6", isCrit);
                        }
                    }
                if (powerAtt)
                    dmg += 10;
                applyDamage(attacker, target, action, dmg);
                if (action.ApplyEffect)
                    if (action.AutomaticHit || !makeSave(target, action.SaveType, action.SaveDC))
                    {
                        _log.Debug(target.Name + " is " + action.EffectApplied);
                        target.AddCondition(action.EffectApplied);
                    }
            }
            else
                _log.Debug("Miss");
        }

        private bool makeSave(Character character, SaveTypes saveType, int saveDC)
        {
            int roll = _roll.Roll20();
            if (character.HasCondition(Conditions.Blessed))
                roll += _roll.RollDice("d4", false);
            switch (saveType)
            {
                case SaveTypes.Cha:
                    roll += character.Saves.Cha;
                    break;
                case SaveTypes.Con:
                    roll += character.Saves.Con;
                    break;
                case SaveTypes.Dex:
                    roll += character.Saves.Dex;
                    break;
                case SaveTypes.Int:
                    roll += character.Saves.Int;
                    break;
                case SaveTypes.Str:
                    roll += character.Saves.Str;
                    break;
                case SaveTypes.Wis:
                    roll += character.Saves.Wis;
                    break;
            }
            if (roll < saveDC && roll + 2 >= saveDC && character.HasCondition(Conditions.CombatInspired))
            {
                _log.Debug("Using bardic inspiration");
                roll += _roll.RollDice("d6", false);
                character.RemoveCondition(Conditions.CombatInspired);
            }
            _log.Debug("Total = " + roll + " (Savetype " + saveType + " DC " + saveDC + ")");
            bool success = roll >= saveDC;
            if (success)
                _log.Debug("Success!");
            else
                _log.Debug("Failure");
            return success;
        }

        private List<Character> getInitiative(List<Character> pcList, List<Character> npcList)
        {
            List<Character> initOrder = new List<Character>();
            foreach (Character c in pcList)
            {
                prepForCombat(c);
                initOrder.Add(c);
            }
            foreach (Character c in npcList)
            {
                prepForCombat(c);
                initOrder.Add(c);
            }
            return initOrder.OrderByDescending(c => c.Initiative).ThenByDescending(c => c.InitiativeBonus).ToList();
        }

        private void prepForCombat(Character c)
        {
            _log.Debug("initiative roll for " + c.Name);
            if (c.HasAbility("Natural Explorer"))
                c.Initiative = _roll.Roll20WithAdv() + c.InitiativeBonus;
            else
                c.Initiative = _roll.Roll20() + c.InitiativeBonus;
            c.Location = c.StartingLocation;
            c.HasActedThisEncounter = false;
            if (c.TempHP < 8 && c.HasAbility("Fiendish Vigor"))
            {
                _log.Debug(c.Name + " uses Fiendish Vigor (Temp HP = 8)");
                c.TempHP = 8;
            }
        }

        private void fileReport()
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(System.IO.File.Create(@"C:\Temp\DnDReport.txt")))
            {
                int tpk = 0, pcdeaths = 0, pcdrops = 0, encounters = 0, npcturns = 0, pcturns = 0, rounds = 0, pcdmgdealt = 0, pcdmgtaken = 0, dmgtemp = 0, dmgward = 0, dmgover = 0, healed = 0;
                int attacks = 0, advattacks = 0, crits = 0, overkilldealt = 0, kills = 0, hits = 0, heal = 0, npcatt = 0, npchit = 0, npccrit = 0, shortrestheal = 0, overheal = 0, powattacks = 0;

                file.WriteLine("Report for simulation");
                for (int i = 0; i < session.Encounters.Count; i++)
                {
                    Encounter e = session.Encounters[i];
                    file.WriteLine("Encounter " + (i + 1).ToString());
                    file.WriteLine("Difficulty: " + e.Difficulty);
                    file.Write("Opponents: ");
                    foreach (var c in e.Opponents)
                        file.Write(c.Name + "; ");
                    file.WriteLine();
                    file.WriteLine("Times Run (%): " + String.Format("{0} {1:p}", e.TimesRun, (float)e.TimesRun / session.Trials));
                    encounters += e.TimesRun;
                    file.WriteLine("Total TPKs (%): " + String.Format("{0} ({1:p})", e.TPKs, (float)e.TPKs / e.TimesRun));
                    tpk += e.TPKs;
                    file.WriteLine("PC Deaths: " + String.Format("{0:f}", (float)e.PCDeaths / e.TimesRun));
                    pcdeaths += e.PCDeaths;
                    file.WriteLine("PC Drops: " + String.Format("{0:f}", (float)e.PCDrops / e.TimesRun));
                    pcdrops += e.PCDrops;
                    file.WriteLine("NPC Turns Taken: " + String.Format("{0:f}", (float)e.NPCTurnsTaken / e.TimesRun));
                    npcturns += e.NPCTurnsTaken;
                    file.WriteLine("PC Turns Taken: " + String.Format("{0:f}", (float)e.PCTurnsTaken / e.TimesRun));
                    pcturns += e.PCTurnsTaken;
                    file.WriteLine("Rounds: " + String.Format("{0:f}", (float)e.Rounds / e.TimesRun));
                    rounds += e.Rounds;
                    file.WriteLine("PCs (start): " + String.Format("{0:f}", (float)e.PCStartCount / e.TimesRun));
                    file.WriteLine("PCs (end): " + String.Format("{0:f}", (float)e.PCEndCount / e.TimesRun));
                    file.WriteLine("PC Dmg Dealt: " + String.Format("{0:f}", (float)e.PCDamageDealt / e.TimesRun));
                    file.WriteLine("PC Dmg Taken: " + String.Format("{0:f}", (float)e.PCDamageTaken / e.TimesRun));
                    file.WriteLine("-------------------------------------");
                }
                foreach (var c in session.PCs)
                {
                    int cattacks = 0, cadvattacks = 0, ccrits = 0, coverkilldealt = 0, ckills = 0, cdmg = 0, chits = 0, cheal = 0, coverheal = 0, cpowattacks = 0;
                    file.WriteLine(c.Name);
                    file.WriteLine("HP/AC: " + c.MaxHP + "/" + c.BaseAC);
                    file.WriteLine("Turns Taken: " + String.Format("{0:f}", (float)c.Statistics.TurnsTaken / session.Trials));
                    file.WriteLine("Turns Active (%): " + String.Format("{0:f} ({1:p})", c.Statistics.TurnsActive / session.Trials, (float)c.Statistics.TurnsActive / c.Statistics.TurnsTaken));
                    file.WriteLine("Turns Concentrating (%): " + String.Format("{0:f} ({1:p})", c.Statistics.TurnsConcentrating / session.Trials, (float)c.Statistics.TurnsConcentrating / c.Statistics.TurnsTaken));
                    file.WriteLine("Concentration Broken: " + String.Format("{0:f}", (float)c.Statistics.ConcentrationBroken / session.Trials));
                    file.WriteLine("Times Dropped: " + String.Format("{0:f} ", (float)c.Statistics.TimesDropped / session.Trials));
                    file.WriteLine("Times Killed (%): " + String.Format("{0:f} ({1:p})", (float)c.Statistics.TimesKilled / session.Trials, (float)c.Statistics.TimesKilled / session.Trials));
                    file.WriteLine("Action Breakdown:");
                    foreach (var a in c.Actions)
                    {
                        file.WriteLine(String.Format("\t" + a.Name));
                        file.WriteLine(String.Format("\t\tTimes Used: {0:f}", (float)a.TimesUsed / session.Trials));
                        file.WriteLine(String.Format("\t\tAttacks Made: {0:f}", (float)a.AttacksMade / session.Trials));
                        cattacks += a.AttacksMade;
                        attacks += a.AttacksMade;
                        file.WriteLine(String.Format("\t\tAttacks /w Advantage (%): {0:f} ({1:p})", (float)a.AttacksWithAdvantage / session.Trials, a.AttacksMade == 0 ? 0 : (float)a.AttacksWithAdvantage / a.AttacksMade));
                        cadvattacks += a.AttacksWithAdvantage;
                        advattacks += a.AttacksWithAdvantage;
                        file.WriteLine(String.Format("\t\tPower Attacks (%): {0:f} ({1:p})", (float)a.PowerAttacks / session.Trials, a.AttacksMade == 0 ? 0 : (float)a.PowerAttacks / a.AttacksMade));
                        cpowattacks += a.PowerAttacks;
                        powattacks += a.PowerAttacks;
                        file.WriteLine(String.Format("\t\tHits (%): {0:f} ({1:p})", (float)a.Hits / session.Trials, a.AttacksMade == 0 ? 0 : (float)a.Hits / a.AttacksMade));
                        hits += a.Hits;
                        chits += a.Hits;
                        file.WriteLine(String.Format("\t\tCrits (%): {0:f} ({1:p})", (float)a.Crits / session.Trials, a.AttacksMade == 0 ? 0 : (float)a.Crits / a.AttacksMade));
                        crits += a.Crits;
                        ccrits += a.Crits;
                        if (a.ActionType == ActionTypes.Attack)
                            file.WriteLine(String.Format("\t\tDamage Dealt (per attack): {0:f} ({1:f})", (float)a.DamageDealt / session.Trials, a.AttacksMade == 0 ? 0 : (float)a.DamageDealt / a.AttacksMade));
                        else
                            file.WriteLine(String.Format("\t\tDamage Dealt (per use): {0:f} ({1:f})", (float)a.DamageDealt / session.Trials, a.TimesUsed == 0 ? 0 : (float)a.DamageDealt / a.TimesUsed));
                        cdmg += a.DamageDealt;
                        pcdmgdealt += a.DamageDealt;
                        file.WriteLine(String.Format("\t\tOverkill (%): {0:f} ({1:p})", (float)a.OverKillDealt / session.Trials, a.DamageDealt == 0 ? 0 : (float)a.OverKillDealt / a.DamageDealt));
                        coverkilldealt += a.OverKillDealt;
                        overkilldealt += a.OverKillDealt;
                        file.WriteLine(String.Format("\t\tKills: {0:f}", (float)a.Kills / session.Trials));
                        kills += a.Kills;
                        ckills += a.Kills;
                        file.WriteLine(String.Format("\t\tHealing Done (per use): {0:f} ({1:f})", (float)a.HealingDone / session.Trials, a.TimesUsed == 0 ? 0 : (float)a.HealingDone / a.TimesUsed));
                        cheal += a.HealingDone;
                        heal += a.HealingDone;
                        file.WriteLine(String.Format("\t\tOverhealing Done (%): {0:f} ({1:p})", (float)a.OverHealingDone / session.Trials, a.HealingDone == 0 ? 0 : (float)a.OverHealingDone / a.HealingDone));
                        coverheal += a.OverHealingDone;
                        overheal += a.OverHealingDone;
                    }
                    foreach (var a in c.Abilities)
                        if (a.TimesUsed > 0)
                        {
                            file.WriteLine(String.Format("\t" + a.Name));
                            file.WriteLine(String.Format("\t\tTimes Used: {0:f}", (float)a.TimesUsed / session.Trials));
                        }
                    file.WriteLine(String.Format("Attacks Made: {0:f}", (float)cattacks / session.Trials));
                    file.WriteLine(String.Format("Attacks /w Advantage (%): {0:f} ({1:p})", (float)cadvattacks / session.Trials, cattacks == 0 ? 0 : (float)cadvattacks / cattacks));
                    file.WriteLine(String.Format("Power Attacks (%): {0:f} ({1:p})", (float)cpowattacks / session.Trials, cattacks == 0 ? 0 : (float)cpowattacks / cattacks));
                    file.WriteLine(String.Format("Hits (%): {0:f} ({1:p})", (float)chits / session.Trials, cattacks == 0 ? 0 : (float)chits / cattacks));
                    file.WriteLine(String.Format("Crits (%): {0:f} ({1:p})", (float)ccrits / session.Trials, cattacks == 0 ? 0 : (float)ccrits / cattacks));
                    file.WriteLine(String.Format("Damage Dealt (per act turn): {0:f} ({1:f})", (float)cdmg / session.Trials, cattacks == 0 ? 0 : (float)cdmg / c.Statistics.TurnsActive));
                    if (c.Statistics.SneakAttacks > 0)
                    {
                        file.WriteLine(String.Format("Sneak Attack Hits (% act turn): {0:f} ({1:p})", (float)c.Statistics.SneakAttacks / session.Trials, (float)c.Statistics.SneakAttacks / c.Statistics.TurnsActive));
                        file.WriteLine(String.Format("Sneak Attack Damage: {0:f}", (float)c.Statistics.SneakAttackDamageDealt / session.Trials));
                    }
                    if (c.Statistics.HexAttacks > 0)
                    {
                        file.WriteLine(String.Format("Hex Attack Hits (% attacks): {0:f} ({1:p})", (float)c.Statistics.HexAttacks / session.Trials, (float)c.Statistics.HexAttacks / cattacks));
                        file.WriteLine(String.Format("Hex Damage: {0:f}", (float)c.Statistics.HexAttackDamageDealt / session.Trials));
                    }
                    file.WriteLine(String.Format("Overkill (%): {0:f} ({1:p})", (float)coverkilldealt / session.Trials, cdmg == 0 ? 0 : (float)coverkilldealt / cdmg));
                    file.WriteLine(String.Format("Kills: {0:f}", (float)ckills / session.Trials));
                    file.WriteLine(String.Format("Healing Done: {0:f}", (float)cheal / session.Trials));
                    file.WriteLine(String.Format("Overhealing Done (%): {0:f} ({1:p})", (float)coverheal / session.Trials, cheal == 0 ? 0 : (float)coverheal / cheal));
                    file.WriteLine(String.Format("Attacks Against: {0:f}", (float)c.Statistics.AttacksAgainst / session.Trials));
                    npcatt += c.Statistics.AttacksAgainst;
                    file.WriteLine(String.Format("Hits Against (%): {0:f} ({1:p})", (float)c.Statistics.HitsAgainst / session.Trials, c.Statistics.AttacksAgainst == 0 ? 0 : (float)c.Statistics.HitsAgainst / c.Statistics.AttacksAgainst));
                    npchit += c.Statistics.HitsAgainst;
                    file.WriteLine(String.Format("Crits Against (%): {0:f} ({1:p})", (float)c.Statistics.CritsAgainst / session.Trials, c.Statistics.AttacksAgainst == 0 ? 0 : (float)c.Statistics.CritsAgainst / c.Statistics.AttacksAgainst));
                    npccrit += c.Statistics.CritsAgainst;
                    file.WriteLine(String.Format("Damage Taken (per act turn): {0:f} ({1:f})", (float)c.Statistics.DamageTaken / session.Trials, c.Statistics.AttacksAgainst == 0 ? 0 : (float)c.Statistics.DamageTaken / c.Statistics.TurnsActive));
                    pcdmgtaken += c.Statistics.DamageTaken;
                    file.WriteLine(String.Format("Temp HP Absorb (%): {0:f} ({1:p})", (float)c.Statistics.TempHPAbsorbed / session.Trials, c.Statistics.DamageTaken == 0 ? 0 : (float)c.Statistics.TempHPAbsorbed / c.Statistics.DamageTaken));
                    dmgtemp += c.Statistics.TempHPAbsorbed;
                    file.WriteLine(String.Format("Ward HP Absorb (%): {0:f} ({1:p})", (float)c.Statistics.WardHPAbsorbed / session.Trials, c.Statistics.DamageTaken == 0 ? 0 : (float)c.Statistics.WardHPAbsorbed / c.Statistics.DamageTaken));
                    dmgward += c.Statistics.WardHPAbsorbed;
                    file.WriteLine(String.Format("Overkilled (%): {0:f} ({1:p})", (float)c.Statistics.OverKillTaken / session.Trials, c.Statistics.DamageTaken == 0 ? 0 : (float)c.Statistics.OverKillTaken / c.Statistics.DamageTaken));
                    dmgover += c.Statistics.OverKillTaken;
                    file.WriteLine(String.Format("Healing Received: {0:f}", (float)c.Statistics.HealingReceived / session.Trials));
                    healed += c.Statistics.HealingReceived;
                    file.WriteLine(String.Format("Short Rest Healing: {0:f}", (float)c.Statistics.ShortRestHealing / session.Trials));
                    shortrestheal += c.Statistics.ShortRestHealing;
                    file.WriteLine("-------------------------------------");
                }
                file.WriteLine("TOTALS");
                file.WriteLine("Trials: " + session.Trials);
                file.WriteLine(String.Format("TPKs (%): {0} ({1:p})", tpk, (float)tpk / session.Trials));
                file.WriteLine("PC Deaths: " + String.Format("{0:f}", (float)pcdeaths / session.Trials));
                file.WriteLine("PC Drops: " + String.Format("{0:f}", (float)pcdrops / session.Trials));
                file.WriteLine("NPC Turns Taken: " + String.Format("{0:f}", (float)npcturns / session.Trials));
                file.WriteLine("PC Turns Taken: " + String.Format("{0:f}", (float)pcturns / session.Trials));
                file.WriteLine("Rounds (per enc): " + String.Format("{0:f} ({1:f})", (float)rounds / session.Trials, (float)rounds / (session.Trials * session.Encounters.Count)));
                file.WriteLine(String.Format("Attacks Made: {0:f}", (float)attacks / session.Trials));
                file.WriteLine(String.Format("Attacks /w Advantage (%): {0:f} ({1:p})", (float)advattacks / session.Trials, attacks == 0 ? 0 : (float)advattacks / attacks));
                file.WriteLine(String.Format("Power Attacks (%): {0:f} ({1:p})", (float)powattacks / session.Trials, attacks == 0 ? 0 : (float)powattacks / attacks));
                file.WriteLine(String.Format("Hits (%): {0:f} ({1:p})", (float)hits / session.Trials, attacks == 0 ? 0 : (float)hits / attacks));
                file.WriteLine(String.Format("Crits (%): {0:f} ({1:p})", (float)crits / session.Trials, attacks == 0 ? 0 : (float)crits / attacks));
                file.WriteLine(String.Format("Damage Dealt (per round): {0:f} ({1:f})", (float)pcdmgdealt / session.Trials, attacks == 0 ? 0 : (float)pcdmgdealt / rounds));
                file.WriteLine(String.Format("Overkill (%): {0:f} ({1:p})", (float)overkilldealt / session.Trials, pcdmgdealt == 0 ? 0 : (float)overkilldealt / pcdmgdealt));
                file.WriteLine(String.Format("Kills: {0:f}", (float)kills / session.Trials));
                file.WriteLine(String.Format("Healing Done: {0:f}", (float)heal / session.Trials));
                file.WriteLine(String.Format("Overhealing Done (%): {0:f} ({1:p})", (float)overheal / session.Trials, heal == 0 ? 0 : (float)overheal / heal));
                file.WriteLine(String.Format("Attacks Against: {0:f}", (float)npcatt / session.Trials));
                file.WriteLine(String.Format("Hits Against (%): {0:f} ({1:p})", (float)npchit / session.Trials, npcatt == 0 ? 0 : (float)npchit / npcatt));
                file.WriteLine(String.Format("Crits Against (%): {0:f} ({1:p})", (float)npccrit / session.Trials, npcatt == 0 ? 0 : (float)npccrit / npcatt));
                file.WriteLine(String.Format("Damage Taken (per attack): {0:f} ({1:f})", (float)pcdmgtaken / session.Trials, npcatt == 0 ? 0 : (float)pcdmgtaken / npcatt));
                file.WriteLine(String.Format("Temp HP Absorb (%): {0:f} ({1:p})", (float)dmgtemp / session.Trials, pcdmgtaken == 0 ? 0 : (float)dmgtemp / pcdmgtaken));
                file.WriteLine(String.Format("Ward HP Absorb (%): {0:f} ({1:p})", (float)dmgward / session.Trials, pcdmgtaken == 0 ? 0 : (float)dmgward / pcdmgtaken));
                file.WriteLine(String.Format("Overkilled (%): {0:f} ({1:p})", (float)dmgover / session.Trials, pcdmgtaken == 0 ? 0 : (float)dmgover / pcdmgtaken));
                file.WriteLine(String.Format("Short Rest Healing: {0:f}", (float)shortrestheal / session.Trials));
            }

        }
    }
}