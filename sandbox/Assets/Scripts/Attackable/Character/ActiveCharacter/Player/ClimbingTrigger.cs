namespace Character2D
{
    public class ClimbingTrigger : Trigger<Climbable>
    {
        public PlayerMovement playerMovement;

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            if (isInTrigger)
            {
                playerMovement.canClimb = true;
            }
            else
            {
                playerMovement.canClimb = false;
                playerMovement.DoneClimbing();
            }
        }
    }
}
