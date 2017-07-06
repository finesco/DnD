namespace DnD.DTO
{
    public class GenericMonsterStats
    {
        public int Id { get; set; }
        public string CR { get; set; }
        public int ProficiencyBonus { get; set; }
        public int ArmorClass { get; set; }
        public int HitPoints { get; set; }
        public int AttackBonus { get; set; }
        public int Damage { get; set; }
        public int SaveDC { get; set; }
    }
}
