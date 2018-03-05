using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class AttackBonus : MonoBehaviour
	{
		private PlayerMovement playerMovement;

		private void Start()
		{
			playerMovement = GetComponent<PlayerMovement>();
		}

		public void Bonus(float bonus)
		{
			playerMovement.MoveBonus(bonus);
		}
	}
}
