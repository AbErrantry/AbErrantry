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

		protected override void Die()
        {
            //take away enemy input
            //enemy no longer targets player
			//enemy no longer attackable
            //death animation for enemy
			//drop loot
			//destroy enemy
			Debug.Log("Enemy died: " + gameObject.name); //TODO: remove debug
        }
	}
}