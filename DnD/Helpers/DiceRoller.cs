using log4net;
using System;

namespace DnD.Helpers
{

    public class DiceRoller : IDiceRoller
    {
        private ILog _log;
        private Random _rng;

        public DiceRoller(ILog logger)
        {
            _log = logger;
            _rng = new Random();
        }

        public int Roll20WithAdv()
        {
            _log.Debug("rolling with advantage");
            return Math.Max(Roll20(), Roll20());
        }

        public int Roll20WithDis()
        {
            _log.Debug("rolling with disadvantage");
            return Math.Min(Roll20(), Roll20());
        }

        public int Roll20()
        {
            int roll = _rng.Next(1, 21);
            _log.Debug("d20 roll: " + roll);
            return roll;
        }

        public int RollDice(string dice, bool isCrit)
        {
            if (dice == null || dice == "")
                return 0;
            int numDice = 1;
            int valueDice;
            int intdloc = dice.IndexOf("d");
            if (intdloc > 0)
                numDice = Convert.ToInt32(dice.Substring(0, intdloc));
            if (isCrit)
                numDice += numDice;
            valueDice = Convert.ToInt32(dice.Substring(intdloc + 1, dice.Length - intdloc - 1));
            int sum = 0;
            for (int i = 0; i < numDice; i++)
                sum += _rng.Next(1, valueDice + 1);
            _log.Debug("rolling " + numDice + "d" + valueDice + ": " + sum);
            return sum;
        }

        public int RollDice(string dice, bool isCrit, int rerollValue)
        {
            int numDice = 1;
            int valueDice;
            int roll;
            int intdloc = dice.IndexOf("d");
            if (intdloc > 0)
                numDice = Convert.ToInt32(dice.Substring(0, intdloc));
            if (isCrit)
                numDice += numDice;
            valueDice = Convert.ToInt32(dice.Substring(intdloc + 1, dice.Length - intdloc - 1));
            int sum = 0;
            for (int i = 0; i < numDice; i++)
            {
                roll = _rng.Next(1, valueDice + 1);
                if (roll > rerollValue)
                    sum += roll;
                else
                    sum += _rng.Next(1, valueDice + 1);
            }
            _log.Debug("rolling " + numDice + "d" + valueDice + " (rerolling results of " + rerollValue + " or less): " + sum);
            return sum;
        }

    }
}
