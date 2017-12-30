using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterBehavior : MonoBehaviour
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

        public bool isAttackable; //whether or not the character is attackable

        public List<InventoryItem> items; //items held by the character
        public GameData gameData;

        //used for initialization
        private void Start()
        {
            items = new List<InventoryItem>();
        }

        //applies damage to the player
        public virtual void TakeDamage(float damage)
        {
            if(isAttackable)
            {
                vitality = vitality - damage;
                if (vitality <= 0f)
                {
                    //kill
                    //spawn items as loot
                }
            }
        }

        public void AddItem(string name)
        {
            bool isFound = false;
            foreach(InventoryItem item in items)
            {
                if(item.item == gameData.itemData.itemDictionary[name])
                {
                    item.quantity++;
                    isFound = true;
                    break;
                }
            }
            if(!isFound)
            {
                InventoryItem itemToAdd = new InventoryItem();
                itemToAdd.item = gameData.itemData.itemDictionary[name];
                itemToAdd.quantity = 1;
                items.Add(itemToAdd);
            }
        }

        public void RemoveItem(string name)
        {
            InventoryItem itemToRemove = new InventoryItem();
            itemToRemove.item = gameData.itemData.itemDictionary[name];
            foreach (InventoryItem inv in items)
            {
                if (inv.item == gameData.itemData.itemDictionary[name])
                {
                    inv.quantity--;
                    //instantiate item on the ground
                    if(inv.quantity <= 0)
                    {
                        items.Remove(inv);
                    }
                    break;
                }
            }
        }

        //todo: differentiate between remove/drop/destroy generically.

        private void PrintItems()
        {
            foreach(InventoryItem inv in items)
            {
                Debug.Log("Item: " + inv.item.name + " | " + inv.quantity);
            }
        }
    }
}
