using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class ActiveCharacterMovement : CharacterMovement 
	{
		private ActiveCharacterAttack characterAttack;

		// Use this for initialization
		protected new void Start() 
		{
			base.Start();
			characterAttack = gameObject.GetComponent<ActiveCharacterAttack>();
		}

		// Update is called once per frame
		protected new void Update() 
		{
			if(!characterAttack.isAttacking)
			{
				base.Update();
			}
			else
			{
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
				SendToAnimator();
			}
		}

		protected new void FixedUpdate()
		{
			if(!characterAttack.isAttacking)
			{
				base.FixedUpdate();
			}
			else
			{
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
				SendToAnimator();
			}
		}
	}
}
