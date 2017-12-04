namespace Character2D
{
    public class AICrouchTrigger : Trigger
    {
        public BehaviorAI aiBehavior; //reference to the BehaviorAI script on the character

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            aiBehavior.InputAttempt();
        }
    }
}
