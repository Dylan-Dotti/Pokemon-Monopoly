using UnityEngine;

[CreateAssetMenu(fileName = "BallPropertyData", menuName = "Scriptable Objects/Ball Property Data")]
public class BallPropertyData : PropertyData
{
    public override int CurrentRent
    {
        get
        {
            if (Owner == null) return 0;
            return RentWithMultipleBalls(Owner.BallProperties.Count);
        }
    }

    public int RentWithMultipleBalls(int numBalls)
    {
        if (numBalls < 0)
        {
            throw new System.ArgumentException(
                "Can't have fewer than 0 balls");
        }
        if (numBalls == 0) return 0;
        return BaseRent * (int)Mathf.Pow(2, numBalls - 1);
    }
}
