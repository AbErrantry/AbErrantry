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
        private PlayerInventory playerInventory;
        private PlayerQuests playerQuests;

        public Animator anim;
        public CameraShift cameraShift;
        public GameObject backpackContainer;
        private RectTransform backpackTransform;

        private float xMinLeft;
        private float xMaxLeft;
        private float xMinRight;
        private float xMaxRight;

        public GameObject inventoryContainer;
        public GameObject journalContainer;
        public GameObject mapContainer;

        public GameObject inventoryTab;
        public GameObject journalTab;
        public GameObject mapTab;

        public Image equippedArmorImage;
        public Image equippedWeaponImage;

        public TMP_Text equippedArmorText;
        public TMP_Text equippedWeaponText;

        public GameObject inventoryItem;
        public GameObject inventoryList;
        public GameObject inventoryMask;
        public GameObject descriptionMask;
        public GameObject amountMask;
        public GameObject confirmMask;
        public GameObject amountContainer;
        public GameObject confirmContainer;

        public GameObject questPrefab;
        public GameObject locationPrefab;

        public GameObject journalList;
        public GameObject mapList;

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
        public Button cancelButton;

        public Button amountCancelButton;
        public Button confirmNoButton;

        public InventoryItem selectedItem;
        private int selectedItemIndex;

        public bool isDestroying;
        public bool isAll;
        public bool isOne;
        public bool isOpen;

        public Sprite currentTabSprite;
        public Sprite nonCurrentTabSprite;

        private Navigation automaticNav;
        private Navigation horizontalNav;
        private Navigation noNav;

        private Navigation firstElementNavigation;
        private Navigation middleElementNavigation;
        private Navigation lastElementNavigation;

        public CanvasGroup canvasGroup;

        //used for initialization
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInventory = GetComponent<PlayerInventory>();
            playerQuests = GetComponent<PlayerQuests>();

            backpackContainer.SetActive(false);
            CloseTabs();
            isOpen = false;
            backpackTransform = backpackContainer.GetComponent<RectTransform>();

            xMinLeft = 0.007f;
            xMaxLeft = 0.75f;
            xMinRight = 0.25f;
            xMaxRight = 0.993f;

            automaticNav = new Navigation();
            horizontalNav = new Navigation();
            noNav = new Navigation();

            automaticNav.mode = Navigation.Mode.Automatic;
            horizontalNav.mode = Navigation.Mode.Horizontal;
            noNav.mode = Navigation.Mode.None;

            selectedItemIndex = 0;

            firstElementNavigation = new Navigation();
            middleElementNavigation = new Navigation();
            lastElementNavigation = new Navigation();

            firstElementNavigation.mode = Navigation.Mode.Explicit;
            middleElementNavigation.mode = Navigation.Mode.Explicit;
            lastElementNavigation.mode = Navigation.Mode.Explicit;
        }

        public void ToggleBackpack()
        {
            if (!isOpen)
            {
                StopCoroutine();
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

                RefreshEquippedItems();

                playerInput.DisableInput(false);

                //TODO: move camera to side
                backpackContainer.SetActive(true);
                backpackContainer.GetComponent<Animator>().SetBool("IsOpen", true);

                //move the scrollbar back to the top of the list
                scrollRect.verticalNormalizedPosition = 1.0f;

                LoadInventoryItems(initOpen: true);
                OpenInventoryTab();

                isOpen = true;

                canvasGroup.interactable = true;
            }
            else
            {
                //TODO: move camera back
                CloseBackpackMenu();
            }
        }

        public void CloseBackpackMenu()
        {
            isOpen = false;
            Player.instance.ResetState();
            cameraShift.ResetCamera();
            playerInput.EnableInput(true);
            canvasGroup.interactable = false;
            StartCoroutine(WaitForClose());
            ElementFocus.focus.RemoveFocus();
            backpackContainer.GetComponent<Animator>().SetBool("IsOpen", false);
        }

        private IEnumerator WaitForClose()
        {
            yield return new WaitForSeconds(0.5f);
            backpackContainer.SetActive(false);
            CloseTabs();
            UnloadInventoryItems();
        }

        private void StopCoroutine()
        {
            StopAllCoroutines();
            backpackContainer.SetActive(false);
            CloseTabs();
            UnloadInventoryItems();
        }

        private void CloseTabs()
        {
            inventoryContainer.SetActive(false);

            inventoryTab.GetComponent<Image>().sprite = nonCurrentTabSprite;
            journalTab.GetComponent<Image>().sprite = nonCurrentTabSprite;
            mapTab.GetComponent<Image>().sprite = nonCurrentTabSprite;

            inventoryTab.GetComponent<Button>().navigation = horizontalNav;
            journalTab.GetComponent<Button>().navigation = horizontalNav;
            mapTab.GetComponent<Button>().navigation = horizontalNav;

            HideAmountConfirmContainers(false);
            journalContainer.SetActive(false);
            mapContainer.SetActive(false);
        }

        public void OpenInventoryTab()
        {
            CloseTabs();
            inventoryContainer.SetActive(true);
            inventoryTab.GetComponent<Image>().sprite = currentTabSprite;
            inventoryTab.GetComponent<Button>().navigation = automaticNav;
            FocusOnInventoryMenu();
        }

        public void OpenJournalTab()
        {
            CloseTabs();
            journalContainer.SetActive(true);
            journalTab.GetComponent<Image>().sprite = currentTabSprite;
            journalTab.GetComponent<Button>().navigation = automaticNav;
        }

        public void OpenMapTab()
        {
            CloseTabs();
            mapContainer.SetActive(true);
            mapTab.GetComponent<Image>().sprite = currentTabSprite;
            mapTab.GetComponent<Button>().navigation = automaticNav;
        }

        private void LoadInventoryItems(bool initOpen = false)
        {
            foreach (InventoryItem inv in playerInventory.items)
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
                controller.itemImage.material = inv.item.material;
                controller.item = inv; //TODO: may not need. figure that out.

                //put the interactable in the list
                newButton.transform.SetParent(inventoryList.transform);

                //for some reason Unity does not use full scale for the instantiated object by default
                newButton.transform.localScale = Vector3.one;
            }
            StartCoroutine(SetUpNavigation());
            if (!initOpen)
            {
                FocusOnInventoryItem();
            }
        }

        private void LoadQuests()
        {
            foreach (var quest in playerQuests.quests)
            {
                if (quest.step > 0)
                {
                    //TODO: fix comments
                    //instantiate a prefab for the interact button
                    GameObject newButton = Instantiate(questPrefab) as GameObject;
                    JournalPrefabReference controller = newButton.GetComponent<JournalPrefabReference>();

                    //set the text for the interactable onscreen
                    controller.quest = quest;
                    controller.questName.text = quest.quest.name;
                    controller.questHint.text = quest.quest.segments[quest.step].hint;
                    controller.questStep.text = quest.quest.segments[quest.step].text;
                    controller.questText.text = quest.quest.text;

                    //put the interactable in the list
                    newButton.transform.SetParent(journalList.transform);

                    //for some reason Unity does not use full scale for the instantiated object by default
                    newButton.transform.localScale = Vector3.one;
                }
            }
        }

        private void LoadLocations()
        {

        }

        private IEnumerator SetUpNavigation()
        {
            while (playerInventory.items.Count != inventoryList.transform.childCount)
            {
                yield return null;
            }
            for (int index = 0; index < inventoryList.transform.childCount; index++)
            {
                if (inventoryList.transform.childCount == 1)
                {
                    firstElementNavigation.selectOnUp = inventoryTab.GetComponent<Button>();
                    inventoryList.transform.GetChild(index).GetComponent<Button>().navigation = firstElementNavigation;
                }
                else if (index == 0)
                {
                    firstElementNavigation.selectOnUp = inventoryTab.GetComponent<Button>();
                    firstElementNavigation.selectOnDown = inventoryList.transform.GetChild(index + 1).GetComponent<Button>();
                    inventoryList.transform.GetChild(index).GetComponent<Button>().navigation = firstElementNavigation;
                }
                else if (index == inventoryList.transform.childCount - 1)
                {
                    lastElementNavigation.selectOnUp = inventoryList.transform.GetChild(index - 1).GetComponent<Button>();
                    inventoryList.transform.GetChild(index).GetComponent<Button>().navigation = lastElementNavigation;
                }
                else
                {
                    middleElementNavigation.selectOnUp = inventoryList.transform.GetChild(index - 1).GetComponent<Button>();
                    middleElementNavigation.selectOnDown = inventoryList.transform.GetChild(index + 1).GetComponent<Button>();
                    inventoryList.transform.GetChild(index).GetComponent<Button>().navigation = middleElementNavigation;
                }
            }
        }

        private void MaskDescription()
        {
            useButton.interactable = false;
            dropButton.interactable = false;
            destroyButton.interactable = false;
            cancelButton.interactable = false;
            descriptionMask.SetActive(true);
        }

        public void FocusOnInventoryMenu()
        {
            StartCoroutine(FocusOnInventoryMenuRoutine());
        }

        public void FocusOnInventoryItem()
        {
            StartCoroutine(FocusOnInventoryItemRoutine());
        }

        public IEnumerator FocusOnInventoryMenuRoutine()
        {
            yield return new WaitForEndOfFrame();
            if (playerInventory.items.Count > 0)
            {
                inventoryMask.SetActive(false);
                ElementFocus.focus.SetMenuFocus(inventoryList.transform.GetChild(0).gameObject, scrollRect, inventoryList.GetComponent<RectTransform>());
            }
            else
            {
                inventoryMask.SetActive(true);
                ElementFocus.focus.SetMenuFocus(inventoryTab, scrollRect, inventoryList.GetComponent<RectTransform>());
            }
            MaskDescription();
        }

        public IEnumerator FocusOnInventoryItemRoutine()
        {
            yield return new WaitForEndOfFrame();
            if (playerInventory.items.Count > 0)
            {
                inventoryMask.SetActive(false);
                ElementFocus.focus.SetItemFocus(inventoryList.transform.GetChild(selectedItemIndex).gameObject);
            }
            else
            {
                inventoryMask.SetActive(true);
                ElementFocus.focus.SetItemFocus(inventoryTab);
            }
            MaskDescription();
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
            itemImage.material = inv.item.material;
            itemType.text = inv.item.type;
            itemDescription.text = inv.item.description;
            itemQuantity.text = inv.quantity.ToString();
            itemStrength.text = inv.item.strength.ToString();
            itemPrice.text = inv.item.price.ToString();
            selectedItem = inv;
            descriptionMask.SetActive(false);
            if (inv.item.type != "Story")
            {
                cancelButton.navigation = horizontalNav;
                useButton.interactable = true;
                dropButton.interactable = true;
                destroyButton.interactable = true;
            }
            else
            {
                cancelButton.navigation = noNav;
            }
            for (int index = 0; index < inventoryList.transform.childCount; index++)
            {
                if (inventoryList.transform.GetChild(index).GetComponent<InventoryPrefabReference>().itemText.text == inv.item.name)
                {
                    selectedItemIndex = index;
                }
            }
            cancelButton.interactable = true;
            ElementFocus.focus.SetItemFocus(cancelButton.gameObject);
        }

        public void SelectQuest(QuestInstance instance)
        {
            //List of quests. selecting one sets it as the active quest and provides more information
        }

        public void SelectLocation(string locationName)
        {
            //popup: are you sure that you want to fast travel here?
            //yes-> travel, no->cancel
        }

        public void HideAmountConfirmContainers(bool isInMenu = true)
        {
            amountMask.SetActive(false);
            amountContainer.SetActive(false);
            confirmMask.SetActive(false);
            confirmContainer.SetActive(false);
            isDestroying = false;
            isAll = false;
            isOne = false;
            if (isInMenu)
            {
                FocusOnInventoryItem();
            }
        }

        public void ShowAmountContainer(bool destroying)
        {
            isDestroying = destroying;
            amountMask.SetActive(true);
            amountContainer.SetActive(true);
            ElementFocus.focus.SetItemFocus(amountCancelButton.gameObject);
        }

        public void ShowConfirmContainer()
        {
            confirmMask.SetActive(true);
            confirmContainer.SetActive(true);
            ElementFocus.focus.SetItemFocus(confirmNoButton.gameObject);
        }

        public void RefreshEquippedItems()
        {
            equippedArmorImage.sprite = Player.instance.armor.sprite;
            equippedWeaponImage.sprite = Player.instance.weapon.sprite;

            equippedArmorImage.material = Player.instance.armor.material;
            equippedWeaponImage.material = Player.instance.weapon.material;

            equippedArmorText.text = Player.instance.armor.name;
            equippedWeaponText.text = Player.instance.weapon.name;
        }

        public void UseItem()
        {
            if (selectedItem.item.type == "Weapon")
            {
                Player.instance.SetWeapon(selectedItem.item.name);
                RefreshEquippedItems();
                playerInventory.RemoveItem(selectedItem, false, false, false, true, false);
            }
            else if (selectedItem.item.type == "Armor")
            {
                Player.instance.SetArmor(selectedItem.item.name);
                RefreshEquippedItems();
                playerInventory.RemoveItem(selectedItem, false, false, false, true, false);
            }
            else if (selectedItem.item.type == "Consumable")
            {
                Player.instance.Heal(Mathf.RoundToInt(selectedItem.item.strength));
                ParticleManager.instance.SpawnParticle(gameObject, "health");
                playerInventory.RemoveItem(selectedItem, false, false, false, false, true);
            }
            else
            {
                Debug.LogError("Unidentified item type being used for item: " + selectedItem.item.name + ": " + selectedItem.item.type);
            }

            if (selectedItem.quantity == 0)
            {
                //move the scrollbar back to the top of the list since the item ran out
                scrollRect.verticalNormalizedPosition = 1.0f;
                selectedItemIndex = 0;
            }

            UnloadInventoryItems();
            LoadInventoryItems();
        }

        public void DropItem()
        {
            if (selectedItem.quantity == 1 || isAll)
            {
                //move the scrollbar back to the top of the list since the item ran out
                scrollRect.verticalNormalizedPosition = 1.0f;
                selectedItemIndex = 0;
            }
            playerInventory.RemoveItem(selectedItem, isAll, true);
            UnloadInventoryItems();
            LoadInventoryItems();
            HideAmountConfirmContainers();
        }

        public void DestroyItem()
        {
            if (selectedItem.quantity == 1 || isAll)
            {
                //move the scrollbar back to the top of the list since the item ran out
                scrollRect.verticalNormalizedPosition = 1.0f;
                selectedItemIndex = 0;
            }
            playerInventory.RemoveItem(selectedItem, isAll, false);
            UnloadInventoryItems();
            LoadInventoryItems();
            HideAmountConfirmContainers();
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
