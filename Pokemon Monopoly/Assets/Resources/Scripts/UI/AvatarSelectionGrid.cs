using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarSelectionGrid : MonoBehaviour
{
    private List<AvatarSelectionGridItem> gridItems;

    private void Awake()
    {
        gridItems = transform.GetComponentsInChildren<AvatarSelectionGridItem>().ToList();
        foreach(var item in gridItems)
        {
            item.MouseEntered += OnItemMouseEnter;
            item.MouseExited += OnItemMouseExit;
            item.MouseClicked += OnItemMouseClick;
        }
    }

    private void OnItemMouseClick(AvatarSelectionGridItem item)
    {
        throw new System.NotImplementedException();
    }

    private void OnItemMouseExit(AvatarSelectionGridItem item)
    {
        throw new System.NotImplementedException();
    }

    private void OnItemMouseEnter(AvatarSelectionGridItem item)
    {
        throw new System.NotImplementedException();
    }
}
