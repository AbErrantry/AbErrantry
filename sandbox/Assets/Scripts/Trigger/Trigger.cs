using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField] public List<GameObject> currentObjects; //list of objects that are in the trigger
    public string objectTag; //tag of the object to detect
    public string layerTag; //layer of the object to detect
    public bool disregardCount; //whether the count should be disregarded on exit or not
    protected abstract void TriggerAction(bool isInTrigger);

    //used for initialization
    private void Start()
    {
        objectTag = "World";
        layerTag = "None";
        disregardCount = false;
    }

    //detects when the character is targeting a new object
    protected void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is the one specified,
        if (other.tag == objectTag || other.gameObject.layer == LayerMask.NameToLayer(layerTag))
        {
            //set the character to be grounded and add the object to the list
            currentObjects.Add(other.gameObject);
            TriggerAction(true);
        }
    }

    //detects when the character is no longer targeting an object
    protected void OnTriggerExit2D(Collider2D other)
    {
        //if the object is the one specified,
        if (other.tag == objectTag || other.gameObject.layer == LayerMask.NameToLayer(layerTag))
        {
            //remove the object from the list
            currentObjects.Remove(other.gameObject);
            if (currentObjects.Count == 0 || disregardCount)
            {
                TriggerAction(false);
            }
        }
    }
}
