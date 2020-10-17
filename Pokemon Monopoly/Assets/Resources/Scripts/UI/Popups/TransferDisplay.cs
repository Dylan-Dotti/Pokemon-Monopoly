using UnityEngine;

public class TransferDisplay : MonoBehaviour
{
    private Transform leftContent;
    private Transform centerContent;
    private Transform rightContent;

    public Transform LeftContent
    {
        set => SetContent(leftContent, value);
    }

    public Transform CenterContent
    {
        set => SetContent(centerContent, value);
    }

    public Transform RightContent
    {
        set => SetContent(rightContent, value);
    }

    private void Awake()
    {
        leftContent = transform.Find("Left Content");
        centerContent = transform.Find("Center Content");
        rightContent = transform.Find("Right Content");
    }

    private void SetContent(Transform content, Transform value)
    {
        foreach (Transform child in content.GetChildren())
            Destroy(child.gameObject);
        value.position = content.position;
        value.parent = content;
        if (content == leftContent)
            value.localPosition += Vector3.right * 55;
        else if (content == rightContent)
            value.localPosition += Vector3.left * 55;
    }
}
