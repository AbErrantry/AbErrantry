using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class AIMoveManager : MonoBehaviour
    {

        [SerializeField] public List<GameObject> currentTarget; //list of objects the character is standing on
        public CharacterMovement characterMovement; //reference to the character movement script
        public BehaviorAI aiBehavior;

        //detects when the player is standing on a new object
        private void OnTriggerEnter2D(Collider2D other)
        {

            //if the object is not part of the player,
            if (other.tag == "Player" || other.tag == "World")
            {
                //Debug.Log(other + " Entered");
                //set the player to be grounded and add the object to the list
                currentTarget.Add(other.gameObject);
                if (currentTarget.Count == 1 && other.tag == "Player")
                {
                    aiBehavior.AIMove(true);
                }
            }
        }

        //detects when the player is no longer standing on an object
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the object is not part of the player,
            if (other.tag == "Player" || other.tag == "World")
            {
                //Debug.Log(other + " Exited");
                //remove the object from the list
                currentTarget.Remove(other.gameObject);

                if (currentTarget.Count == 0)
                {
                    aiBehavior.AIMove(false);
                }
            }
        }
    }
}
