using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class UncrouchManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentObstacles; //list of objects that are in the way of the character standing up
        private CharacterMovement characterMovement; //reference to the character movement script

        //used for initialization
        void Start()
        {
            characterMovement = GameObject.Find("Knight").GetComponent<CharacterMovement>();
        }

        //detects when a new object is obstructing the player
        private void OnTriggerEnter2D(Collider2D other)
        {
            //the object must be a part of the game world (environment)
            if (other.tag == "World")
            {
                //prevent the player from uncrouching and add the object to the list
                characterMovement.canUncrouch = false;
                currentObstacles.Add(other.gameObject);
            }
        }

        //detects when an object is no longer obstructing the player
        private void OnTriggerExit2D(Collider2D other)
        {
            //the object must be a part of the game world (environment)
            if (other.tag == "World")
            {
                //removes the object from the list
                currentObstacles.Remove(other.gameObject);
                if (currentObstacles.Count == 0)
                {
                    //if there are no obstacles in the list, the player can now uncrouch
                    characterMovement.canUncrouch = true;
                }
            }
        }

        //TODO: create check if can uncrouch to optimize CharacterMovement
    }
}
