using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollPanel : Popup
{
    [SerializeField] private Text playerIsRolling;
    [SerializeField] Text playerRolled;
    [SerializeField] private SixSidedDie[] dice;
    [SerializeField] private float changeInterval = 0.05f;

    private Coroutine rollingCR;
    public string RollingPlayerName { get; set; } = "Player";

    public override Coroutine Open()
    {
        Coroutine openCoroutine = base.Open();
        StartRolling();
        return openCoroutine;
    }

    public void StartRolling()
    {
        if (rollingCR != null)
            throw new System.Exception("Already rolling");
        playerIsRolling.gameObject.SetActive(true);
        playerRolled.gameObject.SetActive(false);
        playerIsRolling.text = $"{RollingPlayerName} is rolling...";
        rollingCR = StartCoroutine(RollDiceCR(changeInterval));
    }

    public void StopRolling(int result1, int result2)
    {
        StopCoroutine(rollingCR);
        rollingCR = null;
        dice[0].DisplaySide(result1);
        dice[1].DisplaySide(result2);
        int total = result1 + result2;
        playerRolled.text = $"{RollingPlayerName} rolled {total}";
        RollingPlayerName = "Player";
        playerIsRolling.gameObject.SetActive(false);
        playerRolled.gameObject.SetActive(true);
    }

    private IEnumerator RollDiceCR(float changeInterval)
    {
        while (true)
        {
            dice[0].DisplayRandomSide();
            dice[1].DisplayRandomSide();
            yield return new WaitForSeconds(changeInterval);
        }
    }
}
