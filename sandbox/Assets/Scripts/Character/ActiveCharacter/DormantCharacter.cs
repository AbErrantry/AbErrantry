using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class DormantCharacter : MonoBehaviour
	{
		private Enemy enemy;

		// Use this for initialization
		void Start()
		{
			enemy = GetComponent<Enemy>();
			enemy.canTakeDamage = false;
			enemy.canKnockBack = false;
			enemy.isDormant = true;
		}

		public void BecomeHostile()
		{
			enemy.canTakeDamage = true;
			enemy.canKnockBack = true;
			enemy.isDormant = false;
			enemy.SetPlayerTarget(Player.instance.gameObject);
		}
	}
}
