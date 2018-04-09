using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class FireballSpawnerButton : MonoBehaviour {

	public GameObject[] spawners;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<Player>() != null)
		{
			foreach(GameObject x in spawners)
			{
				StartCoroutine(x.GetComponent<FireballSpawner>().SpawnAnObject());
			}
		}
	}
}
}
