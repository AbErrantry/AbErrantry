using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour {

	public float spawnTime;
	public int numToSpawn;
	public bool requireInput;
	public GameObject itemToSpawn;
	public float minMaxForce;
	private float time;
	private bool isFiring;
	// Use this for initialization
	void Start () 
	{
		time = spawnTime;	
		isFiring = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isFiring)
		{
			if(time<=0)
			{
				isFiring = true;
				StartCoroutine(SpawnAnObject());
				time = spawnTime;
			}
			else
			{
				time -= Time.deltaTime;
			}
		}
	}

	public IEnumerator SpawnAnObject()
	{
		for(int i = 0; i < numToSpawn; i++)
		{
			GameObject clone = Instantiate(itemToSpawn, transform.position, Quaternion.identity);

			clone.GetComponent<Rigidbody2D>().AddForce(
											new Vector2(Random.Range(minMaxForce*-1, minMaxForce),Random.Range(minMaxForce*-1, minMaxForce)));

			yield return new WaitForSeconds(1);
		}

		isFiring = false;
	}
}
