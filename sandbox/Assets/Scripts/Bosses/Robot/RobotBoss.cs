using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class RobotBoss : Boss {

	public Collider2D attackTrigger;
	[Range(0, 30)]
	public float attackCooldown;

	public int damage;
	private float cooldown;
	public bool isAttacking;

	// Use this for initialization
	protected new void Start ()
	{
		name = "Robot";
		canTakeDamage = true;
		isFacingRight = false;
		//player = GameObject.Find("Knight").GetComponent<Transform>();

		base.Start();
	}
	
	// Update is called once per frame
	protected new void Update () 
	{
		
	}

	private void Spawn()
	{
		anim.Play("Spawn");
	}

	protected override void InitializeDeath()
	{
		anim.Play("Death");
	}

	public override void FinalizeDeath()
	{
		BossDefeated();
	}

	public void ApplyDamage(GameObject target)
		{
			target.GetComponent<Attackable>().TakeDamage(gameObject, damage);
		}
}
}
