using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DnD.Models;

namespace DnD.Tests
{
    [TestClass]
    public class CharacterTests
    {
        private Character c1;

        public CharacterTests()
        {
            c1 = new Character("Test", true, Locations.Back, 15, 28, 3, 1, 2, 3, 4, 5, 6);
        }

        [TestMethod]
        public void GetSaveReturnsCorrectSave()
        {
            Assert.AreEqual(1, c1.GetSave(SaveTypes.Str));
            Assert.AreEqual(2, c1.GetSave(SaveTypes.Dex));
            Assert.AreEqual(3, c1.GetSave(SaveTypes.Con));
            Assert.AreEqual(4, c1.GetSave(SaveTypes.Int));
            Assert.AreEqual(5, c1.GetSave(SaveTypes.Wis));
            Assert.AreEqual(6, c1.GetSave(SaveTypes.Cha));
        }

        [TestMethod]
        public void IsTargetableReturnsTrueWhenNotRemovedFromPlay()
        {
            c1.Effects.Add(new Effect(Conditions.Stunned));
            c1.Effects.Add(new Effect(Conditions.Dead));
            c1.Effects.Add(new Effect(Conditions.Invisible));
            c1.Effects.Add(new Effect(Conditions.Unconscious));

            Assert.IsTrue(c1.IsTargetable());
        }


        [TestMethod]
        public void IsTargetableReturnsFalseWhenRemovedFromPlay()
        {
            c1.Effects.Add(new Effect(Conditions.Banished));

            Assert.IsFalse(c1.IsTargetable());
        }

        [TestMethod]
        public void RemoveConditionBySourceDoesNotRemoveFromDifferentSource()
        {
            c1.Effects.Add(new Effect(Conditions.Blinded) { Source = new Character("Test1", true, Locations.Back, 15, 28, 3, 1, 2, 3, 4, 5, 6) });
            c1.RemoveConditionBySource(Conditions.Blinded, new Character("Test2", true, Locations.Back, 15, 28, 3, 1, 2, 3, 4, 5, 6));

            Assert.IsTrue(c1.HasCondition(Conditions.Blinded));        
        }

        [TestMethod]
        public void RemoveConditionBySourcRemovesFromCorrectSource()
        {
            var c2 = new Character("Test1", true, Locations.Back, 15, 28, 3, 1, 2, 3, 4, 5, 6);
            c1.Effects.Add(new Effect(Conditions.Blinded) { Source = c2 });
            c1.RemoveConditionBySource(Conditions.Blinded, c2);

            Assert.IsFalse(c1.HasCondition(Conditions.Blinded));
        }

        [TestMethod]
        public void RemovingShieldDecreasesAC()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded));
            c1.RemoveCondition(Conditions.Shielded);

            Assert.AreEqual(10, c1.CurrentAC);
        }

        [TestMethod]
        public void RemovingShieldBySourceDecreasesAC()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { Source = c1 });
            c1.RemoveConditionBySource(Conditions.Shielded, c1);

            Assert.AreEqual(10, c1.CurrentAC);
        }

        [TestMethod]
        public void AddingShieldIncreasesAC()
        {
            c1.AddCondition(Conditions.Shielded);

            Assert.AreEqual(20, c1.CurrentAC);
        }

        [TestMethod]
        public void AddingShieldBySourceIncreasesAC()
        {
            c1.AddCondition(Conditions.Shielded, c1);

            Assert.AreEqual(20, c1.CurrentAC);
        }

        [TestMethod]
        public void MeleeAdvantageForEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsAdvantageMelee = true });

            Assert.IsTrue(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void RangedAdvantageForEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsAdvantageRanged = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsTrue(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void MeleeAdvantageAgainstEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsAdvantageAgainstMelee = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsTrue(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void RangedAdvantageAgainstEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsAdvantageAgainstRanged = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsTrue(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void MeleeDisadvantageForEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsDisadvantageMelee = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsTrue(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void RangedDisadvantageForEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsDisadvantageRanged = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsTrue(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void MeleeDisadvantageAgainstEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsDisadvantageAgainstMelee = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsTrue(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }

        [TestMethod]
        public void RangedDisadvantageAgainstEffectsGrantAdvantageCorrectly()
        {
            c1.Effects.Add(new Effect(Conditions.Shielded) { GrantsDisadvantageAgainstRanged = true });

            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Melee));
            Assert.IsFalse(c1.AdvantageAgainst(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Melee));
            Assert.IsFalse(c1.DisadvantageFor(AttackTypes.Ranged));
            Assert.IsFalse(c1.DisadvantageAgainst(AttackTypes.Melee));
            Assert.IsTrue(c1.DisadvantageAgainst(AttackTypes.Ranged));
        }
    }
}
