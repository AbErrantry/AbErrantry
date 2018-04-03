using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithBoss : Boss 
{
	//public Collider2D attackTrigger;
	[Range(0,30)]
	public float attackCooldown;
	private float cooldown;
	private int attackPicked;
	private bool isAttacking;
	protected new void Start()
	{
		name = "Wraith";
		canTakeDamage = true;

		base.Start();
		
	}
	
	protected new void Update()
	{

	}

	private void Walk()
	{
		anim.Play("Wraith_Walk");
	}
	private void Dash()
	{
		anim.Play("Wraith_Dash");
	}

	private void Fireball()
	{
		anim.Play("Wraith_Fireball");
	}

	private void RainFire()
	{
		anim.Play("Wraith_RainFire");
	}

	private void Laugh()
	{
		anim.Play("Wraith_Laugh");
	}
	
	private void Spawn()
	{
		anim.Play("Wraith_Spawn");
	}
	protected override void InitializeDeath()
	{
		anim.Play("Wraith_Death");
	}

	public override void FinalizeDeath()
	{
		BossDefeated();
	}
}
