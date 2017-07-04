namespace DnD.Helpers
{
    public interface IDiceRoller
    {
        int Roll20WithAdv();
        int Roll20WithDis();
        int Roll20();
        int RollDice(string dice, bool isCrit);
        int RollDice(string dice, bool isCrit, int maxRerollValue);
    }
}
