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

		private FMOD.Studio.EventInstance golemMusic;

		private FMOD.Studio.EventInstance golemAttack;
		private FMOD.Studio.EventInstance golemDeath;
		private FMOD.Studio.EventInstance golemHurt;

		[Range(0, 30)]
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

			BackgroundSwitch.instance.ResetSongs();

			golemAttack = FMODUnity.RuntimeManager.CreateInstance("event:/Golem/attack");
			golemAttack.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			golemDeath = FMODUnity.RuntimeManager.CreateInstance("event:/Golem/death");
			golemDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			
			golemHurt = FMODUnity.RuntimeManager.CreateInstance("event:/Golem/take_damage");
			golemDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			golemMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/boss/cave_boss");
			golemMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			golemMusic.start();

			base.Start();
		}

		protected new void Update()
		{
			if (cooldown <= 0 && !isAttacking)
			{
				isAttacking = true;
				if ((currentVitality / maxVitality) * 100 >= 75)
				{
					PickAttack(2);
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
					StartCoroutine(Move());
					break;
				case 1:
					StartCoroutine(Smash());
					break;
				case 2:
					StartCoroutine(SwipeUp());
					break;
				case 3:
					StartCoroutine(SwipeDown());
					break;
			}

		}
		private IEnumerator Move()
		{
			Vector3 NewLoc = new Vector3(Random.Range(SmashBounds.bounds.min.x, SmashBounds.bounds.max.x), Random.Range(SmashBounds.bounds.min.y, SmashBounds.bounds.max.y),
				transform.position.z);
			startTime = Time.time;
			while (Time.time < startTime + 3f)
			{
				gameObject.transform.position = new Vector3(Mathf.SmoothStep(gameObject.transform.position.x, NewLoc.x, (Time.time - startTime) / 3f), Mathf.SmoothStep(gameObject.transform.position.y, NewLoc.y, (Time.time - startTime) / 3f),
					transform.position.z);
				yield return null;
			}
			isAttacking = false;
			
			StopAllCoroutines();
			//PickAttack(3);
		}

		private IEnumerator SwipeUp()
		{
			Vector3 hand1Orig = hand1.transform.position;
			Vector3 hand2Orig = hand2.transform.position;
			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, UpSwipeBounds.bounds.min.x - hand1.GetComponentInChildren<Collider2D>().bounds.size.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand1.transform.position.y, UpSwipeBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);

				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, UpSwipeBounds.bounds.max.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand2.transform.position.y, UpSwipeBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);
				yield return null;
			}

			yield return new WaitForSeconds(1f);
			golemAttack.start();
			anim.Play("UpSwipe");

			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, UpSwipeBounds.bounds.min.x, (Time.time - startTime) / 2.25f), hand2.transform.position.y, transform.position.z);
				yield return null;
			}

			yield return new WaitForSeconds(.5f);
			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime) / 2.25f),
					transform.position.z);

				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime) / 2.25f),
					transform.position.z);
				yield return null;
			}

			StartCoroutine(Move());
			yield return new WaitForSeconds(1f);
			isAttacking = false;
			StopAllCoroutines();
		}

		private IEnumerator SwipeDown()
		{
			Vector3 hand1Orig = hand1.transform.position;
			Vector3 hand2Orig = hand2.transform.position;
			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, DownSwipeBounds.bounds.min.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand1.transform.position.y, DownSwipeBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);

				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, DownSwipeBounds.bounds.max.x + hand2.GetComponentInChildren<Collider2D>().bounds.size.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand2.transform.position.y, DownSwipeBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			golemAttack.start();
			anim.Play("DownSwipe");

			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, DownSwipeBounds.bounds.max.x, (Time.time - startTime) / 2.25f), hand1.transform.position.y, transform.position.z);
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime) / 2.25f),
					transform.position.z);

				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime) / 2.25f), Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime) / 2.25f),
					transform.position.z);
				yield return null;
			}

			StartCoroutine(Move());
			yield return new WaitForSeconds(1f);
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
			while (Time.time < startTime + 1.5f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1NewLoc.x, (Time.time - startTime) / 1.5f), Mathf.SmoothStep(hand1.transform.position.y, hand1NewLoc.y, (Time.time - startTime) / 1.5f),
					transform.position.z);
				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2NewLoc.x, (Time.time - startTime) / 1.5f), Mathf.SmoothStep(hand2.transform.position.y, hand2NewLoc.y, (Time.time - startTime) / 1.5f),
					transform.position.z);

				yield return null;
			}

			yield return new WaitForSeconds(1f);

			anim.Play("Smash");
			golemAttack.start();
			startTime = Time.time;
			while (Time.time < startTime + 2.25f)
			{
				hand1.transform.position = new Vector3(hand1.transform.position.x, Mathf.SmoothStep(hand1.transform.position.y, SmashBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);
				hand2.transform.position = new Vector3(hand2.transform.position.x, Mathf.SmoothStep(hand2.transform.position.y, SmashBounds.bounds.min.y, (Time.time - startTime) / 2.25f),
					transform.position.z);

				yield return null;
			}

			startTime = Time.time;
			while (Time.time < startTime + 1.5f)
			{
				hand1.transform.position = new Vector3(Mathf.SmoothStep(hand1.transform.position.x, hand1Orig.x, (Time.time - startTime) / 1.5f), Mathf.SmoothStep(hand1.transform.position.y, hand1Orig.y, (Time.time - startTime) / 1.5f),
					transform.position.z);

				hand2.transform.position = new Vector3(Mathf.SmoothStep(hand2.transform.position.x, hand2Orig.x, (Time.time - startTime) / 1.5f), Mathf.SmoothStep(hand2.transform.position.y, hand2Orig.y, (Time.time - startTime) / 1.5f),
					transform.position.z);
				yield return null;
			}
			isAttacking = false;
		}

		protected override void Flinch()
		{
			base.Flinch();
			golemHurt.start();
			
			StopAllCoroutines();
			isAttacking = true;
			StartCoroutine(Move());
			
		}

		protected override void InitializeDeath()
		{
			anim.Play("Death");
			golemDeath.start();
		}

		public override void FinalizeDeath()
		{
			BossDefeated();
		}

		public void ApplyDamage(GameObject target)
		{
			target.GetComponent<Attackable>().TakeDamage(gameObject, damage);
		}

		private void OnDestroy()
		{
			golemMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}
