using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBoss : Boss
{
	bool test;
	bool test2;

	protected new void Start()
	{
		name = "Ogre";
		health = 100;
		base.Start();
		test = false;
		test2 = false;
	}

	protected new void Update()
	{
		if (!test && Time.time > 10.0f)
		{
			Punch();
			test = true;
		}
		else if (!test2 && Time.time > 20.0f)
		{
			RaiseWater();
			test2 = true;
		}
	}

	public void RaiseWater()
	{
		anim.Play("Raise");
	}

	public void LowerWater()
	{
		anim.Play("Lower");
	}

	public void Punch()
	{
		anim.Play("Punch");
	}

	public void Hurt()
	{
		anim.Play("Hurt");
	}

	protected override void InitializeDeath()
	{
		anim.Play("Death");
	}

	public override void FinalizeDeath()
	{
		BossDefeated();
	}
}
