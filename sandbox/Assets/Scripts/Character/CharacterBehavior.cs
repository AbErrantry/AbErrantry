﻿using System.Collections;
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

        public GameObject interactable;

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
                    instantiateItem(itemToRemove.item);
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

        private void instantiateItem(Item item)
        {
            //TODO: fix comments
            //instantiate a prefab for the interact button
            GameObject newItem = Instantiate(interactable) as GameObject;
            InteractableObject io = newItem.GetComponent<InteractableObject>();
            SpriteRenderer sr = newItem.GetComponent<SpriteRenderer>();
            BoxCollider2D bc = newItem.GetComponent<BoxCollider2D>();

            //set the text for the interactable onscreen 
            io.name = item.name;
            io.typeOfInteractable = InteractableObject.Types.Item;
            io.SetType();

            sr.sprite = item.sprite;

            newItem.transform.position = transform.position;

            bc.size = sr.bounds.size;

            //for some reason Unity does not use full scale for the instantiated object by default
            newItem.transform.localScale = Vector3.one;
        }
    }
}
