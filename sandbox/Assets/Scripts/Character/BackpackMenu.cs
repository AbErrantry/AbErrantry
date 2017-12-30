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
        public GameObject inventoryGrid;

        public GameObject inventoryMask;
        public GameObject descriptionMask;

        public TMP_Text itemText;
        public Image itemImage;
        public TMP_Text itemType;
        public TMP_Text itemDescription;
        public TMP_Text itemQuantity;
        public TMP_Text itemStrength;
        public TMP_Text itemPrice;

        public InventoryItem selectedItem;

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

            //delete ui elements from the list for the next iteration
            var children = new List<GameObject>();
            foreach (Transform child in inventoryGrid.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));

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

                controller.itemImage.sprite = inv.item.sprite;

                controller.item = inv; //TODO: may not need. figure that out.

                //put the interactable in the list
                newButton.transform.SetParent(inventoryGrid.transform);

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
        }
    }
}
