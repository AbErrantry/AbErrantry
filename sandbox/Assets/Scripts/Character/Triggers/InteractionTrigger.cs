namespace Character2D
{
    public class InteractionTrigger : Trigger
    {
        public CharacterInteraction characterInteraction;

        //used for initialization
        void Start()
        {
            objectTag = "Interactable"; //overrides the tag from "World"
            disregardCount = true; //don't consider the object count for onTriggerExit in Trigger
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            characterInteraction.DisplayText();
        }
    }
}
