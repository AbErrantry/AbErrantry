using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class RobotBoss : Boss
	{

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

		private FMOD.Studio.EventInstance robotMusic;
		private FMOD.Studio.EventInstance robotAttack;
		private FMOD.Studio.EventInstance robotDeath;
		private FMOD.Studio.EventInstance robotHurt;


		// Use this for initialization
		protected new void Start()
		{
			name = "Robot";
			canTakeDamage = true;
			isFacingRight = false;
			//player = GameObject.Find("Knight").GetComponent<Transform>();
			moveLoc = GameObject.Find("RobotMoveArea").GetComponent<BoxCollider2D>();
			attackTrigger = GameObject.Find("RobotAttackTrigger").GetComponent<BoxCollider2D>();
			cooldown = attackCooldown;

			BackgroundSwitch.instance.ResetSongs();

			robotAttack = FMODUnity.RuntimeManager.CreateInstance("event:/Robot/attack");
			robotAttack.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			robotDeath = FMODUnity.RuntimeManager.CreateInstance("event:/Robot/death");
			robotDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			
			robotHurt = FMODUnity.RuntimeManager.CreateInstance("event:/Robot/take_damage");
			robotDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			robotMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/boss/ice_boss");
			robotMusic.setVolume(PlayerPrefs.GetFloat("MusicVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			robotMusic.start();

			base.Start();
		}

		// Update is called once per frame
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
					PickAttack(3);
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
					StartCoroutine(ShootBackpack());
					break;
				case 2:
					StartCoroutine(ShootCrouch());
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
			float newLocX = Random.Range(moveLoc.bounds.min.x, moveLoc.bounds.max.x);
			float newLocY = player.transform.position.y;
			while (Time.time < startTime + 2.25f)
			{
				
				transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, newLocX, (Time.time - startTime) / 2.25f),
					Mathf.SmoothStep(transform.position.y, newLocY, (Time.time - startTime) / 2.25f), transform.position.z);

				yield return null;
			}

			yield return new WaitForSeconds(.5f);
			StartCoroutine(ShootBackpack());
		}

		private IEnumerator ShootStanding()
		{
			if (anim.GetBool("DoneStanding"))
			{
				anim.SetBool("DoneStanding", false);
			}
			float mult = -1;
			anim.Play("ShootStanding");
			yield return new WaitForSeconds(0.5f);

			for (int i = 0; i < snowballCount; i++)
			{
				SnowBallShooting(mult);
				robotAttack.start();
				yield return new WaitForSeconds(0.5f);
			}

			anim.SetBool("DoneStanding", true);

			StartCoroutine(Move());
			yield return new WaitForSeconds(.5f);
			isAttacking = false;
			StopAllCoroutines();
		}

		private void SnowBallShooting(float mult)
		{
			if (isFacingRight)
			{
				mult = 1;
			}
			else
			{
				mult = -1;
			}
			float forceMult = mult * Vector2.Distance(player.transform.position, transform.position);
			//Debug.Log(Vector2.Distance(player.transform.position, transform.position));
			GameObject clone = Instantiate(snowball, attackTrigger.bounds.center, Quaternion.identity);

			clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceMult * 52, 0));
		}

		private void BackPackShooting(float mult, bool rightJetShoot)
		{
			if (isFacingRight)
			{
				mult = 1;
			}
			else
			{
				mult = -1;
			}
			float forceMult = mult * Vector2.Distance(player.transform.position, transform.position);
			//Debug.Log(Vector2.Distance(player.transform.position, transform.position));
			GameObject clone;

			if (rightJetShoot)
			{
				clone = Instantiate(snowball, attackTrigger.bounds.min, Quaternion.identity);
				clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceMult * 10, forceMult * 27 * mult));
			}
			else
			{
				clone = Instantiate(snowball, attackTrigger.bounds.max, Quaternion.identity);
				clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceMult * 10, forceMult * 27 * mult));
			}

		}

		private IEnumerator ShootBackpack()
		{
			if (anim.GetBool("DoneBackpack"))
			{
				anim.SetBool("DoneBackpack", false);
			}
			float mult = -1;
			anim.Play("ShootBackpack");
			yield return new WaitForSeconds(0.5f);

			for (int i = 0; i < snowballCount * 2; i++)
			{
				robotAttack.start();
				robotAttack.start();
				BackPackShooting(mult, true);
				BackPackShooting(mult, false);
				yield return new WaitForSeconds(0.5f);
			}

			anim.SetBool("DoneBackpack", true);

			StartCoroutine(Move());
			yield return new WaitForSeconds(.5f);
			isAttacking = false;
			StopAllCoroutines();
		}

		private IEnumerator ShootCrouch()
		{
			if (anim.GetBool("DoneCrouch"))
			{
				anim.SetBool("DoneCrouch", false);
			}
			float mult = -1;
			anim.Play("ShootCrouch");
			yield return new WaitForSeconds(0.5f);

			for (int i = 0; i < snowballCount; i++)
			{
				robotAttack.start();
				SnowBallShooting(mult);
				yield return new WaitForSeconds(0.5f);
			}

			anim.SetBool("DoneCrouch", true);

			StartCoroutine(Move());
			yield return new WaitForSeconds(.5f);
			isAttacking = false;
			StopAllCoroutines();
		}

		private void Spawn()
		{
			anim.Play("Spawn");
		}
		protected override void Flinch()
		{
			base.Flinch();
			robotHurt.start();
			
			StopAllCoroutines();
			isAttacking = true;
			StartCoroutine(Move());
			
		}
		protected override void InitializeDeath()
		{
			anim.Play("Death");
			robotDeath.start();
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
			robotMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}
