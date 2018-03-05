using UnityEngine;

namespace Character2D
{
    public class RollBonus : StateMachineBehaviour
    {
        private PlayerMovement playerMovement;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement = animator.GetComponentInParent<PlayerMovement>();
            playerMovement.MoveBonus(2.0f);
            playerMovement.isRolling = true;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerMovement.isRolling = false;
        }
    }
}
