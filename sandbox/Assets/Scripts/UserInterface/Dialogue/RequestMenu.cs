using System.Collections;
using System.Collections.Generic;
using Character2D;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestMenu : MonoBehaviour
{
	private PlayerInventory playerInventory;
	private Player player;
	private Dialogue2D.DialogueManager dialogueManager;

	public Animator anim;
	public GameObject requestContainer;

	public TMP_Text requestText;

	public bool isOpen;

	private bool isGold;
	private int amount;
	private Item item;
	private string characterName;
	private int successPath;
	private int failurePath;

	public Button YesButton;
	public Button NoButton;

	private Navigation verticalNav;
	private Navigation noNav;

	private void Start()
	{
		playerInventory = GetComponent<PlayerInventory>();
		dialogueManager = GetComponent<Dialogue2D.DialogueManager>();
		player = GetComponent<Player>();

		requestContainer.SetActive(false);

		isOpen = false;
		isGold = false;

		verticalNav = new Navigation();
		noNav = new Navigation();

		verticalNav.mode = Navigation.Mode.Vertical;
		noNav.mode = Navigation.Mode.None;
	}

	public void ToggleRequest(int requestSuccessPath, int requestFailurePath, string requestingCharacterName,
		bool requestIsGold, int requestAmount, string requestedItemName = null)
	{
		YesButton.interactable = false;
		NoButton.navigation = noNav;
		if (!isOpen)
		{
			requestContainer.SetActive(true);
			isOpen = true;
			Player.instance.PlayOpenMenuNoise();
			ElementFocus.focus.SetItemFocus(NoButton.gameObject);
			SetUpRequest(requestSuccessPath, requestFailurePath, requestingCharacterName, requestIsGold, requestAmount, requestedItemName);
		}
		else
		{
			CloseRequestMenu(-1);
		}
	}

	public void CloseRequestMenu(int result)
	{
		Player.instance.PlayCloseMenuNoise();
		requestContainer.SetActive(false);
		ElementFocus.focus.RemoveFocus();
		isOpen = false;
		dialogueManager.SetDoneWaiting(true, result);
	}

	public void ChooseYes()
	{
		if (isGold)
		{
			player.SetGold(-amount);
		}
		else
		{
			InventoryItem invItem = playerInventory.GetInventoryItem(item.name);
			for (int index = 0; index < amount; index++)
			{
				playerInventory.RemoveItem(invItem, removeAll : false, drop : false, isTransaction : false, equip : false, use : false, give : true);
			}
		}
		CloseRequestMenu(successPath);
	}

	public void ChooseNo()
	{
		CloseRequestMenu(failurePath);
	}

	private void SetUpRequest(int requestSuccessPath, int requestFailurePath, string requestingCharacterName,
		bool requestIsGold, int requestAmount, string requestedItemName = null)
	{
		successPath = requestSuccessPath;
		failurePath = requestFailurePath;

		characterName = requestingCharacterName;
		isGold = requestIsGold;
		amount = requestAmount;
		if (!isGold)
		{
			item = GameData.data.itemData.itemDictionary[requestedItemName];
			requestText.text = characterName + " requests " + amount + " " + item.name + "(s).";
			if (playerInventory.CheckForItem(item.name, amount))
			{
				YesButton.interactable = true;
				NoButton.navigation = verticalNav;
			}
		}
		else
		{
			item = null;
			requestText.text = characterName + " requests " + amount + " gold.";
			if (player.gold >= amount)
			{
				YesButton.interactable = true;
				NoButton.navigation = verticalNav;
			}
		}
	}
}
