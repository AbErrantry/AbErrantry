using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Character : MonoBehaviour
    {
        public enum Types //enumeration of character types
        {
            Knight, Goblin, Villager, Skeleton, Orc
        };

        public Types type; //the type of character

        public float vitality; //the vitality of the character
        public float strength; //the strength of the character
        public float agility; //the agility of the character
        public float weight; //the weight of the character
        public bool isActive; //whether or not the character is active

        public GameObject interactable;

        public List<InventoryItem> items; //items held by the character

        //used for initialization
        protected void Start()
        {
            items = new List<InventoryItem>();
            
            vitality = 100; //TODO: remove
        }

        public void AddItem(string name)
        {
            bool isFound = false;
            foreach(InventoryItem item in items)
            {
                if(item.item == GameData.data.itemData.itemDictionary[name])
                {
                    item.quantity++;
                    isFound = true;
                    break;
                }
            }
            if(!isFound)
            {
                InventoryItem itemToAdd = new InventoryItem();
                itemToAdd.item = GameData.data.itemData.itemDictionary[name];
                itemToAdd.quantity = 1;
                items.Add(itemToAdd);
            }
        }

        public void RemoveItem(InventoryItem itemToRemove, bool removeAll, bool drop)
        {
            int amountToDrop = 0;
            if(removeAll)
            {
                amountToDrop = itemToRemove.quantity;
                items.Remove(itemToRemove);
            }
            else
            {
                amountToDrop = 1;
                itemToRemove.quantity--;
                if (itemToRemove.quantity <= 0)
                {
                    items.Remove(itemToRemove);
                }
            }

            if(drop)
            {
                for(int i = 0; i < amountToDrop; i++)
                {
                    InstantiateItem(itemToRemove.item, transform.position);
                }
            }
        }


        private void PrintItems()
        {
            foreach(InventoryItem inv in items)
            {
                Debug.Log("Item: " + inv.item.name + " | " + inv.quantity);
            }
        }

        public void InstantiateItem(Item item, Vector3 pos)
        {
            //TODO: fix comments
            //instantiate a prefab for the interact button
            GameObject newItem = Instantiate(interactable) as GameObject;
            Pickup pu = newItem.GetComponent<Pickup>();
            SpriteRenderer sr = newItem.GetComponent<SpriteRenderer>();
            BoxCollider2D bc = newItem.GetComponent<BoxCollider2D>();

            //set the text for the interactable onscreen 
            pu.name = item.name;
            pu.typeOfInteractable = Interactable.Types.Pickup;
            pu.SetType();

            sr.sprite = item.sprite;

            newItem.transform.position = pos;

            bc.size = sr.bounds.size;

            //for some reason Unity does not use full scale for the instantiated object by default
            newItem.transform.localScale = Vector3.one;
        }
    }
}
