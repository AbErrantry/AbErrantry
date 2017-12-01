using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class AICrouchManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentCeiling; //list of objects the character is standing on
        public CharacterMovement characterMovement; //reference to the character movement script
        public BehaviorAI aiBehavior;

        //detects when the player is standing on a new object
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the object is not part of the player,
            if (other.tag == "World")
            {
                Debug.Log("enter " + other);
                //set the player to be grounded and add the object to the list
                currentCeiling.Add(other.gameObject);
                aiBehavior.Crouch();
            }
        }

        //detects when the player is no longer standing on an object
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the object is not part of the player,
            if (other.tag == "World")
            {
                Debug.Log("exit " + other);
                //remove the object from the list
                currentCeiling.Remove(other.gameObject);
                if (currentCeiling.Count == 0)
                {
                    aiBehavior.Crouch();
                }
            }
        }
    }
}
