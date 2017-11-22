using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class GroundedManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentGround; //list of objects the character is standing on
        private CharacterMovement characterMovement; //reference to the character movement script

        //used for initialization
        void Start()
        {
            characterMovement = GameObject.Find("Knight").GetComponent<CharacterMovement>();
        }

        //detects when the player is standing on a new object
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the object is not part of the player,
            if (other.tag != "Player")
            {
                //set the player to be grounded and add the object to the list
                characterMovement.isGrounded = true;
                currentGround.Add(other.gameObject);
            }
        }

        //detects when the player is no longer standing on an object
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the object is not part of the player,
            if (other.tag != "Player")
            {
                //remove the object from the list
                currentGround.Remove(other.gameObject);
                if(currentGround.Count == 0)
                {
                    //if there are no objects in the list, the character is no longer grounded
                    characterMovement.isGrounded = false;
                }
            }
        }

        //TODO: create check if grounded to optimize CharacterMovement
    }
}
