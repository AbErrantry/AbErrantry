using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour {

	public bool spawnLarge;
	public float spawnTime;
	public GameObject smallSnowball;
	public GameObject largeSnowball;
	private Collider2D spawnLoc;
	private GameObject snowball;
	private float countdown;
	// Use this for initialization
	void Start () 
	{
		if(spawnLarge)
		{
			snowball = largeSnowball;
		}
		else
		{
			snowball = smallSnowball;
		}
		countdown = spawnTime;
		spawnLoc = gameObject.GetComponent<BoxCollider2D>();
	}

	public void Update()
	{
		if(countdown <= 0)
		{
			SpawnSnowball();
			countdown = spawnTime;
		}
		else
		{
			countdown -= Time.deltaTime;
		}
	}

	public void SpawnSnowball()
	{
		Instantiate(snowball,spawnLoc.bounds.center,Quaternion.identity);
	}

}
