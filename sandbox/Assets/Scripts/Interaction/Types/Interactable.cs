using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum Types //enumeration of character types
    {
        BackDoor, SideDoor, Pickup, NPC, Chest
    };

    public int id;
    public new string name;
    [System.NonSerialized] public string type;
    [System.NonSerialized] public Types typeOfInteractable;

    protected void Start()
    {
        //set name 
        SetType();
        SetID();
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
                type = "Talk to";
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

    private void SetID()
    {
        //on scene load, instantiate objects in scene with instance ID that was set previously
        //if object is newly instantiated,
        id = GetInstanceID();

        //if object is being instantiated from file
        //id = id in file.
    }
}
