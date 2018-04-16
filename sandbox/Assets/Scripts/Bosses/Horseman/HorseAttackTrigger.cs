using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class HorseAttackTrigger : MonoBehaviour {


		public Horseman horseman;

		public void Start()
		{
			horseman = gameObject.GetComponentInParent<Horseman>();
		}
		public void OnTriggerEnter2D(Collider2D col)
		{
			if(col.gameObject.GetComponent<Attackable>() != null)
			{
				horseman.ApplyDamage(col.gameObject);
			}
		}
}
}
