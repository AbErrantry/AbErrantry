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
