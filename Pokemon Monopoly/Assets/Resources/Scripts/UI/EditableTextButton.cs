using UnityEngine.UI;

public class EditableTextButton : Button
{
    private Text buttonText;

    public string text
    {
        get => buttonText.text;
        set => buttonText.text = value;
    }

    protected override void Awake()
    {
        base.Awake();
        buttonText = GetComponentInChildren<Text>();
    }
}
