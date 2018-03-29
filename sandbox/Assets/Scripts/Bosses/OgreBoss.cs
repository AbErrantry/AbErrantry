using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBoss : Boss
{

	protected new void Start()
	{
		name = "Ogre";
		base.Start();
		canTakeDamage = true;
	}

	protected new void Update()
	{

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
