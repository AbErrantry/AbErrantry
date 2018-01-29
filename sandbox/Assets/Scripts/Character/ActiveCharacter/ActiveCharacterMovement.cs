using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class ActiveCharacterMovement : CharacterMovement 
	{
		private ActiveCharacterAttack characterAttack;
		private ActiveCharacter activeCharacter;

		// Use this for initialization
		protected new void Start() 
		{
			base.Start();
			activeCharacter = gameObject.GetComponent<ActiveCharacter>();
			characterAttack = gameObject.GetComponent<ActiveCharacterAttack>();
		}

		// Update is called once per frame
		protected new void Update() 
		{
			base.Update();
		}

		protected new void FixedUpdate()
		{
			base.FixedUpdate();
			if(characterAttack.isAttacking || activeCharacter.isDying)
			{
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
				SendToAnimator();
			}            
		}
	}
}
