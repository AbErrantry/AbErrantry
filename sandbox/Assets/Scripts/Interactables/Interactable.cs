using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum Types //enumeration of character types
    {
        BackDoor, SideDoor, Pickup, NPC, Chest
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
            case Types.BackDoor:
                type = "Enter";
                break;
            case Types.SideDoor:
                type = "Open";
                break;
            case Types.Pickup:
                type = "Pick up";
                break;
            case Types.NPC:
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
