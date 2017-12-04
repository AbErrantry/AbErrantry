namespace Character2D
{
    public class GroundedTrigger : Trigger
    {
        public CharacterMovement characterMovement; //reference to the character movement script

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            if (isInTrigger)
            {
                characterMovement.GroundedReset();
            }
            else
            {
                characterMovement.isGrounded = false;
            }
        }
    }
}
