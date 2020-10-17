using UnityEngine;

[ExecuteAlways]
public class UIArrow : MonoBehaviour
{
    private RectTransform body;
    private RectTransform head;

    private void Awake()
    {
        body = transform.Find("Graphics").Find("Body").GetComponent<RectTransform>();
        head = transform.Find("Graphics").Find("Head").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (transform.hasChanged || body.hasChanged || head.hasChanged)
        {
            float bodyWidth = body.rect.width;
            float headWidth = head.rect.width;
            Vector2 baseBodyPos = Vector2.left * bodyWidth / 2;
            Vector2 baseHeadPos = Vector2.right * headWidth / 2;
            float distBetweenCenters = Vector2.Distance(baseBodyPos, baseHeadPos);
            Vector2 leftPoint = baseBodyPos + Vector2.left * bodyWidth / 2;
            Vector2 rightPoint = baseHeadPos + Vector2.right * headWidth / 2;
            Vector2 arrowCenter = (leftPoint + rightPoint) * Vector2.right / 2;
            body.localPosition = (baseBodyPos - arrowCenter) * 0.99f;
            head.localPosition = baseHeadPos - arrowCenter;
        }
    }
}
