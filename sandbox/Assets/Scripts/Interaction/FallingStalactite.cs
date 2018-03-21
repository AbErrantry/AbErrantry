using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStalactite : MonoBehaviour {

	public float dropTime;
	public bool isDropping;
	public GameObject itemToDrop;
	private float time;
	// Use this for initialization
	void Start ()
	{
		time = dropTime;
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
		
	}
}
