using UnityEngine;
using System.Collections;

namespace Character2D
{
    public class InteractionTrigger : Trigger
    {
        public PlayerInteraction playerInteraction;
        private BoxCollider2D boxCollider;

        //used for initialization
        void Start()
        {
            objectTag = "Interactable"; //overrides the tag from "World"
            disregardCount = true; //don't consider the object count for onTriggerExit in Trigger

            boxCollider = GetComponent<BoxCollider2D>();
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            playerInteraction.DisplayText();
        }

        public void DisableTrigger()
        {
            boxCollider.enabled = false;
        }

        public void EnableTrigger()
        {
            boxCollider.enabled = true;
            StartCoroutine(WiggleCollider());
        }

        private IEnumerator WiggleCollider()
        {
            for(int i = 0; i < 5; i++)
            {
                boxCollider.size = new Vector2(boxCollider.size.x - 0.1f, boxCollider.size.y);
                yield return new WaitForFixedUpdate();
            }

            for(int i = 0; i < 5; i++)
            {
                boxCollider.size = new Vector2(boxCollider.size.x + 0.1f, boxCollider.size.y);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
