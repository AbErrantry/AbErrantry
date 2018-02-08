using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class ActiveCharacterMovement : CharacterMovement 
	{
		private CharacterAttack characterAttack;
		private Attackable attackable;

		// Use this for initialization
		protected new void Start() 
		{
			base.Start();
			attackable = gameObject.GetComponent<Attackable>();
			characterAttack = gameObject.GetComponent<CharacterAttack>();
		}

		// Update is called once per frame
		protected new void Update() 
		{
			base.Update();
		}

		protected new void FixedUpdate()
		{
			base.FixedUpdate();
			if(characterAttack.isAttacking || attackable.isDying)
			{
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
				SendToAnimator();
			}            
		}
	}
}
