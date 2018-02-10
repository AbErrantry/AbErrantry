using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class Enemy : Attackable 
	{

        [System.Serializable]
        public class BeaconControl
        {
            public GameObject currTarget;
            public GameObject[] beacons;
            public int beaconNum;
        }
        private float maxSpeed;
        public CharacterMovement acMove;
        public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;
        public BeaconControl beacCon; 
		//used for initialization
		protected new void Start()
		{
			base.Start();
            canFlinch = false;
		    canKnockBack = true;
		    canTakeDamage = true;
            acMove.mvmtSpeed = 1;
            beacCon.beaconNum = 0;
            beacCon.currTarget = beacCon.beacons[beacCon.beaconNum];
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
            beacCon.beaconNum++;
            beacCon.currTarget = beacCon.beacons[beacCon.beaconNum % beacCon.beacons.Length];

            if(beacCon.currTarget.transform.position.x - this.gameObject.transform.position.x <0 && acMove.mvmtSpeed == 1)
            {
                TurnAround();
            }
            else if(beacCon.currTarget.transform.position.x - this.gameObject.transform.position.x >0 && acMove.mvmtSpeed == -1)
            {
                TurnAround();
            }
          
        }

        public void TurnAround() //every now and then I get a little bit lonely
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