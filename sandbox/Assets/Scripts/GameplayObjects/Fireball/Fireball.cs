using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class Fireball : MonoBehaviour {

	public float fireballTime;
	public int fireballDamage;
	//private float time;
	// Use this for initialization
	void Start () 
	{
		//time = fireballTime;
		StartCoroutine(FireBallBegin());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if(time <= 0)
		//{
		//	EndFireBall();
		//}
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.GetComponent<Attackable>() != null)
		{
			col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject, fireballDamage);
		}
	}

	public IEnumerator FireBallBegin()
	{
		yield return new WaitForSeconds(fireballTime);
		gameObject.GetComponent<Animator>().SetTrigger("Dying");
		yield return new WaitForSeconds(0.2f);
		EndFireBall();
	}
	public void EndFireBall()
	{
		Destroy(this.gameObject);
	}
}
}