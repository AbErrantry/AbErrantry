using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class ScanTrigger : Trigger<Player>
	{
		public Enemy enemy; // The Enemy script on the enemy

		//fires upon an object entering/exiting the trigger box
		protected override void TriggerAction(bool isInTrigger)
		{
			if (isInTrigger)
			{
				enemy.playerInRange = true;
			}
			else
			{
				enemy.playerInRange = false;
			}
		}
	}
}
