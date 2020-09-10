using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollPanel : MonoBehaviour
{
    private GameObject background;
    private SixSidedDie[] dice;
    private Text playerIsRolling;
    private Text playerRolled;

    private Coroutine rollingCR;
    private string rollingPlayerName = "Player";

    private void Awake()
    {
        background = transform.Find("Background").gameObject;
        dice = GetComponentsInChildren<SixSidedDie>();
        playerIsRolling = transform.Find("Player is rolling")
            .GetComponent<Text>();
        playerRolled = transform.Find("Player rolled")
            .GetComponent<Text>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ActivateAndStartRoll(
        string playerName, float changeInterval = 0.05f)
    {
        Activate();
        StartRolling(playerName, changeInterval);
    }

    public void StartRolling(
        string playerName, float changeInterval = 0.05f)
    {
        if (rollingCR != null)
            throw new System.Exception("Already rolling");
        rollingPlayerName = playerName;
        playerIsRolling.text = $"{playerName} is rolling...";
        rollingCR = StartCoroutine(RollDiceCR(changeInterval));
    }

    public void StopRolling(int result1, int result2)
    {
        StopCoroutine(rollingCR);
        rollingCR = null;
        dice[0].DisplaySide(result1);
        dice[1].DisplaySide(result2);
        int total = result1 + result2;
        playerRolled.text = $"{rollingPlayerName} rolled {total}";
        rollingPlayerName = "Player";
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
