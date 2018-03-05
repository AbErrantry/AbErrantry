using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public abstract class Attackable : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator anim;

        protected int maxVitality;
        protected int currentVitality;

        public bool isDying;

        public bool canFlinch;
        public bool canKnockBack;
        public bool canTakeDamage;

        //used for initialization
        protected void Start()
        {
            maxVitality = currentVitality = 50; //TODO: remove
            isDying = false;
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        //applies damage to the player
        public virtual void TakeDamage(GameObject attacker, int damage)
        {
            if (!isDying)
            {
                if (canFlinch)
                {
                    Flinch();
                }

                if (canKnockBack)
                {
                    KnockBack(attacker.transform.position, damage);
                }

                if (canTakeDamage)
                {
                    currentVitality = currentVitality - damage;
                    if (currentVitality <= 0f)
                    {
                        Die();
                    }
                }
            }
        }

        public void Kill()
        {
            currentVitality = 0;
            TakeDamage(gameObject, 0);
        }

        protected void KnockBack(Vector3 attackerLocation, float intensity)
        {
            Vector3 force;
            if (attackerLocation.x < transform.position.x)
            {
                force = new Vector3(intensity * 100f, 50f, 0.0f);
            }
            else
            {
                force = new Vector3(-(intensity * 100f), 50f, 0.0f);
            }
            rb.AddForce(force);
        }

        protected void Flinch()
        {
            anim.Play("FLINCH");
        }

        protected void Die()
        {
            InitializeDeath();
        }

        protected abstract void InitializeDeath();

        public abstract void FinalizeDeath();
    }
}
