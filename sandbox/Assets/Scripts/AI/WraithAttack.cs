using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class WraithAttack : MonoBehaviour
	{
		public WraithBoss wraith;

		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Attackable>() != null)
			{
				wraith.ApplyDamage(col.gameObject);
			}
		}
	}
}
