using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger<T> : MonoBehaviour
{
    [SerializeField] public List<GameObject> currentObjects; //list of objects that are in the trigger

    public bool disregardCount; //whether the count should be disregarded on exit or not

    protected abstract void TriggerAction(bool isInTrigger);

    //used for initialization
    private void Start()
    {
        disregardCount = false;
    }

    //detects when the character is targeting a new object
    protected void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is the one specified,
        if (other.gameObject.GetComponent<T>() != null)
        {
            //set the character to be grounded and add the object to the list
            currentObjects.Add(other.gameObject);
            NullCheck();
            TriggerAction(true);
        }
    }

    //detects when the character is no longer targeting an object
    protected void OnTriggerExit2D(Collider2D other)
    {
        //if the object is the one specified,
        if (other.gameObject.GetComponent<T>() != null)
        {
            //remove the object from the list
            currentObjects.Remove(other.gameObject);
            NullCheck();
            if (currentObjects.Count == 0 || disregardCount)
            {
                TriggerAction(false);
            }
        }
    }

    protected void NullCheck()
    {
        currentObjects.RemoveAll(item => item == null);
    }
}
