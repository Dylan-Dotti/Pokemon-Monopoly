using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackNextMenuControl : MonoBehaviour
{
    public event UnityAction<int> Back;
    public event UnityAction<int> Next;

    private Button backButton;
    private Button nextButton;
    private Text indexText;

    private int currentIndex = 1;
    private int maxIndex = 1;

    public int MaxIndex
    {
        get => maxIndex;
        set
        {
            maxIndex = value;
            currentIndex = Mathf.Min(currentIndex, maxIndex);
            backButton.interactable = currentIndex > 1;
            nextButton.interactable = currentIndex < maxIndex;
            UpdateIndexText();
        }
    }

    private void Awake()
    {
        backButton = transform.Find("Back Button").GetComponent<Button>();
        backButton.interactable = false;
        backButton.onClick.AddListener(() => TriggerBack());
        nextButton = transform.Find("Next Button").GetComponent<Button>();
        nextButton.interactable = false;
        nextButton.onClick.AddListener(() => TriggerNext());
        indexText = transform.Find("Index Text").GetComponent<Text>();
        UpdateIndexText();
    }

    public void TriggerBack()
    {
        currentIndex = Mathf.Max(1, currentIndex - 1);
        UpdateButtons();
        UpdateIndexText();
        Back?.Invoke(currentIndex);
    }

    public void TriggerNext()
    {
        currentIndex = Mathf.Min(maxIndex, currentIndex + 1);
        UpdateButtons();
        UpdateIndexText();
        Next?.Invoke(currentIndex);
    }

    public void ResetIndex()
    {
        currentIndex = 1;
        UpdateButtons();
        UpdateIndexText();
    }

    private void UpdateIndexText() => 
        indexText.text = $"{currentIndex}/{maxIndex}";

    private void UpdateButtons()
    {
        backButton.interactable = currentIndex > 1;
        nextButton.interactable = currentIndex < maxIndex;
    }
}
