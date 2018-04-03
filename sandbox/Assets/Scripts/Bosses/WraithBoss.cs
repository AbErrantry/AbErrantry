using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class WraithBoss : Boss 
{
	//public Collider2D attackTrigger;
	[Range(0,30)]
	public float attackCooldown;

	public int damage;
	public float cooldown;
	public int attackPicked;
	public bool isAttacking;
	public GameObject fireball;
	public int fireballCount;
	public float minMaxForce;
	protected new void Start()
	{
		name = "Wraith";
		canTakeDamage = true;
		cooldown = attackCooldown;
		base.Start();
		
	}
	
	protected new void Update()
	{

		if(cooldown<=0 && !isAttacking)
		{
			PickAttack(3);
			if((currentVitality/maxVitality)*100 >= 75)
			{
				PickAttack(3);
			}
			else if((currentVitality/maxVitality)*100 >= 50)
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
		isAttacking=true;
		switch(Random.Range(0, attackLevel +1))
		{
			case 0:
			Laugh();
			break;
			case 1:
			Dash();
			break;
			case 2:
			StartCoroutine(Fireball());
			break;
			case 3:
			RainFire();
			break;
		}
		
	}
	private void Walk()
	{
		anim.Play("Wraith_Walk");
	}
	private void Dash()
	{
		anim.Play("Wraith_Dash");
	}

	private IEnumerator Fireball()
	{
		if(anim.GetBool("EndFireball"))
		{
			anim.SetBool("EndFireball", false);
		}
		
		anim.Play("Wraith_WindupFireball");
		yield return new WaitForSeconds(1);
		for(int i = 0; i < fireballCount; i++)
		{
			GameObject clone = Instantiate(fireball, transform.position, Quaternion.identity);

			clone.GetComponent<Rigidbody2D>().AddForce(
											new Vector2(Random.Range(minMaxForce*-1, minMaxForce)*10,Random.Range(minMaxForce*-1, minMaxForce)*10));

			yield return new WaitForSeconds(1);
		}
		
		yield return new WaitForSeconds(2);
		anim.SetBool("EndFireball", true);
		yield return new WaitForFixedUpdate();
		isAttacking = false;
		StopAllCoroutines();
		
	}

	private IEnumerator RainFire()
	{
		anim.Play("Wraith_RainFire");
		anim.SetBool("EndRainFire", true);
		yield return new WaitForFixedUpdate();
		anim.SetBool("EndRainFire", false);
		yield return new WaitForFixedUpdate();
		StopAllCoroutines();
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

	public void ApplyDamage(GameObject target)
	{
		target.GetComponent<Attackable>().TakeDamage(gameObject, damage);
	}
}
}
