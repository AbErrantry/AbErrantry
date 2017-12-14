using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;

public class ItemData : ScriptableObject
{
    public Dictionary<string, Item> itemDictionary;

    //default constructor
    public ItemData()
    {
        itemDictionary = new Dictionary<string, Item>();
        GetItems();
    }

    //gets each item into memory from the database
    private void GetItems()
    {
        string itemDatabase = "ItemDatabase.xml";
        List<Item> itemList = new List<Item>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(itemDatabase);
        itemList = (from item in XDoc.Root.Elements("item")
                            select new Item
                            {
                                name = item.AttributeValueNull_String("name"),
                                type = item.Element("type").ElementValueNull_String(),
                                speed = item.Element("speed").ElementValueNull_Float(),
                                strength = item.Element("strength").ElementValueNull_Float(),
                                useLimit = item.Element("useLimit").ElementValueNull_Integer(),
                                rarity = item.Element("rarity").ElementValueNull_Integer(),
                                price = item.Element("price").ElementValueNull_Float(),
                                isDestroyable = item.Element("isDestroyable").ElementValueNull_Boolean(),
                            }).OrderBy(x => x.name).ToList();

        foreach(Item item in itemList)
        {
            itemDictionary.Add(item.name, item);
        }
    }
    
    //debug function
    private void PrintItems()
    {
        foreach (Item val in itemDictionary.Values)
        {
            Debug.Log(val.name + " " + val.type + " " + val.speed + " " + val.strength + " " + val.useLimit + " " + val.rarity + " " + val.price + " " + val.isDestroyable);
        }
    }
}
