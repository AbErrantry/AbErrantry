using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementFocus : MonoBehaviour
{
    public static ElementFocus focus;
    private EventSystem eventSystem;
    private RectTransform selectedItem;
    private ScrollRect scrollRect;
    private RectTransform contentPanel;

    // Use this for initialization
    private void Start()
    {
        if (focus == null)
        {
            focus = this;
            eventSystem = GetComponent<EventSystem>();
            selectedItem = null;
        }
        else if (focus != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (selectedItem != null && eventSystem.currentSelectedGameObject != null)
        {
            if (selectedItem.gameObject != eventSystem.currentSelectedGameObject)
            {
                if (eventSystem.currentSelectedGameObject.transform.parent.gameObject == contentPanel.gameObject)
                {
                    selectedItem = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
                    SnapToItem();
                }
            }
        }
    }

    public void SetFocus(GameObject itemToFocus, ScrollRect itemScrollRect, RectTransform itemContentPanel)
    {
        eventSystem.SetSelectedGameObject(itemToFocus);
        selectedItem = itemToFocus.GetComponent<RectTransform>();
        scrollRect = itemScrollRect;
        contentPanel = itemContentPanel;
    }

    public void RemoveFocus()
    {
        selectedItem = null;
    }

    public void SnapToItem()
    {
        int itemIndex = selectedItem.transform.GetSiblingIndex();
        int itemCount = scrollRect.content.transform.childCount - 1;
        scrollRect.verticalNormalizedPosition = (float)(itemCount - itemIndex)/ itemCount;
    }
}
