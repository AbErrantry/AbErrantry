using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class FallingStalagtyte : MonoBehaviour {


	public float fallTime;

	private float time;
	// Use this for initialization
	void Start ()
	{
		time = fallTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(time<= 0)
		{
			DropATyte();
			time = fallTime;
		}
		else
		{
			time -= Time.deltaTime;
		}
	}

	void DropATyte()
	{
		
	}
}
}
