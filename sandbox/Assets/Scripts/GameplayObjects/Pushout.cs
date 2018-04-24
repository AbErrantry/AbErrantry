using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class Pushout : MonoBehaviour {

	private Rigidbody2D playerRBody;

	[Tooltip("Set the Force of the pushout. Is multiplied by 100 in script.")]
	[Range(0,10)]
	public float pushForce;
	void Start()
	{
		pushForce *= 1000;
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.GetComponent<Player>() != null)
		{
			playerRBody = col.gameObject.GetComponent<Rigidbody2D>();
			Vector2 playerPos = col.transform.position;
			Vector2 dir = col.contacts[0].point - playerPos;
			dir = -dir.normalized;
			playerRBody.velocity = new Vector2(0,0);
			playerRBody.AddForce(new Vector2(-pushForce,0));
		}
	}
}
}
