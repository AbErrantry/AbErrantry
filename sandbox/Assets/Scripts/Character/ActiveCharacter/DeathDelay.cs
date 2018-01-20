using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class DeathDelay : StateMachineBehaviour 
	{
		private ActiveCharacter activeCharacter;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			activeCharacter = animator.GetComponentInParent<ActiveCharacter>();
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			activeCharacter.FinalizeDeath();
		}
	}
}
