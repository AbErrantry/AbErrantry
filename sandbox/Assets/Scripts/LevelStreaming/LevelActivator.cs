using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    public LevelInfo levelInfo;
    public LevelStreamManager manager; //reference to the LevelStreamManager component for this level
    private int backgroundID;

    private void Start()
    {
        backgroundID = levelInfo.backgroundID;
    }

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player,
        if (other.tag == "Player")
        {
            manager.MakeActive();
            BackgroundSwitch.instance.UpdateScrolling(backgroundID);
        }
    }
}
