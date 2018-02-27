using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character2D
{
    public class PlayerInventory : MonoBehaviour
    {
        public GameObject PickupPrefab;
        public List<InventoryItem> Items; //items held by the character

        public static event Action<LevelItemTuple, bool, string> OnLooseItemChanged; //todo: set loose items
        public static event Action<ItemTuple> OnInventoryItemChanged; //todo: set loose items

        //used for initialization
        protected void Start()
        {
            Items = new List<InventoryItem>();
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            var savedItems = new List<ItemTuple>();
            savedItems = GameData.data.saveData.ReadPlayerItems();
            foreach (ItemTuple item in savedItems)
            {
                for (int index = 0; index < item.quantity; index++)
                {
                    AddItem(item.name, true);
                }
            }
        }

        private void AddItem(string itemName, bool init = false)
        {
            bool found = false;
            var itemTuple = new ItemTuple();
            itemTuple.name = itemName;
            itemTuple.quantity = 0;

            foreach (var item in Items)
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
                Items.Add(itemToAdd);
            }

            if (!init)
            {
                OnInventoryItemChanged(itemTuple);
            }
        }

        public void CollectPickup(Pickup pickup)
        {
            var levelItem = new LevelItemTuple();
            levelItem.id = pickup.id;
            OnLooseItemChanged(levelItem, false, SceneManager.GetActiveScene().name);
            AddItem(pickup.name);
        }

        public void RemoveItem(InventoryItem itemToRemove, bool removeAll, bool drop)
        {
            var amountToDrop = 0;
            int amountLeft = 0;
            if (removeAll)
            {
                amountToDrop = itemToRemove.quantity;
                amountLeft = 0;
                Items.Remove(itemToRemove);
            }
            else
            {
                amountToDrop = 1;
                itemToRemove.quantity--;
                amountLeft = itemToRemove.quantity;
                if (itemToRemove.quantity <= 0)
                {
                    Items.Remove(itemToRemove);
                }
            }

            var itemTuple = new ItemTuple();
            itemTuple.name = itemToRemove.item.name;
            itemTuple.quantity = amountLeft;
            OnInventoryItemChanged(itemTuple);

            if (!drop) return;

            for (var i = 0; i < amountToDrop; i++)
            {
                InstantiateItem(itemToRemove.item, transform.position);
            }
        }

        public void InstantiateItem(Item item, Vector3 pos, bool write = true, int id = 0)
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

            if (write)
            {
                pickup.GenerateID();
                while (!GameData.data.saveData.ValidateUniqueItemID(pickup.id))
                {
                    pickup.GenerateID();
                }
                var levelItem = new LevelItemTuple();
                levelItem.id = pickup.id;
                levelItem.name = pickup.name;
                levelItem.xLoc = pos.x;
                levelItem.yLoc = pos.y;
                OnLooseItemChanged(levelItem, true, SceneManager.GetActiveScene().name);
            }
            else
            {
                pickup.id = id;
            }
        }
    }
}
