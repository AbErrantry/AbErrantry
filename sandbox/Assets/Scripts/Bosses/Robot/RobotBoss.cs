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

	[Header("Robot Moving")]
	public Collider2D moveLoc;

	[Header("Shooting Standing")]
	public int snowballCount;
	public GameObject snowball;
	public float minMaxForce;

	// Use this for initialization
	protected new void Start ()
	{
		name = "Robot";
		canTakeDamage = true;
		isFacingRight = false;
		//player = GameObject.Find("Knight").GetComponent<Transform>();
		moveLoc = GameObject.Find("RobotMoveArea").GetComponent<BoxCollider2D>();
		attackTrigger = GameObject.Find("RobotAttackTrigger").GetComponent<BoxCollider2D>();
		cooldown = attackCooldown;
		base.Start();
	}
	
	// Update is called once per frame
	protected new void Update () 
	{

		if (cooldown <= 0 && !isAttacking)
		{
			isAttacking = true;
			if ((currentVitality / maxVitality) * 100 >= 75)
			{
				PickAttack(1);
			}
			else if ((currentVitality / maxVitality) * 100 >= 50)
			{
				PickAttack(3);
			}
			else
			{
				PickAttack(3);
			}
			cooldown = attackCooldown;

		}
		else if (isAttacking)
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

			switch (Random.Range(0, attackLevel + 1))
			{
				case 0:
					StartCoroutine(ShootStanding());
					break;
				case 1:
					StartCoroutine(ShootStanding());
					break;
				case 2:
					ShootCrouch();
					break;
				case 3:
					StartCoroutine(Move());
					
					break;
				default:
					
					break;
			}

		}
	
	private IEnumerator Move()
	{
        float startTime = Time.time;
		float newLocX = Random.Range(moveLoc.bounds.min.x,moveLoc.bounds.max.x);
		float newLocY = Random.Range(moveLoc.bounds.min.y,moveLoc.bounds.max.y);
        while (Time.time < startTime + 2.25f)
		{
			
			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, newLocX, (Time.time - startTime) / 2.25f),
											 Mathf.SmoothStep(transform.position.y, newLocY, (Time.time - startTime) / 2.25f), transform.position.z);

			yield return null;
		}

		yield return new WaitForSeconds(2);
		ShootBackpack();
	}

	private IEnumerator ShootStanding()
	{
		float mult = -1;
		anim.Play("ShootStanding");
		yield return new WaitForSeconds(0.5f);
			
		if(isFacingRight)
		{
			mult = 1;
		}
		else
		{
			mult=-1;
		}

		for (int i = 0; i < snowballCount; i++)
		{
			SnowBallShooting(mult);
			yield return new WaitForSeconds(0.5f);
		}
		
		yield return new WaitForSeconds(1);
		isAttacking = false;
		StopAllCoroutines();
	}

	private void SnowBallShooting(float mult)
	{
		float forceMult = mult * Vector2.Distance(player.transform.position, transform.position);
		Debug.Log(Vector2.Distance(player.transform.position, transform.position));
		GameObject clone = Instantiate(snowball, attackTrigger.bounds.center, Quaternion.identity);

		clone.GetComponent<Rigidbody2D>().AddForce (new Vector2(forceMult*10,0));
	}
	private void ShootBackpack()
	{
		anim.Play("ShootBackpack");
	}

	private void ShootCrouch()
	{
		anim.Play("ShootCrouch");
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
