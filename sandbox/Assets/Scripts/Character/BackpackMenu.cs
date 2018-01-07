using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Character2D
{
    public class BackpackMenu : MonoBehaviour
    {
        public GameObject backpackContainer;

        public GameObject inventoryContainer;
        public GameObject journalContainer;
        public GameObject mapContainer;

        public CharacterBehavior characterBehavior;

        public GameObject inventoryItem;
        public GameObject inventoryList;

        public ScrollRect scrollRect;

        public GameObject inventoryMask;
        public GameObject descriptionMask;

        public TMP_Text itemText;
        public Image itemImage;
        public TMP_Text itemType;
        public TMP_Text itemDescription;
        public TMP_Text itemQuantity;
        public TMP_Text itemStrength;
        public TMP_Text itemPrice;

        public Button useButton;
        public Button dropButton;
        public Button destroyButton;

        public InventoryItem selectedItem;

        public GameObject amountMask;
        public GameObject confirmMask;

        public GameObject amountContainer;
        public GameObject confirmContainer;

        public bool isDestroying;
        public bool isAll;
        public bool isOne;

        private bool isOpen;

        //used for initialization
        private void Start()
        {
            backpackContainer.SetActive(false);
            CloseTabs();
            isOpen = false;
        }

        public void ToggleBackpack()
        {
            if (!isOpen)
            {
                backpackContainer.SetActive(true);
                LoadInventoryItems();
                OpenInventoryTab();
                isOpen = true;
                Time.timeScale = 0.0f;

                //move the scrollbar back to the top of the list
                scrollRect.verticalNormalizedPosition = 1.0f;
            }
            else
            {
                CloseBackpackMenu();
                isOpen = false;
                Time.timeScale = 1.0f;
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
            CloseTabs();
            UnloadInventoryItems();
            backpackContainer.SetActive(false);
            isOpen = false;
            Time.timeScale = 1.0f;
        }

        private void LoadInventoryItems()
        {
            foreach (InventoryItem inv in characterBehavior.items)
            {
                //TODO: fix comments
                //instantiate a prefab for the interact button
                GameObject newButton = Instantiate(inventoryItem) as GameObject;
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

            if(characterBehavior.items.Count > 0)
            {
                inventoryMask.SetActive(false);
            }
            else
            {
                inventoryMask.SetActive(true);
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
            if(inv.item.type == "story")
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
            if(selectedItem.item.type == "weapon")
            {
                Debug.Log("Equipped " + selectedItem.item.name + ".");
                //need to also unequip the currently-equipped item and add it back to inventory.
            }
            else if(selectedItem.item.type == "consumable")
            {
                Debug.Log("Heal player for " + selectedItem.item.strength + " health points.");
            }
            else
            {
                Debug.Log("Should not have gotten here.");
            }
            if(selectedItem.quantity == 1)
            {
                //move the scrollbar back to the top of the list
                scrollRect.verticalNormalizedPosition = 1.0f;
            }
            characterBehavior.RemoveItem(selectedItem, false, false);
            UnloadInventoryItems();
            LoadInventoryItems();
            
        }

        public void DropItem()
        {
            characterBehavior.RemoveItem(selectedItem, isAll, true);
            UnloadInventoryItems();
            LoadInventoryItems();
            HideAmountConfirmContainers();
            //move the scrollbar back to the top of the list
            scrollRect.verticalNormalizedPosition = 1.0f;
        }

        public void DestroyItem()
        {
            characterBehavior.RemoveItem(selectedItem, isAll, false);
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
