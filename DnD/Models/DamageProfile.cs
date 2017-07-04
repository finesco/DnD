namespace DnD.Models
{
    public class DamageProfile
    {
        public int Attribute { get; set; }
        public int Level { get; set; }
        public string DamageDice { get; set; }
        public string ExtraDamageDice { get; set; }
        public int CritRange { get; set; }
        public bool SaveAllowed { get; set; }
        public bool SaveHalfDamage { get; set; }
        public bool GWF { get; set; }
        public bool IsBarb { get; set; }
        public int WeaponBonus { get; set; }
        public bool GWM { get; set; }
        public int NumAttacks { get; set; }
        public bool IncludeBonusInDamage { get; set; }
        public bool IsWarlock { get; set; }
        public int OtherHitModifier { get; set; }
        public int OtherDamageModifier { get; set; }

        public DamageProfile()
        {
            Attribute = 16;
            Level = 1;
            DamageDice = "1d6";
            CritRange = 20;
            SaveAllowed = false;
            SaveHalfDamage = false;
            GWF = false;
            GWM = false;
            IsBarb = false;
            WeaponBonus = 0;
            NumAttacks = 1;
            IncludeBonusInDamage = true;
            IsWarlock = false;
            OtherDamageModifier = 0;
            OtherHitModifier = 0;
            ExtraDamageDice = "0d6";
        }
    }
}