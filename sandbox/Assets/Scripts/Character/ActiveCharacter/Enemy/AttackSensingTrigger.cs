using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class AttackSensingTrigger : Trigger<Player>
	{
		public EnemyAttack enemyAttack;

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
