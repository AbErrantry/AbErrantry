using UnityEngine;

namespace Character2D
{
	public class ExplodingTrigger : Trigger<Attackable>
	{
		public ExplodingCrate explodingCrate; //reference to the character attack script

		// Use this for initialization
		void Start()
		{
			disregardCount = false;
		}

		//fires upon an object entering/exiting the trigger box
		protected override void TriggerAction(bool isInTrigger)
		{
			if (isInTrigger)
			{
				explodingCrate.canHitAttack = true;
			}
			else
			{
				explodingCrate.canHitAttack = false;
			}
		}
	}
}
