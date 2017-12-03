using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class GroundedManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentGround; //list of objects the character is standing on
        public CharacterMovement characterMovement; //reference to the character movement script

        //detects when the character is standing on a new object
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //set the character to be grounded and add the object to the list
                characterMovement.GroundedReset();
                currentGround.Add(other.gameObject);
            }
        }

        //detects when the character is no longer standing on an object
        private void OnTriggerExit2D(Collider2D other)
        {//if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //remove the object from the list
                currentGround.Remove(other.gameObject);
                if (currentGround.Count == 0)
                {
                    //if there are no objects in the list, the character is no longer grounded
                    characterMovement.isGrounded = false;
                }
            }
        }
    }
}
