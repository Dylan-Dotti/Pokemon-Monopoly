using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MoneyText : Text
{
    public override string text
    {
        get => base.text;
        set
        {
            string newValue = value.Replace(SpecialStrings.POKEMONEY_SYMBOL, "");
            newValue = newValue.Insert(0, SpecialStrings.POKEMONEY_SYMBOL);
            base.text = newValue;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        text = "200";
    }
}
