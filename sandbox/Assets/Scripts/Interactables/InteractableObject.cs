using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum Types //enumeration of character types
    {
        EnterDoor, ExitDoor, Item, Character, Chest
    };

    public new string name;
    [System.NonSerialized] public string type;
    public Types typeOfInteractable;

    protected void Start()
    {
        SetType();
    }

    public void SetType()
    {
        switch (typeOfInteractable)
        {
            case Types.EnterDoor:
                type = "Enter";
                break;
            case Types.ExitDoor:
                type = "Exit";
                break;
            case Types.Item:
                type = "Pick up";
                break;
            case Types.Character:
                type = "Talk";
                break;
            case Types.Chest:
                type = "Open";
                break;
            default:
                Debug.LogError("Item: " + name + " (named " + gameObject.name + " in hierarchy) " +
                    "has an undefined type. Please set it in the InteractableObject script.");
                break;
        }
    }
}
