using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
    public class BackpackMenu : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerInteraction playerInteraction;
        public Animator anim;
        public CharacterInventory characterInventory;
        public CameraShift cameraShift;
        public GameObject backpackContainer;
        private RectTransform backpackTransform;

        public float xMinLeft;
        public float xMaxLeft;
        public float xMinRight;
        public float xMaxRight;

        public GameObject inventoryContainer;
        public GameObject journalContainer;
        public GameObject mapContainer;
        public GameObject inventoryTab;
        public GameObject inventoryItem;
        public GameObject inventoryList;
        public GameObject inventoryMask;
        public GameObject descriptionMask;
        public GameObject amountMask;
        public GameObject confirmMask;
        public GameObject amountContainer;
        public GameObject confirmContainer;

        public ScrollRect scrollRect;

        public Image itemImage;

        public TMP_Text itemText;
        public TMP_Text itemType;
        public TMP_Text itemDescription;
        public TMP_Text itemQuantity;
        public TMP_Text itemStrength;
        public TMP_Text itemPrice;

        public Button useButton;
        public Button dropButton;
        public Button destroyButton;

        public InventoryItem selectedItem;

        public bool isDestroying;
        public bool isAll;
        public bool isOne;
        public bool isOpen;

        //used for initialization
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInteraction = GetComponent<PlayerInteraction>();
            backpackContainer.SetActive(false);
            CloseTabs();
            isOpen = false;
            backpackTransform = backpackContainer.GetComponent<RectTransform>();
            xMinLeft = 0.01f;
            xMaxLeft = 0.75f;
            xMinRight = 0.25f;
            xMaxRight = 0.99f;
        }

        public void ToggleBackpack()
        {
            if (!isOpen)
            {
                playerInteraction.CloseContainer();
                if (cameraShift.ShiftCameraLeft(true))
                {
                    backpackTransform.anchorMin = new Vector2(xMinLeft, backpackTransform.anchorMin.y);
                    backpackTransform.anchorMax = new Vector2(xMaxLeft, backpackTransform.anchorMax.y);
                }
                else
                {
                    backpackTransform.anchorMin = new Vector2(xMinRight, backpackTransform.anchorMin.y);
                    backpackTransform.anchorMax = new Vector2(xMaxRight, backpackTransform.anchorMax.y);
                }

                playerInput.DisableInput(false);

                //TODO: move camera to side
                backpackContainer.SetActive(true);
                LoadInventoryItems();
                OpenInventoryTab();
                isOpen = true;

                //move the scrollbar back to the top of the list
                scrollRect.verticalNormalizedPosition = 1.0f;
            }
            else
            {
                //TODO: move camera back
                CloseBackpackMenu();
                isOpen = false;
            }
        }

        private void CloseTabs()
        {
            inventoryContainer.SetActive(false);
            HideAmountConfirmContainers();
            journalContainer.SetActive(false);
            mapContainer.SetActive(false);
        }

        public void OpenInventoryTab()
        {
            CloseTabs();
            inventoryContainer.SetActive(true);
        }

        public void OpenJournalTab()
        {
            CloseTabs();
            journalContainer.SetActive(true);
        }

        public void OpenMapTab()
        {
            CloseTabs();
            mapContainer.SetActive(true);
        }

        public void CloseBackpackMenu()
        {
            cameraShift.ResetCamera();
            playerInput.EnableInput();
            CloseTabs();
            UnloadInventoryItems();
            backpackContainer.SetActive(false);
            isOpen = false;
            Time.timeScale = 1.0f;
        }

        private void LoadInventoryItems()
        {
            foreach (InventoryItem inv in characterInventory.Items)
            {
                //TODO: fix comments
                //instantiate a prefab for the interact button
                GameObject newButton = Instantiate(inventoryItem)as GameObject;
                InventoryPrefabReference controller = newButton.GetComponent<InventoryPrefabReference>();

                //set the text for the interactable onscreen
                controller.itemText.text = inv.item.name;
                controller.itemQuantity.text = inv.quantity.ToString();
                controller.itemPrice.text = inv.item.price.ToString();
                controller.itemStrength.text = inv.item.strength.ToString();
                controller.itemImage.sprite = inv.item.sprite;
                controller.item = inv; //TODO: may not need. figure that out.

                //put the interactable in the list
                newButton.transform.SetParent(inventoryList.transform);

                //for some reason Unity does not use full scale for the instantiated object by default
                newButton.transform.localScale = Vector3.one;
            }

            if (characterInventory.Items.Count > 0)
            {
                inventoryMask.SetActive(false);
                ElementFocus.focus.SetFocus(inventoryList.transform.GetChild(0).gameObject, scrollRect, inventoryList.GetComponent<RectTransform>());
            }
            else
            {
                inventoryMask.SetActive(true);
                ElementFocus.focus.SetFocus(inventoryTab, scrollRect, inventoryList.GetComponent<RectTransform>());
            }
            descriptionMask.SetActive(true);
        }

        //delete ui elements from the list for the next iteration
        private void UnloadInventoryItems()
        {
            var children = new List<GameObject>();
            foreach (Transform child in inventoryList.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));
            ElementFocus.focus.RemoveFocus();
        }

        public void SelectItem(InventoryItem inv)
        {
            itemText.text = inv.item.name;
            itemImage.sprite = inv.item.sprite;
            itemType.text = inv.item.type;
            itemDescription.text = inv.item.description;
            itemQuantity.text = inv.quantity.ToString();
            itemStrength.text = inv.item.strength.ToString();
            itemPrice.text = inv.item.price.ToString();
            selectedItem = inv;
            descriptionMask.SetActive(false);
            if (inv.item.type == "story")
            {
                useButton.interactable = false;
                dropButton.interactable = false;
                destroyButton.interactable = false;
            }
            else
            {
                useButton.interactable = true;
                dropButton.interactable = true;
                destroyButton.interactable = true;
            }
        }

        public void HideAmountConfirmContainers()
        {
            amountMask.SetActive(false);
            amountContainer.SetActive(false);
            confirmMask.SetActive(false);
            confirmContainer.SetActive(false);
            isDestroying = false;
            isAll = false;
            isOne = false;
        }

        public void ShowAmountContainer(bool destroying)
        {
            isDestroying = destroying;
            amountMask.SetActive(true);
            amountContainer.SetActive(true);
        }

        public void ShowConfirmContainer()
        {
            confirmMask.SetActive(true);
            confirmContainer.SetActive(true);
        }

        public void UseItem()
        {
            if (selectedItem.item.type == "weapon")
            {
                Debug.Log("Equipped " + selectedItem.item.name + ".");
                //need to also unequip the currently-equipped item and add it back to inventory.
            }
            else if (selectedItem.item.type == "consumable")
            {
                Debug.Log("Heal player for " + selectedItem.item.strength + " health points.");
                anim.Play("large-health");
            }
            else
            {
                Debug.Log("Should not have gotten here.");
            }

            if (selectedItem.quantity == 1)
            {
                //move the scrollbar back to the top of the list
                scrollRect.verticalNormalizedPosition = 1.0f;
            }
            characterInventory.RemoveItem(selectedItem, false, false);
            UnloadInventoryItems();
            LoadInventoryItems();
        }

        public void DropItem()
        {
            characterInventory.RemoveItem(selectedItem, isAll, true);
            UnloadInventoryItems();
            LoadInventoryItems();
            HideAmountConfirmContainers();
            //move the scrollbar back to the top of the list
            scrollRect.verticalNormalizedPosition = 1.0f;
        }

        public void DestroyItem()
        {
            characterInventory.RemoveItem(selectedItem, isAll, false);
            UnloadInventoryItems();
            LoadInventoryItems();
            HideAmountConfirmContainers();
            //move the scrollbar back to the top of the list
            scrollRect.verticalNormalizedPosition = 1.0f;
        }

        public void SetAmount(bool all)
        {
            isAll = all;
            if (isDestroying)
            {
                ShowConfirmContainer();
            }
            else
            {
                DropItem();
            }
        }
    }
}
