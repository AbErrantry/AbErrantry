using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class AICrouchManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentObstacles; //list of objects impeding the character's way
        public BehaviorAI aiBehavior; //reference to the BehaviorAI script on the character

        //detects when the character is impeded by a new obstacle
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //set the character to be grounded and add the object to the list
                currentObstacles.Add(other.gameObject);
                aiBehavior.InputAttempt();
            }
        }

        //detects when the character is no longer impeded by an obstacle
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the colliding object is a part of the game world (environment)
            if (other.tag == "World")
            {
                //remove the object from the list
                currentObstacles.Remove(other.gameObject);
                if (currentObstacles.Count == 0)
                {
                    aiBehavior.InputAttempt();
                }
            }
        }
    }
}
