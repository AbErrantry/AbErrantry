using UnityEngine;

namespace Character2D
{
    public class SwingingTrigger : Trigger
    {
        public CharacterAttack characterAttack; //reference to the character attack script

        // Use this for initialization
        void Start()
        {
            //get whether the character is the player or an enemy
            if (transform.root.tag == "Player")
            {
                objectTag = "Enemy"; //set its target as any enemy
            }
            else
            {
                objectTag = "Player"; //set its target as any player
            }
            disregardCount = false;
        }

        //detects when the character is targeting a new object
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            //if the object is the one specified,
            if (other.tag == objectTag || other.gameObject.layer == LayerMask.NameToLayer("Attackable"))
            {
                //set the character to be grounded and add the object to the list
                currentObjects.Add(other.gameObject);
                TriggerAction(true);
            }
        }

        //detects when the character is no longer targeting an object
        protected override void OnTriggerExit2D(Collider2D other)
        {
            //if the object is the one specified,
            if (other.tag == objectTag || other.gameObject.layer == LayerMask.NameToLayer("Attackable"))
            {
                //remove the object from the list
                currentObjects.Remove(other.gameObject);
                if (currentObjects.Count == 0 || disregardCount)
                {
                    TriggerAction(false);
                }
            }
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            if (isInTrigger)
            {
                characterAttack.canHitSwing = true;
            }
            else
            {
                characterAttack.canHitSwing = false;
            }
        }
    }
}
