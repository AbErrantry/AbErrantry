using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    public LevelStreamManager manager; //reference to the LevelStreamManager component for this level

    //function that fires when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the colliding object is the player, load the scene
        if (other.tag == "Player")
        {
            manager.LoadScene();
        }
    }
}
