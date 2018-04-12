using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class WraithBoss : Boss
	{
		public Collider2D attackTrigger;
		[Range(0, 30)]
		public float attackCooldown;

		public int damage;
		private float cooldown;
		//public int attackPicked;
		public bool isAttacking;

		[Header("Fireball Attack")]
		public GameObject fireball;
		public int fireballCount;
		public float minMaxForce;

		[Header("RainFire")]
		public Collider2D rainFireTrigger;
		public GameObject giantFireBall;
		public int giantFireCount;
		private Vector2 min;
		private Vector2 max;
		//public Transform player;
		private float startTime;
		//private bool isFacingRight;
		protected new void Start()
		{
			name = "Wraith";
			canTakeDamage = true;
			cooldown = attackCooldown;
			min = rainFireTrigger.bounds.min;
			max = rainFireTrigger.bounds.max;
			//player = GameObject.Find("Knight").GetComponent<Transform>();
			startTime = Time.time;
			isFacingRight = false;
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

			if (cooldown <= 0 && !isAttacking)
			{
				isAttacking = true;
				if ((currentVitality / maxVitality) * 100 >= 75)
				{
					PickAttack(1);
				}
				else if ((currentVitality / maxVitality) * 100 >= 50)
				{
					PickAttack(2);
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
					Laugh();
					break;
				case 1:
					StartCoroutine(Dash());
					break;
				case 2:
					StartCoroutine(Fireball());
					break;
				case 3:
					StartCoroutine(RainFire());
					break;
				default:
					Walk();
					break;
			}

		}
		private void Walk()
		{
			anim.Play("Wraith_Walk");
		}
		private IEnumerator Dash()
		{
			if (anim.GetBool("EndDash"))
			{
				anim.SetBool("EndDash", false);
			}

			anim.Play("Wraith_Dash");
			startTime = Time.time;
			while (Time.time < startTime + 1.25f)
			{
				if (isFacingRight)
				{
					transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, transform.position.x + 0.1f, (Time.time - startTime) / 1.25f), transform.position.y, transform.position.z);
				}
				else
				{
					transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, transform.position.x - 0.1f, (Time.time - startTime) / 1.25f), transform.position.y, transform.position.z);
				}

				yield return null;
			}

			anim.SetBool("EndDash", true);
		}

		private IEnumerator Fireball()
		{
			if (anim.GetBool("EndFireball"))
			{
				anim.SetBool("EndFireball", false);
			}

			anim.Play("Wraith_WindupFireball");

			yield return new WaitForSeconds(1);
			for (int i = 0; i < fireballCount; i++)
			{
				GameObject clone = Instantiate(fireball, attackTrigger.bounds.center, Quaternion.identity);

				clone.GetComponent<Rigidbody2D>().AddForce(
					new Vector2(Random.Range(minMaxForce * -1, minMaxForce) * 10, Random.Range(minMaxForce * -1, minMaxForce) * 10));
				clone = Instantiate(fireball, attackTrigger.bounds.center, Quaternion.identity);
				clone.GetComponent<Rigidbody2D>().AddForce(
					new Vector2(Random.Range(minMaxForce * -1, minMaxForce) * 10, Random.Range(minMaxForce * -1, minMaxForce) * 10));
				i++;
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
			if (anim.GetBool("EndRainFire"))
			{
				anim.SetBool("EndRainFire", false);
			}

			anim.Play("Wraith_RainFire");

			for (int i = 0; i < giantFireCount; i++)
			{
				Instantiate(giantFireBall, new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y)), Quaternion.identity);
			}

			yield return new WaitForSeconds(3f);
			anim.SetBool("EndRainFire", true);
			yield return new WaitForSeconds(3f);
			anim.SetBool("EndRainFire", false);
			yield return new WaitForFixedUpdate();

			isAttacking = false;
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
