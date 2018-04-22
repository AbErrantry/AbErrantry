using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{

	public class Horseman : Boss
	{
		[Range(0, 15)]
		public float attackCooldown;
		public Collider2D attackTrigger;
		public int damage;
		private float cooldown;
		private int attackPicked;
		public bool isAttacking;
		public LavaPlume[] plumes;
		public LavaPlume[] bigPlumes;
		public Platform leftPlatform;
		public Platform rightPlatform;

		private FMOD.Studio.EventInstance horsemanMusic;
		private FMOD.Studio.EventInstance horsemanAttack;
		private FMOD.Studio.EventInstance horsemanDeath;
		private FMOD.Studio.EventInstance horsemanHurt;

		protected new void Start()
		{
			name = "Horseman";
			canTakeDamage = true;
			cooldown = attackCooldown;
			isAttacking = false;
			attackTrigger = gameObject.GetComponentInChildren<BoxCollider2D>();

			BackgroundSwitch.instance.ResetSongs();
			
			horsemanAttack = FMODUnity.RuntimeManager.CreateInstance("event:/Horseman/attack");
			horsemanAttack.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			horsemanDeath = FMODUnity.RuntimeManager.CreateInstance("event:/Horseman/death");
			horsemanDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			
			horsemanHurt = FMODUnity.RuntimeManager.CreateInstance("event:/Horseman/take_damage");
			horsemanDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			horsemanMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/boss/lava_boss");
			horsemanMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			horsemanMusic.start();

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
			if (cooldown <= 0 && !isAttacking)
			{
				PickAttack(1); //just put the amount of cases there are in pickAttack()

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
			isAttacking = true;
			switch (Random.Range(0, attackLevel + 1))
			{
				case 0:
					StartCoroutine(Attack1());
					break;
				case 1:
					StartCoroutine(Attack2());
					break;
			}
		}

		//platforms opening
		private IEnumerator Attack1()
		{
			if (!anim.GetBool("Attack1"))
			{
				anim.SetBool("Attack1", true);
			}
			anim.Play("Attack1");
			horsemanAttack.start();
			yield return new WaitForSeconds(1f);
			Plume();
			yield return new WaitForSeconds(3f);
			float startTime = Time.time;
			while (Time.time < startTime + 10f)
			{

				leftPlatform.transform.position = new Vector3(Mathf.SmoothStep(leftPlatform.transform.position.x, leftPlatform.endLoc.x, (Time.time - startTime) / 10f),
					leftPlatform.transform.position.y, transform.position.z);
				rightPlatform.transform.position = new Vector3(Mathf.SmoothStep(rightPlatform.transform.position.x, rightPlatform.endLoc.x, (Time.time - startTime) / 10f),
					rightPlatform.transform.position.y, transform.position.z);
				yield return null;
			}

			yield return new WaitForSeconds(5);

			startTime = Time.time;
			while (Time.time < startTime + 10f)
			{

				leftPlatform.transform.position = new Vector3(Mathf.SmoothStep(leftPlatform.transform.position.x, leftPlatform.startLoc.x, (Time.time - startTime) / 10f),
					leftPlatform.transform.position.y, transform.position.z);
				rightPlatform.transform.position = new Vector3(Mathf.SmoothStep(rightPlatform.transform.position.x, rightPlatform.startLoc.x, (Time.time - startTime) / 10f),
					rightPlatform.transform.position.y, transform.position.z);
				yield return null;
			}

			UnPlume();

			yield return new WaitForSeconds(.5f);
			anim.SetBool("Attack1", false);
			isAttacking = false;
			StopAllCoroutines();

		}

		//plumes attacking
		private IEnumerator Attack2()
		{
			int plumeNum = Random.Range(0, bigPlumes.Length + 1);
			if (!anim.GetBool("Attack2"))
			{
				anim.SetBool("Attack2", true);
			}
			anim.Play("Attack2");
			horsemanAttack.start();
			yield return new WaitForSeconds(1f);
			Plume();
			yield return new WaitForSeconds(.5f);
			bigPlumes[plumeNum].PlumeFake();
			yield return new WaitForSeconds(3f);

			for (int i = 0; i < bigPlumes.Length; i++)
			{
				if (i != plumeNum)
				{
					bigPlumes[i].PlumeIt();
				}
			}
			yield return new WaitForSeconds(2);
			for (int i = 0; i < bigPlumes.Length; i++)
			{
				if (i != plumeNum)
				{
					bigPlumes[i].UnPlumeIt();
				}
			}
			yield return new WaitForSeconds(1);

			UnPlume();

			yield return new WaitForSeconds(.5f);
			anim.SetBool("Attack2", false);
			isAttacking = false;
			StopAllCoroutines();
		}

		private void Plume()
		{
			for (int i = 0; i < plumes.Length; i++)
			{
				plumes[i].PlumeIt();
			}
		}

		private void UnPlume()
		{
			for (int i = 0; i < plumes.Length; i++)
			{
				plumes[i].UnPlumeIt();
			}
		}

		protected override void Flinch()
		{
			base.Flinch();
			horsemanHurt.start();
			
			if(!isAttacking)
			{
				StopAllCoroutines();
				isAttacking = true;
				StartCoroutine(Attack2());
			}
		}

		protected override void InitializeDeath()
		{
			anim.Play("Death");
			horsemanDeath.start();
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
			horsemanMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}
