using UnityEngine;

public class LevelUnloadTrigger : MonoBehaviour
{
    public LevelStreamManager manager; //reference to the LevelStreamManager component for this level

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player, unload the level
        if (other.tag == "Player")
        {
            manager.UnloadScene();
        }
    }
}
