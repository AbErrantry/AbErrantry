using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class OgreBoss : Boss
{
	[Range(0,15)]
	public float attackCooldown;
	public Collider2D posPositions;
	public Collider2D attackTrigger;
	public int punchDamage;
	private float cooldown;
	private int attackPicked;
	private bool isAttacking;
	private Vector2 min;
	private Vector2 max;
	

	protected new void Start()
	{
		name = "Ogre";
		canTakeDamage = true; 
		cooldown = attackCooldown;
		isAttacking = false;
		min = posPositions.bounds.min;
		max = posPositions.bounds.max;
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
				PickAttack(1);
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
			isAttacking=false;
			break;
			case 1:
			Punch();
			isAttacking=false;
			break;
			case 2:
			StartCoroutine(MoveAway());
			break;
		}
	}

	public IEnumerator MoveAway()
	{
		isAttacking=true;
		LowerWater();
		yield return new WaitForSeconds(2.3f);
		NewPosition();
		//RaiseWater();
		yield return new WaitForSeconds(2f);
		gameObject.GetComponent<Animator>().SetBool("Moved", false);
		isAttacking=false;
	}

	public void NewPosition()
	{
		gameObject.transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
		gameObject.GetComponent<Animator>().SetBool("Moved", true);
	}

	public void RaiseWater()
	{
		anim.Play("Raise");
	}

	public void LowerWater()
	{
		anim.Play("Lower");
	}

	private void Punch()
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

	public void ApplyDamage(GameObject target)
	{
		target.GetComponent<Attackable>().TakeDamage(gameObject, punchDamage);
	}
}
}
