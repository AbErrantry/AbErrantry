using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class ExplodingCrate : Crate
	{
		public bool canHitAttack;
		private float explodeDamage;
		private float explodeTime;
		public ExplodingTrigger explodingTrigger;

		protected FMOD.Studio.EventInstance crateExplosion;

		public new void Start()
		{
			base.Start();
			canFlinch = false;
			canHitAttack = false;
			explodeDamage = 25.0f;
			explodeTime = 0.5f;

			crateExplosion = FMODUnity.RuntimeManager.CreateInstance("event:/Crate/explode");
			crateExplosion.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

			FMODUnity.RuntimeManager.AttachInstanceToGameObject(crateExplosion, GetComponent<Transform>(), GetComponent<Rigidbody>());
		}

		//applies damage to the player
		public override void TakeDamage(GameObject attacker, int damage, bool appliesKnockback = true)
		{
			if (!isDying)
			{
				if (attacker.GetComponent<ExplodingCrate>() != null)
				{
					DestroyCrate();
				}
				else if (canTakeDamage)
				{
					currentVitality = currentVitality - damage;
					if (currentVitality <= 0f)
					{
						Die();
					}
				}
			}
		}

		public override void Kill()
		{
			DestroyCrate();
		}

		protected override void InitializeDeath()
		{
			//start explode anim
			anim.Play("PRIME");
		}

		public override void FinalizeDeath()
		{
			//explode
			boxMove.enabled = false;
			Destroy(GetComponent<CompositeCollider2D>());
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;
			rb.gravityScale = 0.0f;
			StartCoroutine(Attack());
			crateExplosion.start();
		}

		public override void DestroyCrate()
		{
			StartCoroutine(Attack());
		}

		protected IEnumerator Attack()
		{
			isDying = true;
			anim.Play("EXPLODE");
			float attackStart = Time.time;
			List<GameObject> targetsHit = new List<GameObject>();
			while (Time.time - attackStart < explodeTime)
			{
				if (canHitAttack)
				{
					ApplyDamage(explodingTrigger.currentObjects, explodeDamage, ref targetsHit);
				}
				yield return new WaitForFixedUpdate();
			}
			Destroy(gameObject);
		}

		//applies damage to each character in the attack range
		protected void ApplyDamage(List<GameObject> targets, float damage, ref List<GameObject> targetsHit)
		{
			for (int i = targets.Count - 1; i >= 0; i--)
			{
				if (!targetsHit.Contains(targets[i]))
				{
					targetsHit.Add(targets[i]);
					targets[i].GetComponent<Attackable>().TakeDamage(gameObject, Mathf.RoundToInt(damage));
				}
			}
		}
	}
}
