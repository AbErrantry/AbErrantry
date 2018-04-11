using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class Snowball : MonoBehaviour {

	public int snowballDamage;
	private Vector2 completeStop;
	// Use this for initialization
	void Start ()
	{
		completeStop = new Vector2(0,0);
		//gameObject.GetComponent<Animator>().SetTrigger("Spawn");

	}
	void Update()
	{
		if(gameObject.GetComponent<Rigidbody2D>().velocity == completeStop)
		{
			StartCoroutine(DestroySnowball());
		}
	}
	
	public void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.GetComponent<Attackable>() != null)
		{
			col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject,snowballDamage);
			StartCoroutine(DestroySnowball());
		}
		
	}
	
	public IEnumerator DestroySnowball()
	{
		gameObject.GetComponent<Animator>().SetBool("Death",true);
		yield return new WaitForSeconds(1.25f);
		Destroy(this.gameObject);
	}
}
}
