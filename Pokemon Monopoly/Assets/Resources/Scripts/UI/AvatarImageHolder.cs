using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarImageHolder : MonoBehaviour
{
    public RectTransform AvatarImage
    {
        set
        {
            transform.GetChildren().ForEach(c => Destroy(c.gameObject));
        }
    }
}
