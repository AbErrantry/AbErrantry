using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class FireballGiant : MonoBehaviour {

		public int damage;
		public void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.GetComponent<Attackable>() != null)
			{
				col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject, damage);
			}
			StartCoroutine(Death());
		}

		public IEnumerator Death()
		{
			gameObject.GetComponent<Animator>().SetTrigger("Dying");
			yield return new WaitForSeconds(0.5f);
			Destroy(gameObject);
		}
	}
}
