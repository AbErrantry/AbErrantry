using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObjRepeater : MonoBehaviour {

	public bool isLeft;
	public GameObject OtherSide;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(isLeft)
		{
			col.transform.position = new Vector2(OtherSide.transform.position.x - col.bounds.size.x, OtherSide.transform.position.y);
		}
		else
		{
			col.transform.position = new Vector2(OtherSide.transform.position.x + col.bounds.size.x, OtherSide.transform.position.y);
		}
	
	}
}
