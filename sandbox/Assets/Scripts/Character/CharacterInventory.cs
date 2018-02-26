using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterInventory : MonoBehaviour
    {
        public GameObject PickupPrefab;
        public List<InventoryItem> Items; //items held by the character

        //public static event Action<int, bool, string[], Vector2> OnLooseItemChanged; //todo: set loose items

        //used for initialization
        protected void Start()
        {
            Items = new List<InventoryItem>();
        }

        public void AddItem(string itemName)
        {
            foreach (var item in Items)
            {
                if (item.item != GameData.data.itemData.itemDictionary[itemName]) continue;
                item.quantity++;
                return;
            }

            var itemToAdd = new InventoryItem
            {
                item = GameData.data.itemData.itemDictionary[itemName],
                quantity = 1
            };
            Items.Add(itemToAdd);
        }

        public void RemoveItem(InventoryItem itemToRemove, bool removeAll, bool drop)
        {
            var amountToDrop = 0;
            if (removeAll)
            {
                amountToDrop = itemToRemove.quantity;
                Items.Remove(itemToRemove);
            }
            else
            {
                amountToDrop = 1;
                itemToRemove.quantity--;
                if (itemToRemove.quantity <= 0)
                {
                    Items.Remove(itemToRemove);
                }
            }

            if (!drop) return;
            
            for (var i = 0; i < amountToDrop; i++)
            {
                InstantiateItem(itemToRemove.item, transform.position);
            }
        }

        public void InstantiateItem(Item item, Vector3 pos)
        {
            //instantiate a prefab for the pickup
            var newPickup = Instantiate(PickupPrefab) as GameObject;
            var pickup = newPickup.GetComponent<Pickup>();
            var rend = newPickup.GetComponent<SpriteRenderer>();
            var collider = newPickup.GetComponent<BoxCollider2D>();

            //set the properties for the pickup
            pickup.name = item.name;
            pickup.typeOfInteractable = Interactable.Types.Pickup;
            pickup.SetType();
            rend.sprite = item.sprite;
            newPickup.transform.position = pos;
            collider.size = rend.bounds.size;

            //for some reason Unity does not use full scale for the instantiated object by default
            newPickup.transform.localScale = Vector3.one;
        }
    }
}
