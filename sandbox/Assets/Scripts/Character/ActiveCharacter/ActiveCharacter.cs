using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public abstract class ActiveCharacter : Character 
	{
		//used for initialization
		protected new void Start () 
		{
			base.Start();
		}

		//applies damage to the player
        public void TakeDamage(float damage)
        {
            vitality = vitality - damage;
            if (vitality <= 0f)
            {
                Die();
            }
        }

		protected abstract void Die();
	}
}
