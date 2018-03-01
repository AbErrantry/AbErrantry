namespace Character2D
{
    public class ClimbingTriggerTop : Trigger<Climbable>
    {
        public PlayerMovement playerMovement;

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            //nothing
        }
    }
}
