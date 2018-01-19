using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class SlideBonus : StateMachineBehaviour 
	{
		private PlayerMovement playerMovement;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			playerMovement = animator.GetComponentInParent<PlayerMovement>();
			playerMovement.MoveBonus(1.0f);
		}
	}
}
