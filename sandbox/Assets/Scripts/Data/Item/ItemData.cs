using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : ScriptableObject
{
    public Dictionary<string, Item> itemDictionary;

    //default constructor
    private void OnEnable()
    {
        itemDictionary = new Dictionary<string, Item>();
        GetItems();
    }

    //gets each item into memory from the database
    private void GetItems()
    {
        string itemDatabase = Application.streamingAssetsPath + "/ItemDatabase.xml";
        List<Item> itemList = new List<Item>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(itemDatabase);
        itemList = (from item in XDoc.Root.Elements("item") select new Item
        {
            name = item.AttributeValueNull_String("name"),
                description = item.Element("description").ElementValueNull_String(),
                type = item.Element("type").ElementValueNull_String(),
                speed = item.Element("speed").ElementValueNull_Float(),
                strength = item.Element("strength").ElementValueNull_Float(),
                useLimit = item.Element("useLimit").ElementValueNull_Integer(),
                rarity = item.Element("rarity").ElementValueNull_Integer(),
                price = item.Element("price").ElementValueNull_Integer(),
                isKey = item.Element("isKey").ElementValueNull_Boolean(),
                spriteName = item.Element("asset").ElementValueNull_String(),
                materialName = item.Element("material").ElementValueNull_String(),
        }).OrderBy(x => x.name).ToList();

        foreach (Item item in itemList)
        {
            item.sprite = Resources.Load<Sprite>("Items/" + item.spriteName);
            item.material = Resources.Load<Material>("Shaders/" + item.materialName);
            itemDictionary.Add(item.name, item);
        }
    }

    private void PrintItems()
    {
        foreach (Item val in itemDictionary.Values)
        {
            Debug.Log(val.name + " " + val.type + " " + val.speed + " " + val.strength +
                " " + val.useLimit + " " + val.rarity + " " + val.price + " " + val.isKey +
                " " + val.materialName + " " + val.spriteName);
        }
    }
}
