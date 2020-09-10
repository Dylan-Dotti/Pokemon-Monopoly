using UnityEngine;
using UnityEngine.UI;

public class SixSidedDie : MonoBehaviour
{
    private Text rollText;
    private int lastSideDisplayed;

    private void Awake()
    {
        rollText = GetComponentInChildren<Text>();
    }

    public void DisplayRandomSide()
    {
        int side = lastSideDisplayed;
        while (side == lastSideDisplayed)
        {
            side = Random.Range(1, 7);
        }
        DisplaySide(side);
    }

    public void DisplaySide(int side)
    {
        if (side < 1 || side > 6)
            throw new System.ArgumentException("invalid side: " + side);
        rollText.text = side.ToString();
        lastSideDisplayed = side;
    }
}
