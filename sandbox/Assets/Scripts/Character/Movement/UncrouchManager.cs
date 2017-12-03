using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class UncrouchManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentObstacles; //list of objects that are in the way of the character standing up
        public CharacterMovement characterMovement; //reference to the character movement script

        //detects when a new object is obstructing the character
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //prevent the character from uncrouching and add the object to the list
                characterMovement.canUncrouch = false;
                currentObstacles.Add(other.gameObject);
            }
        }

        //detects when an object is no longer obstructing the character
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //removes the object from the list
                currentObstacles.Remove(other.gameObject);
                if (currentObstacles.Count == 0)
                {
                    //if there are no obstacles in the list, the character can now uncrouch
                    characterMovement.canUncrouch = true;
                }
            }
        }
    }
}
