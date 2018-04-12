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

    private void Awake()
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
                if (eventSystem.currentSelectedGameObject.transform.parent.gameObject != null && contentPanel != null)
                {
                    if (eventSystem.currentSelectedGameObject.transform.parent.gameObject == contentPanel.gameObject)
                    {
                        selectedItem = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
                        StopAllCoroutines();
                        SnapToItem();
                    }
                }
            }
        }
        else if (eventSystem.currentSelectedGameObject == null && Selectable.allSelectables.Count > 0)
        {
            if (selectedItem != null)
            {
                StartCoroutine(HighlightFocus(selectedItem.gameObject));
            }
            else
            {
                foreach (Selectable selectableUI in Selectable.allSelectables)
                {
                    if (selectableUI.interactable)
                    {
                        StartCoroutine(HighlightFocus(selectableUI.gameObject));
                        break;
                    }
                }
            }
        }
        else if (eventSystem.currentSelectedGameObject != null && !eventSystem.currentSelectedGameObject.activeInHierarchy)
        {
            RemoveFocus();
        }
    }

    public void SetMenuFocus(GameObject itemToFocus, ScrollRect itemScrollRect, RectTransform itemContentPanel)
    {
        StartCoroutine(HighlightFocus(itemToFocus));
        selectedItem = itemToFocus.GetComponent<RectTransform>();
        scrollRect = itemScrollRect;
        contentPanel = itemContentPanel;
    }

    public void SetItemFocus(GameObject itemToFocus)
    {
        StartCoroutine(HighlightFocus(itemToFocus));
        selectedItem = itemToFocus.GetComponent<RectTransform>();
    }

    public void RemoveFocus()
    {
        selectedItem = null;
        eventSystem.SetSelectedGameObject(null);
    }

    public void SnapToItem()
    {
        int itemIndex = selectedItem.transform.GetSiblingIndex();
        int itemCount = scrollRect.content.transform.childCount - 1;
        StartCoroutine(LerpToPosition((float) (itemCount - itemIndex) / itemCount));
    }

    private IEnumerator LerpToPosition(float finalPos)
    {
        float startTime = Time.time;
        while (Time.time - startTime < 0.25f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.SmoothStep(scrollRect.verticalNormalizedPosition, finalPos, (Time.time - startTime) / 0.25f);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator HighlightFocus(GameObject itemToFocus)
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(itemToFocus);
    }
}
