﻿namespace Character2D
{
    public class ClimbingTriggerTop : Trigger
    {
        public PlayerMovement playerMovement;

        // Use this for initialization
        void Start()
        {
            objectTag = "Ladder"; //set its target as any enemy
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            //nothing
        }
    }
}
