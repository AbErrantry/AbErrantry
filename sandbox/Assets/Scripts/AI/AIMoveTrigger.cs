namespace Character2D
{
    public class AIMoveTrigger : Trigger
    {
        public BehaviorAI aiBehavior; //reference to the BehaviorAI component on the character

        //used for initialization
        private void Start()
        {
            objectTag = "Player"; //overrides the tag from "World"
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            aiBehavior.AITrack(isInTrigger);
        }
    }
}
