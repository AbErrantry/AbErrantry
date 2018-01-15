using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public abstract class ActiveCharacter : Character 
	{
		protected Rigidbody2D rb;
		protected SpriteRenderer sr;


		//used for initialization
		protected new void Start () 
		{
			base.Start();
			rb = GetComponent<Rigidbody2D>();
			sr = GetComponent<SpriteRenderer>();
		}

		//applies damage to the player
        public void TakeDamage(GameObject attacker, float damage)
        {
            vitality = vitality - damage;
			KnockBack(attacker.transform.position, damage);
			StartCoroutine(FlinchColor());
            if (vitality <= 0f)
            {
                Die();
            }
        }

		protected void KnockBack(Vector3 attackerLocation, float intensity)
		{
			Vector3 force;
			if(attackerLocation.x < transform.position.x)
			{
				force = new Vector3(intensity*100f, 50f, 0.0f);
			}
			else
			{
				force = new Vector3(-(intensity*100f), 50f, 0.0f);
			}
			rb.AddForce(force);
		}

		//no longer works due to HSV material :(
		protected IEnumerator FlinchColor()
		{
			float redTime = Time.time;
			while(Time.time - redTime < 0.25f)
			{
				sr.color = Color.Lerp(sr.color, Color.red, (Time.time - redTime) / 0.25f);
				yield return new WaitForFixedUpdate();
			}

			float whiteTime = Time.time;
			while(Time.time - whiteTime < 0.25f)
			{
				sr.color = Color.Lerp(sr.color, Color.white, (Time.time - whiteTime) / 0.25f);
				yield return new WaitForFixedUpdate();
			}

			yield return null;
		}

		protected abstract void Die();
	}
}
