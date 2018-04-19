using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class Snowball : MonoBehaviour {

	public int snowballDamage;

	[Tooltip("How long until the Snowball disappears")]
	[Range(1,60)]
	public float timeout;
	private Vector2 completeStop;
	// Use this for initialization
	void Start ()
	{

	}
	void Update()
	{
		if(timeout <=0)
		{
			StartCoroutine(DestroySnowball());
		}
		else
		{
			timeout -= Time.deltaTime;
		}
	}
	
	public void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.GetComponent<Attackable>() != null)
		{
			col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject,snowballDamage,false);
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
