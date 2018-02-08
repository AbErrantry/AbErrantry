using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class Enemy : Attackable 
	{
        private float maxSpeed;
        public ActiveCharacterMovement acMove;
        public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;

		//used for initialization
		protected new void Start()
		{
			base.Start();
            acMove.mvmtSpeed = 1;
        }

		protected override void InitializeDeath()
		{
			//take away enemy input
            //enemy no longer targets player
			//enemy no longer attackable
			isDying = true;
			anim.SetBool("isDying", isDying); //death animation 
		}

        internal void JumpAttempt()
        {
            acMove.jumpInput = false;
            if (topJump.currentObjects.Count == 0 && botJump.currentObjects.Count != 0)
            {
                acMove.jumpInput = true; //send jump input
            }
        }

        public override void FinalizeDeath()
		{
			//drop loot
			Debug.Log("Enemy died: " + gameObject.name); //TODO: remove debug
			Destroy(gameObject);
		}
   
        public void SwitchBeacon()
        {
            switch(acMove.mvmtSpeed == 1)
            {
                case true:
                    acMove.mvmtSpeed = -1;
                    break;
                case false:
                    acMove.mvmtSpeed = 1;
                    break;
            }
        }

        
    }
    
}