using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBoss : Boss
{
	protected new void Start()
	{
		name = "Ogre";
		health = 100;
		base.Start();
	}

	protected new void Update()
	{

	}

	public void RaiseWater()
	{
		anim.Play("RaiseWater");
	}

	public void LowerWater()
	{
		anim.Play("LowerWater");
	}

	public void Punch()
	{
		anim.Play("Punch");
	}
}
