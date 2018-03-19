using System.Collections;
using System.Collections.Generic;
using Character2D;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
	private PlayerInventory playerInventory;
	private CharacterInventory characterInventory;

	private List<InventoryItem> currentInventory;

	private Dialogue2D.DialogueManager dialogueManager;

	public Animator anim;
	public GameObject storeContainer;

	public GameObject buyTab;
	public GameObject sellTab;

	public GameObject inventoryItem;
	public GameObject inventoryList;
	public GameObject inventoryMask;
	public GameObject descriptionMask;
	public GameObject amountMask;
	public GameObject amountContainer;

	public ScrollRect scrollRect;

	public Image itemImage;

	public TMP_Text goldText;

	public TMP_Text itemText;
	public TMP_Text itemType;
	public TMP_Text itemDescription;
	public TMP_Text itemQuantity;
	public TMP_Text itemStrength;
	public TMP_Text itemPrice;

	public Button confirmButton;
	public TMP_Text confirmButtonText;
	public Button cancelButton;

	public Button amountCancelButton;

	public InventoryItem selectedItem;

	private int selectedItemIndex;

	public bool isOpen;

	public Sprite currentTabSprite;
	public Sprite nonCurrentTabSprite;

	private Navigation automaticNav;
	private Navigation horizontalNav;
	private Navigation noNav;

	public bool isSelling;

	public Image goldImage;
	public Image containerImage;

	private Color purpleColor;
	private Color blueColor;

	private Navigation firstElementNavigation;
	private Navigation middleElementNavigation;
	private Navigation lastElementNavigation;

	//used for initialization
	private void Start()
	{
		playerInventory = GetComponent<PlayerInventory>();
		dialogueManager = GetComponent<Dialogue2D.DialogueManager>();

		storeContainer.SetActive(false);

		CloseTabs();
		isOpen = false;
		isSelling = false;

		automaticNav = new Navigation();
		horizontalNav = new Navigation();
		noNav = new Navigation();

		automaticNav.mode = Navigation.Mode.Automatic;
		horizontalNav.mode = Navigation.Mode.Horizontal;
		noNav.mode = Navigation.Mode.None;

		selectedItemIndex = 0;

		purpleColor = new Color32(166, 2, 202, 255);
		blueColor = new Color32(103, 110, 255, 255);

		firstElementNavigation = new Navigation();
		middleElementNavigation = new Navigation();
		lastElementNavigation = new Navigation();

		firstElementNavigation.mode = Navigation.Mode.Explicit;
		middleElementNavigation.mode = Navigation.Mode.Explicit;
		lastElementNavigation.mode = Navigation.Mode.Explicit;
	}

	public void ToggleStore(CharacterInventory inv)
	{
		characterInventory = inv;
		if (!isOpen)
		{
			//TODO: move camera to side
			storeContainer.SetActive(true);

			//move the scrollbar back to the top of the list
			scrollRect.verticalNormalizedPosition = 1.0f;

			OpenSellTab();
			isOpen = true;
		}
		else
		{
			CloseStoreMenu();
		}
	}

	public void CloseStoreMenu()
	{
		storeContainer.SetActive(false);
		ElementFocus.focus.RemoveFocus();
		CloseTabs();
		UnloadInventoryItems();
		isOpen = false;
		dialogueManager.SetDoneWaiting();
	}

	private void CloseTabs()
	{
		UnloadInventoryItems();

		selectedItemIndex = 0;

		buyTab.GetComponent<Image>().sprite = nonCurrentTabSprite;
		sellTab.GetComponent<Image>().sprite = nonCurrentTabSprite;

		HideAmountConfirmContainers(false);
	}

	public void OpenSellTab()
	{
		CloseTabs();
		isSelling = true;
		confirmButtonText.text = "Sell";
		goldImage.color = blueColor;
		containerImage.color = blueColor;
		inventoryMask.GetComponent<Image>().color = blueColor;
		descriptionMask.GetComponent<Image>().color = blueColor;
		currentInventory = playerInventory.items;
		goldText.text = characterInventory.GetComponent<NPC>().name + "'s gold: " + characterInventory.GetComponent<NPC>().gold.ToString();

		sellTab.GetComponent<Image>().sprite = currentTabSprite;
		LoadInventoryItems();
		FocusOnInventoryMenu();
	}

	public void OpenBuyTab()
	{
		CloseTabs();
		isSelling = false;
		confirmButtonText.text = "Buy";
		goldImage.color = purpleColor;
		containerImage.color = purpleColor;
		inventoryMask.GetComponent<Image>().color = purpleColor;
		descriptionMask.GetComponent<Image>().color = purpleColor;
		currentInventory = characterInventory.items;
		goldText.text = "Your gold: " + Player.instance.gold.ToString();

		buyTab.GetComponent<Image>().sprite = currentTabSprite;
		LoadInventoryItems();
		FocusOnInventoryMenu();
	}

	private void LoadInventoryItems(bool initOpen = false)
	{
		List<InventoryItem> itemList = new List<InventoryItem>();
		if (isSelling)
		{
			itemList = playerInventory.items;
		}
		else
		{
			itemList = characterInventory.items;
		}
		foreach (InventoryItem inv in itemList)
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
		StartCoroutine(SetUpNavigation(itemList));
		if (!initOpen)
		{
			FocusOnInventoryItem();
		}
	}

	private IEnumerator SetUpNavigation(List<InventoryItem> itemList)
	{
		while (itemList.Count != inventoryList.transform.childCount)
		{
			yield return null;
		}
		for (int index = 0; index < inventoryList.transform.childCount; index++)
		{
			if (inventoryList.transform.childCount == 1)
			{
				firstElementNavigation.selectOnUp = sellTab.GetComponent<Button>();
				inventoryList.transform.GetChild(index).GetComponent<Button>().navigation = firstElementNavigation;
			}
			else if (index == 0)
			{
				firstElementNavigation.selectOnUp = sellTab.GetComponent<Button>();
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
		confirmButton.interactable = false;
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
		if (currentInventory.Count > 0)
		{
			inventoryMask.SetActive(false);
			ElementFocus.focus.SetMenuFocus(inventoryList.transform.GetChild(0).gameObject, scrollRect, inventoryList.GetComponent<RectTransform>());
		}
		else
		{
			inventoryMask.SetActive(true);
			ElementFocus.focus.SetMenuFocus(sellTab, scrollRect, inventoryList.GetComponent<RectTransform>());
		}
		MaskDescription();
	}

	public IEnumerator FocusOnInventoryItemRoutine()
	{
		yield return new WaitForEndOfFrame();
		if (currentInventory.Count > 0)
		{
			inventoryMask.SetActive(false);
			ElementFocus.focus.SetItemFocus(inventoryList.transform.GetChild(selectedItemIndex).gameObject);
		}
		else
		{
			inventoryMask.SetActive(true);
			ElementFocus.focus.SetItemFocus(sellTab);
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
		itemType.text = inv.item.type;
		itemDescription.text = inv.item.description;
		itemQuantity.text = inv.quantity.ToString();
		itemStrength.text = inv.item.strength.ToString();
		itemPrice.text = inv.item.price.ToString();
		itemImage.material = inv.item.material;
		selectedItem = inv;
		descriptionMask.SetActive(false);
		if (inv.item.type != "Story" || !isSelling)
		{
			confirmButton.interactable = true;
			cancelButton.navigation = horizontalNav;
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

	public void HideAmountConfirmContainers(bool isInMenu = true)
	{
		amountMask.SetActive(false);
		amountContainer.SetActive(false);
		if (isInMenu)
		{
			FocusOnInventoryItem();
		}
	}

	public void ShowAmountContainer()
	{
		amountMask.SetActive(true);
		amountContainer.SetActive(true);
		ElementFocus.focus.SetItemFocus(amountCancelButton.gameObject);
	}

	public void Confirm(bool isAll)
	{
		int quantity = 1;
		bool failed = false;
		if (isAll)
		{
			quantity = selectedItem.quantity;
		}
		if (isSelling)
		{
			if ((quantity * selectedItem.item.price) > characterInventory.GetComponent<NPC>().gold)
			{
				failed = true;
				EventDisplay.instance.AddEvent(characterInventory.GetComponent<NPC>().name +
					" does not have enough gold for the transaction (Has " +
					characterInventory.GetComponent<NPC>().gold + ", needs " + (quantity * selectedItem.item.price) + ").");
			}
			else
			{
				for (int index = 0; index < quantity; index++)
				{
					characterInventory.AddItem(selectedItem.item.name);
				}
				characterInventory.GetComponent<NPC>().SetGold(-(quantity * selectedItem.item.price));
				playerInventory.RemoveItem(selectedItem, isAll, false, true);
				Player.instance.SetGold((quantity * selectedItem.item.price));
			}
			goldText.text = characterInventory.GetComponent<NPC>().name + "'s gold: " + characterInventory.GetComponent<NPC>().gold.ToString();
		}
		else
		{
			if ((quantity * selectedItem.item.price) > Player.instance.gold)
			{
				failed = true;
				EventDisplay.instance.AddEvent("You do not have enough gold for the transaction (Have " +
					Player.instance.gold + ", need " + (quantity * selectedItem.item.price) + ").");
			}
			else
			{
				for (int index = 0; index < quantity; index++)
				{
					playerInventory.AddItem(selectedItem.item.name);
				}
				Player.instance.SetGold(-(quantity * selectedItem.item.price));
				characterInventory.RemoveItem(selectedItem, isAll);
				characterInventory.GetComponent<NPC>().SetGold((quantity * selectedItem.item.price));
			}
			goldText.text = "Your gold: " + Player.instance.gold.ToString();
		}

		if ((selectedItem.quantity == 0 || isAll) && !failed)
		{
			//move the scrollbar back to the top of the list since the item ran out
			scrollRect.verticalNormalizedPosition = 1.0f;
			selectedItemIndex = 0;
		}
		HideAmountConfirmContainers();
		UnloadInventoryItems();
		LoadInventoryItems();
	}
}
