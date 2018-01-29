using UnityEngine;

namespace Character2D
{
    public class AIMoveTrigger : Trigger
    {
        public BehaviorAI aiBehavior; //reference to the BehaviorAI component on the character
        public RaycastHit2D[] hits;
        private int distance;
        public Transform character;
        Vector2 size;
        public GameObject whatToTrack;
        public int pLayer;
 
        //used for initialization
        private void Start()
        {
            objectTag = "Player"; //overrides the tag from "World"
           // distance = Camera.main.pixelWidth / 2;
            //pLayer = whatToTrack.layer;
            
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            aiBehavior.AITrack(isInTrigger);
        }

    }
}
