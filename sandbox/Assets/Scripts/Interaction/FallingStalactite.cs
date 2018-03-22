using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class FallingStalactite : MonoBehaviour {

	public float dropTime;
	public bool randomizeDrop;
	public GameObject itemToDrop;
	public float stalactiteDamage;
	public float destoryDelay;
	private float time;

	[Header("CameraShaking")]

	public float camShakeIntensity;
	public float camShakeTime;
	private CameraShake camShake;
	// Use this for initialization
	void Start ()
	{
		if(randomizeDrop)
		{
			dropTime = Random.Range(1.5f,10.75f);
			time = dropTime;
		}
		else
		{
			time = dropTime;
		}

		camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(time<= 0)
		{
			time = dropTime;
			DropATite();
		}
		else
		{
			time -= Time.deltaTime;
		}
	}

	void DropATite()
	{
		if(!camShake.isShaking)
		{
			camShake.isShaking = true;
			StartCoroutine(camShake.Shaker(camShakeIntensity,camShakeTime));
		}
		GameObject clone = Instantiate(itemToDrop,transform.position,Quaternion.identity);
		clone.GetComponent<StalactiteHit>().SetVar(stalactiteDamage, gameObject, destoryDelay);
	}
}
}
