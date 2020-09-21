using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectionPanel : MonoBehaviour
{
    private List<AvatarSelectionGridItem> gridItems;
    private AvatarSelectionGridItem selectedItem;
    private Transform highlightOutliner;
    private Transform selectionOutliner;
    private Button doneButton;

    private void Awake()
    {
        Transform panelContent = transform.Find("Panel Content");
        gridItems = new List<AvatarSelectionGridItem>(panelContent
            .Find("Avatar Panel Grid").GetComponentsInChildren<AvatarSelectionGridItem>());
        highlightOutliner = panelContent.Find("Highlight Outliner");
        selectionOutliner = panelContent.Find("Selection Outliner");
        doneButton = panelContent.GetComponentInChildren<Button>();
        doneButton.onClick.AddListener(Close);
        foreach (var item in gridItems)
        {
            item.MouseEntered += OnItemMouseEnter;
            item.MouseExited += OnItemMouseExit;
            item.MouseClicked += OnItemMouseClick;
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        highlightOutliner.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnItemMouseClick(AvatarSelectionGridItem item)
    {
        selectedItem = item;
        highlightOutliner.gameObject.SetActive(false);
        selectionOutliner.gameObject.SetActive(true);
        doneButton.interactable = true;
        selectionOutliner.transform.position = item.transform.position;
        MultiplayerSettings.Instance.AvatarImageName = item.AvatarImagePrefab.name;
    }

    private void OnItemMouseExit(AvatarSelectionGridItem item)
    {
        highlightOutliner.gameObject.SetActive(false);
    }

    private void OnItemMouseEnter(AvatarSelectionGridItem item)
    {
        if (item != selectedItem)
        {
            highlightOutliner.gameObject.SetActive(true);
            highlightOutliner.transform.position = item.transform.position;
        }
    }
}
