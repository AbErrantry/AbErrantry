using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class GolemBoss : Boss
{
	public GameObject hand1;
	public GameObject hand2;
	public GameObject eyes;
	public Collider2D DownSwipeBounds;
	public Collider2D UpSwipeBounds;
	public Collider2D SmashBounds;
	
	[Range(0,30)]
	public float attackCooldown;

	public int damage;
	private float cooldown;
	//public int attackPicked;
	public bool isAttacking;
	private float startTime;
	protected new void Start()
	{
		name = "Golem";
		cooldown = attackCooldown;
		canTakeDamage = true;
		startTime = Time.time;
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
			StartCoroutine(Smash());
			break;
			case 1:
			StartCoroutine(SwipeDown());
			break;
			case 2:
			StartCoroutine(SwipeUp());
			break;
		}
		
	}

	private IEnumerator SwipeUp()
	{
		Vector3 hand1Orig = hand1.transform.position;
		Vector3 hand2Orig = hand2.transform.position;
		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, UpSwipeBounds.bounds.min.x - hand1.GetComponentInChildren<Collider2D>().bounds.size.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, UpSwipeBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);

			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, UpSwipeBounds.bounds.max.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, UpSwipeBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);														
			yield return null;
		}

		yield return new WaitForSeconds(1);
		anim.Play("UpSwipe");

		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, UpSwipeBounds.bounds.min.x, (Time.time - startTime)/2.25f)
															,hand2.transform.position.y, transform.position.z);
			yield return null;
		}

		yield return new WaitForSeconds(1);
		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);

			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);														
			yield return null;
		}
		isAttacking = false;
		StopAllCoroutines();
	}

	private IEnumerator SwipeDown()
	{
		Vector3 hand1Orig = hand1.transform.position;
		Vector3 hand2Orig = hand2.transform.position;
		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, DownSwipeBounds.bounds.min.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, DownSwipeBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);

			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, DownSwipeBounds.bounds.max.x + hand2.GetComponentInChildren<Collider2D>().bounds.size.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, DownSwipeBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);														
			yield return null;
		}
		yield return new WaitForSeconds(1);
		anim.Play("DownSwipe");

		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, DownSwipeBounds.bounds.max.x, (Time.time - startTime)/2.25f)
															,hand1.transform.position.y, transform.position.z);
			yield return null;
		}
	yield return new WaitForSeconds(2);
		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);

			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);														
			yield return null;
		}
		isAttacking = false;
		StopAllCoroutines();
	}

	private IEnumerator Smash()
	{
		Vector3 hand1Orig = hand1.transform.position;
		Vector3 hand2Orig = hand2.transform.position;
		Vector3 hand1NewLoc = new Vector3(Random.Range(SmashBounds.bounds.min.x, SmashBounds.bounds.center.x), SmashBounds.bounds.max.y, transform.position.z);
		Vector3 hand2NewLoc = new Vector3(Random.Range(SmashBounds.bounds.center.x, SmashBounds.bounds.max.x), SmashBounds.bounds.max.y, transform.position.z);
		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1NewLoc.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, hand1NewLoc.y, (Time.time - startTime)/2.25f), 
															transform.position.z);
			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2NewLoc.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, hand2NewLoc.y, (Time.time - startTime)/2.25f), 
															transform.position.z);
																							
			yield return null;
		}

		yield return new WaitForSeconds(0.5f);

		anim.Play("Smash");

		startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(hand1.transform.position.x, Mathf.SmoothStep(hand1.transform.position.y, SmashBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);
			hand2.transform.position = new Vector3(hand2.transform.position.x, Mathf.SmoothStep(hand2.transform.position.y, SmashBounds.bounds.min.y, (Time.time - startTime)/2.25f), 
															transform.position.z);
																							
			yield return null;
		}
		
			startTime = Time.time;
		while(Time.time < startTime + 2.25f)
		{
            hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);

			hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime)/2.25f)
															,Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime)/2.25f), 
															transform.position.z);														
			yield return null;
		}
		isAttacking = false;
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
