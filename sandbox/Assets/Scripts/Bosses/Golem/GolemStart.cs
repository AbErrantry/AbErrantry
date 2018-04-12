﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class GolemStart : MonoBehaviour {

	public GameObject golemPrefab;
	public Collider2D upSwipeTrigger;
	public Collider2D downSwipeTrigger;
	public Collider2D SmashTrigger;
	void Start () 
	{
		
	}
	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<Player>() != null)
		{
			GameObject clone = Instantiate(golemPrefab, new Vector3( GetComponent<BoxCollider2D>().bounds.center.x, GetComponent<BoxCollider2D>().bounds.max.y, golemPrefab.transform.position.z)
										,Quaternion.identity);

			clone.GetComponent<GolemBoss>().UpSwipeBounds = upSwipeTrigger;
			clone.GetComponent<GolemBoss>().DownSwipeBounds = downSwipeTrigger;
			clone.GetComponent<GolemBoss>().SmashBounds = SmashTrigger;
			
			Destroy(this.gameObject);
		}
	}
}
}
