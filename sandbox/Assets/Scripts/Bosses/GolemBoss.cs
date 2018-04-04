using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class GolemBoss : Boss
{
	public Collider2D hand1;
	public Collider2D hand2;
	public Collider2D eyes;
		[Range(0,30)]
	public float attackCooldown;

	public int damage;
	private float cooldown;
	//public int attackPicked;
	public bool isAttacking;
	protected new void Start()
	{
		name = "Golem";
		cooldown = attackCooldown;
		canTakeDamage = true;
		base.Start();
	}

	protected new void Update()
	{
		
		if(cooldown<=0 && !isAttacking)
		{
			isAttacking=true;
			if((currentVitality/maxVitality)*100 >= 75)
			{
				PickAttack(2);
			}
			else if((currentVitality/maxVitality)*100 >= 50)
			{
				PickAttack(2);
			}
			else
			{
				PickAttack(2);
			}
			cooldown = attackCooldown;
			
		}
		else if(isAttacking)
		{
			cooldown = attackCooldown;
		}
		else
		{
			cooldown -= Time.deltaTime;
		}
	}

	
	public void PickAttack(int attackLevel)
	{
		
		switch(Random.Range(0, attackLevel +1))
		{
			case 0:
			Smash();
			break;
			case 1:
			SwipeDown();
			break;
			case 2:
			SwipeUp();
			break;
		}
		
	}

	private void SwipeUp()
	{
		anim.Play("UpSwipe");
	}

	private void SwipeDown()
	{
		anim.Play("DownSwipe");
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

	public void ApplyDamage(GameObject target)
	{
		target.GetComponent<Attackable>().TakeDamage(gameObject, damage);
	}
}
}
