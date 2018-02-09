using UnityEngine;
using System.Collections;

namespace Character2D
{
    public class InteractionTrigger : Trigger
    {
        public PlayerInteraction playerInteraction;
        private BoxCollider2D boxCollider;
        private float colliderSizeX;

        //used for initialization
        void Start()
        {
            objectTag = "Interactable"; //overrides the tag from "World"
            disregardCount = true; //don't consider the object count for onTriggerExit in Trigger

            boxCollider = GetComponent<BoxCollider2D>();
            colliderSizeX = boxCollider.size.x;
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
            if(boxCollider.isActiveAndEnabled)
            {
                StartCoroutine(WiggleCollider());
            }
        }

        private IEnumerator WiggleCollider()
        {
            boxCollider.size = new Vector2(colliderSizeX, boxCollider.size.y);
            boxCollider.size = new Vector2(boxCollider.size.x - 0.01f, boxCollider.size.y);
            yield return new WaitForFixedUpdate();
            boxCollider.size = new Vector2(colliderSizeX, boxCollider.size.y);
        }
    }
}
