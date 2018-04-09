using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class DormantCharacter : MonoBehaviour
	{
		private Enemy enemy;
		public new string name;

		// Use this for initialization
		void Start()
		{
			enemy = GetComponent<Enemy>();
			name = GetComponent<NPC>().name;
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
