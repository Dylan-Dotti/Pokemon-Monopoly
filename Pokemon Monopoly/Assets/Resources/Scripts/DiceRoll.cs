
public struct DiceRoll
{
    public int Roll1 { get; private set; }
    public int Roll2 { get; private set; }

    public int RollTotal => Roll1 + Roll2;
    public bool IsDoubleRoll => Roll1.Equals(Roll2);

    public DiceRoll(int roll1, int roll2)
    {
        Roll1 = roll1;
        Roll2 = roll2;
    }
}
