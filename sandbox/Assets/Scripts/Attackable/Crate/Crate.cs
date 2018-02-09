using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class Crate : Attackable 
	{
		public BoxMove boxMove;

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
			boxMove.enabled = false;
			Destroy (GetComponent<CompositeCollider2D>());
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;
			anim.Play("DESTROY");
		}

		public override void FinalizeDeath()
		{
			Destroy(gameObject);
		}
	}
}
