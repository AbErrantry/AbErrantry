using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class BearBoss : Boss
	{
		private BearMovement bearMovement;

		//used for initialization
		protected new void Start()
		{
			name = "Bear";
			base.Start();
			bearMovement = GetComponent<BearMovement>();
			canFlinch = false;
			canKnockBack = false;
			canTakeDamage = true;
		}

		private new void Update()
		{
			bearMovement.mvmtSpeed = 1.0f;
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
			BossDefeated();
			//TODO: give random amount of gold around enemy difficulty.
			Destroy(gameObject);
		}

		private void OnDestroy()
		{
			if (GameObject.Find("CameraTarget"))
			{
				var followTarget = GameObject.Find("CameraTarget").GetComponent<FollowTarget>();
				if (followTarget != null)
				{
					followTarget.target = Player.instance.gameObject.transform;
				}
			}
		}
	}
}
