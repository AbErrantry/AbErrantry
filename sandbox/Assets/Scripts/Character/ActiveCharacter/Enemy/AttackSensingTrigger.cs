using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class AttackSensingTrigger : Trigger
	{
		public EnemyAttack enemyAttack;
		// Use this for initialization
		void Start()
		{
			objectTag = "Player";
		}

		protected override void TriggerAction(bool isInTrigger)
		{
			if (isInTrigger)
			{
				enemyAttack.canAttack = true;
			}
			else
			{
				enemyAttack.canAttack = false;
			}
		}
	}
}
