using UnityEngine;

public class Item
{
    public string name;
    public string description; //TODO: add to XML
    public string type;
    public float speed;
    public float strength;
    public int useLimit;
    public int rarity;
    public int price;
    public bool isKey; //if a key item, cannot drop/destroy/sell
    public Sprite sprite; //exists in the Resources folder with filename as its name
}
