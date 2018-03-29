using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : Boss
{
	protected new void Start()
	{
		name = "Golem";
		health = 100;
		base.Start();
	}

	protected new void Update()
	{

	}

	private void SwipeUp()
	{
		anim.Play("SwipeUp");
	}

	private void SwipeDown()
	{
		anim.Play("SwipeDown");
	}

	private void Smash()
	{
		anim.Play("Smash");
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
