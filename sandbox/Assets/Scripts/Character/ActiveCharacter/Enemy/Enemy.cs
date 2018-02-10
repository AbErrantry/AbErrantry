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
        private EnemyMovement enemyMovement;
        public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;
        public BeaconControl beacCon; 
		//used for initialization
		protected new void Start()
		{
			base.Start();
            enemyMovement = GetComponent<EnemyMovement>();
            canFlinch = false;
		    canKnockBack = true;
		    canTakeDamage = true;
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

        public override void FinalizeDeath()
		{
			//drop loot
			Debug.Log("Enemy died: " + gameObject.name); //TODO: remove debug
			Destroy(gameObject);
		}
   
        //TODO: change to using fixed update to constantly monitor towards target (for edge cases)
        //TODO: move to a behavior AI class that will take care of figuring out where the enemy needs to go
        public void SwitchBeacon()
        {
            beacCon.beaconNum++;
            beacCon.currTarget = beacCon.beacons[beacCon.beaconNum % beacCon.beacons.Length];

            if(beacCon.currTarget.transform.position.x - this.gameObject.transform.position.x <0 && enemyMovement.isFacingRight)
            {
                enemyMovement.ChangeDirection();
            }
            else if(beacCon.currTarget.transform.position.x - this.gameObject.transform.position.x >0 && !enemyMovement.isFacingRight)
            {
                enemyMovement.ChangeDirection();
            }
        }
    }
}