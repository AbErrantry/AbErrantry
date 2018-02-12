using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    public LevelStreamManager manager; //reference to the LevelStreamManager component for this level

    public BackgroundSwitch backgroundSwitch;
    public int backgroundID;

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player,
        if (other.tag == "Player")
        {
            manager.MakeActive();
            backgroundSwitch.UpdateScrolling(backgroundID);
        }
    }
}
