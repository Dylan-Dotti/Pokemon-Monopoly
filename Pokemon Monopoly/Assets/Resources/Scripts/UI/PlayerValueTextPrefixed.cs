using UnityEngine;

public abstract class PlayerValueTextPrefixed : PlayerValueText
{
    [SerializeField] protected string prefix;

    public string Prefix
    {
        get => prefix;
        set
        {
            prefix = value;
        }
    }

    protected override string GetValueText(MonopolyPlayer player)
    {
        return Prefix;
    }
}
