using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFall : MonoBehaviour 
{
	private Rigidbody2D rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void ApplyDownwardForce()
	{
		rb.AddForce(Vector2.down * 50.0f, ForceMode2D.Force);
	}

	public void CancelDownwardForce()
	{
		rb.velocity = new Vector2(rb.velocity.x, 0.0f);
	}
}
