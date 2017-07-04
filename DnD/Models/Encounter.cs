using System.Collections.Generic;

namespace DnD.Models
{
    public enum EncounterDifficulties { Easy, Medium, Hard, Deadly }


    public class Encounter
    {
        public EncounterDifficulties Difficulty { get; set; }
        public List<Character> Opponents { get; set; }
        public int PCDeaths { get; set; }
        public int Round { get; set; }
        public int Rounds { get; set; }
        public int PCDrops { get; set; }
        public int TimesRun { get; set; }
        public int TPKs { get; set; }
        public int NPCTurnsTaken { get; set; }
        public int PCTurnsTaken { get; set; }
        public int PCStartCount { get; set; }
        public int PCEndCount { get; set; }
        public int PCDamageDealt { get; set; }
        public int PCDamageTaken { get; set; }

        public Encounter()
        {
            Opponents = new List<Character>();
        }
    }
}
