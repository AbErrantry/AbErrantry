using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class AttackableTile : Attackable
	{
		// Use this for initialization
		protected new void Start()
		{
			base.Start();
			canTakeDamage = true;
			canKnockBack = false;
			canFlinch = false;
		}

		//applies damage to the player
		public override void TakeDamage(GameObject attacker, int damage)
		{
			if (!isDying)
			{
				if (canTakeDamage && attacker.GetComponent<ExplodingCrate>() != null)
				{
					currentVitality = currentVitality - damage;
					if (currentVitality <= 0f)
					{
						Die();
					}
				}
			}
		}

		protected override void InitializeDeath()
		{
			Destroy(GetComponent<CompositeCollider2D>());
			GetComponent<BoxCollider2D>().enabled = false;
			anim.Play("EXPLODE");
		}

		public override void FinalizeDeath()
		{
			Destroy(gameObject);
		}

		public virtual void DestroyCrate()
		{
			InitializeDeath();
		}
	}
}
