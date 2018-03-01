using UnityEngine;

namespace Character2D
{
    public class AttackTrigger : Trigger<Attackable>
    {
        public CharacterAttack characterAttack; //reference to the character attack script

        // Use this for initialization
        void Start()
        {
            disregardCount = false;
        }

        //fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            if (isInTrigger)
            {
                characterAttack.canHitAttack = true;
            }
            else
            {
                characterAttack.canHitAttack = false;
            }
        }
    }
}
