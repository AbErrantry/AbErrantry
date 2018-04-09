using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class CharacterInventory : MonoBehaviour
	{
		private NPC npc;
		public List<InventoryItem> items; //items held by the character

		public static event Action<ItemTuple, string> OnCharacterItemChanged;

		//used for initialization
		protected void Start()
		{
			items = new List<InventoryItem>();
			npc = GetComponent<NPC>();
			InitializeInventory();
		}

		private void InitializeInventory()
		{
			var savedItems = new List<ItemTuple>();
			savedItems = GameData.data.saveData.ReadCharacterItems(npc.name);
			foreach (ItemTuple item in savedItems)
			{
				for (int index = 0; index < item.quantity; index++)
				{
					AddItem(item.name, true);
				}
			}
		}

		public void AddItem(string itemName, bool init = false)
		{
			bool found = false;
			var itemTuple = new ItemTuple();
			itemTuple.name = itemName;
			itemTuple.quantity = 0;

			foreach (var item in items)
			{
				if (item.item == GameData.data.itemData.itemDictionary[itemName])
				{
					item.quantity++;
					itemTuple.quantity = item.quantity;
					found = true;
					break;
				}
			}

			if (!found)
			{
				var itemToAdd = new InventoryItem
				{
					item = GameData.data.itemData.itemDictionary[itemName],
						quantity = 1
				};
				itemTuple.quantity = itemToAdd.quantity;
				items.Add(itemToAdd);
			}

			if (!init)
			{
				OnCharacterItemChanged(itemTuple, npc.name);
			}
		}

		public void RemoveItem(InventoryItem itemToRemove, bool removeAll)
		{
			int amountLeft = 0;
			if (removeAll)
			{
				amountLeft = 0;
				items.Remove(itemToRemove);
			}
			else
			{
				itemToRemove.quantity--;
				amountLeft = itemToRemove.quantity;
				if (itemToRemove.quantity <= 0)
				{
					items.Remove(itemToRemove);
				}
			}

			var itemTuple = new ItemTuple();
			itemTuple.name = itemToRemove.item.name;
			itemTuple.quantity = amountLeft;
			OnCharacterItemChanged(itemTuple, npc.name);
		}
	}
}
