using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class Crate : Attackable 
	{
		// Use this for initialization
		private new void Start () 
		{
			base.Start();
			currentVitality = 20.0f;
			canTakeDamage = true;
			canKnockBack = false;
			canFlinch = true;
		}

		protected override void InitializeDeath()
		{
			anim.Play("DESTROY");
		}

		public override void FinalizeDeath()
		{
			Destroy(gameObject);
		}
	}
}
