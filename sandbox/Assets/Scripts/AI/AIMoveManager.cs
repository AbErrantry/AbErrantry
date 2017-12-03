using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class AIMoveManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentTargets; //list of objects the character is targeting
        public BehaviorAI aiBehavior; //reference to the BehaviorAI component on the character

        //detects when the character is targeting a new object
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if the object is the player,
            if (other.tag == "Player")
            {
                //set the character to be grounded and add the object to the list
                currentTargets.Add(other.gameObject);
                aiBehavior.AITrack(true);
            }
        }

        //detects when the character is no longer targeting an object
        private void OnTriggerExit2D(Collider2D other)
        {
            //if the object is the player,
            if (other.tag == "Player")
            {
                //remove the object from the list
                currentTargets.Remove(other.gameObject);
                if (currentTargets.Count == 0)
                {
                    aiBehavior.AITrack(false);
                }
            }
        }
    }
}
