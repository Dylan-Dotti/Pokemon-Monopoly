using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceRoll : MonoBehaviour
{
    [SerializeField] private PlayerRollController controller;

    private InputField rollInput1;
    private InputField rollInput2;
    private Button rollButton;

    private void Awake()
    {
        rollInput1 = transform.Find("Roll Values").Find("Roll1")
            .GetComponent<InputField>();
        rollInput2 = transform.Find("Roll Values").Find("Roll2")
            .GetComponent<InputField>();
        rollButton = GetComponentInChildren<Button>();
        rollButton.onClick.AddListener(OnRollClicked);
    }

    private void OnRollClicked()
    {
        int roll1 = int.Parse(rollInput1.text);
        int roll2 = int.Parse(rollInput2.text);
        controller.RollDice(roll1, roll2);
    }
}
