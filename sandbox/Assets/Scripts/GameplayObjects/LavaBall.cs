using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class LavaBall : MonoBehaviour {

	public int damage;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<Attackable>() != null)
		{
			col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject,damage);
		}
	}

}
}