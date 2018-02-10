using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class EnemyMovement : CharacterMovement 
	{
		public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;

		// Use this for initialization
		private new void Start() 
		{
			base.Start();
			mvmtSpeed = 1.0f;
		}

		// Update is called once per frame
		private new void Update()
		{
			base.Update();

		}

		// Update is called once per tick
		private new void FixedUpdate()
		{
			base.FixedUpdate();
			
		}

		public void JumpAttempt()
        {
            jumpInput = false;
            if (topJump.currentObjects.Count == 0 && botJump.currentObjects.Count != 0)
            {
                jumpInput = true; //send jump input
            }
        }

		public void ChangeDirection()
		{
			if(isFacingRight)
			{
				mvmtSpeed = -1.0f;
			}
			else
			{
				mvmtSpeed = 1.0f;
			}
		}
	}
}