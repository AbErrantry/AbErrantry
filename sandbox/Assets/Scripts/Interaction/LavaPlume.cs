using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPlume : MonoBehaviour {

	public float delay;
	private float time;

	// Use this for initialization
	void Start ()
	{
		time = delay;
		gameObject.GetComponent<Animator>().SetFloat("Delay", time);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(time<=0)
		{
			gameObject.GetComponent<Animator>().SetFloat("Delay", time);
			time = delay;
			
		}
		else
		{
			time -= Time.deltaTime;
			gameObject.GetComponent<Animator>().SetFloat("Delay", time);
		}
	}
}
