using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class Enemy : ActiveCharacter 
	{
		//used for initialization
		protected new void Start()
		{
			base.Start();
		}

		protected override void InitializeDeath()
		{
			//take away enemy input
            //enemy no longer targets player
			//enemy no longer attackable
			isDying = true;
			anim.SetBool("isDying", isDying); //death animation 
		}

		public override void FinalizeDeath()
		{
			//drop loot
			Debug.Log("Enemy died: " + gameObject.name); //TODO: remove debug
			Destroy(gameObject);
		}
	}
}