using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{

public class Horseman : Boss 
{
	[Range(0,15)]
	public float attackCooldown;
	public Collider2D attackTrigger;
	public int damage;
	private float cooldown;
	private int attackPicked;
	public bool isAttacking;
	public LavaPlume[] plumes;
	public LavaPlume[] bigPlumes;
	protected new void Start()
	{
		name = "Horseman";
		canTakeDamage = true; 
		cooldown = attackCooldown;
		isAttacking = false;
		attackTrigger = gameObject.GetComponentInChildren<BoxCollider2D>();
		base.Start();
	}

	protected new void Update()
	{
		if (player.position.x >= transform.position.x)
		{
			isFacingRight = true;
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		else
		{
			isFacingRight = false;
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

		//Debug.Log("Min: "+ min.ToString() + " Max: " + max.ToString());
		if(cooldown<=0 && !isAttacking)
		{
			if((currentVitality/maxVitality)*100 >= 75)
			{
				PickAttack(1);//just put the amount of cases there are in pickAttack()
			}
			else if((currentVitality/maxVitality)*100 >= 50)
			{
				PickAttack(1);
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
		isAttacking=true;
		switch(Random.Range(0, attackLevel +1))
		{
			case 0:
			StartCoroutine(Attack1());
			break;
			case 1:
			StartCoroutine(Attack2());
			break;
		}
	}

	private IEnumerator Attack1()
	{
		if(!anim.GetBool("Attack1"))
		{
			anim.SetBool("Attack1",true);
		}
		anim.Play("Attack1");
		yield return new WaitForSeconds(1f);
		Plume();
		yield return new WaitForSeconds(.5f);
		
		yield return new WaitForSeconds(2);
		UnPlume();

		yield return new WaitForSeconds(.5f);
		anim.SetBool("Attack1",false);
		isAttacking = false;
		StopAllCoroutines();

	}

	private IEnumerator Attack2()
	{
		int plumeNum = Random.Range(0,bigPlumes.Length +1);
		if(!anim.GetBool("Attack2"))
		{
			anim.SetBool("Attack2",true);
		}
		anim.Play("Attack2");
		yield return new WaitForSeconds(1f);
		Plume();
		yield return new WaitForSeconds(.5f);
		bigPlumes[plumeNum].PlumeFake();
		yield return new WaitForSeconds(2f);

		for(int i = 0; i<bigPlumes.Length;i++)
		{
			if(i != plumeNum)
			{
				bigPlumes[i].PlumeIt();
			}
		}
		yield return new WaitForSeconds(1);
		for(int i = 0; i<bigPlumes.Length;i++)
		{
			if(i != plumeNum)
			{
				bigPlumes[i].UnPlumeIt();
			}
		}
		yield return new WaitForSeconds(1);

		UnPlume();

		yield return new WaitForSeconds(.5f);
		anim.SetBool("Attack2",false);
		isAttacking = false;
		StopAllCoroutines();
	}

	private void Plume()
	{
		for(int i =0; i<plumes.Length; i++)
		{
			plumes[i].PlumeIt();
		}
	}

	private void UnPlume()
	{
		for(int i = 0; i<plumes.Length; i++)
		{
			plumes[i].UnPlumeIt();
		}
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
