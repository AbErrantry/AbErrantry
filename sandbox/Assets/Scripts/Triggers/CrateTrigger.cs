using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class CrateTrigger : Trigger 
	{
		BoxFall boxFall;

		private void Start()
		{
			boxFall = GetComponentInParent<BoxFall>();
		}

		//fires upon an object entering/exiting the trigger box
        protected override void TriggerAction(bool isInTrigger)
        {
            if (!isInTrigger)
            {
                boxFall.ApplyDownwardForce();
            }
			else
            {
                boxFall.CancelDownwardForce();
            }
        }
	}
}
