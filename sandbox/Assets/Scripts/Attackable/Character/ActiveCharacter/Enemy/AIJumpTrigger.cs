using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class AIJumpTrigger : Trigger<Standable>
    {
        public EnemyMovement enemy; //reference to the BehaviorAI script on the character

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            enemy.JumpAttempt();
        }
    }
}
