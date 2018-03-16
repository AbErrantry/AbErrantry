using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public string type;
    public float speed;
    public float strength;
    public int useLimit;
    public int rarity;
    public int price;
    public bool isKey; //if a key item, cannot drop/destroy/sell

    public string spriteName;
    public string materialName;

    public Sprite sprite; //exists in the Resources/Items folder
    public Material material; //exists in the Resources/Shaders folder
}
