using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        //character components
        protected Animator anim; //the animator component of the character character
        public AttackTrigger attackTrigger;
        protected Rigidbody2D rb;

        public bool canHitAttack;

        protected float[] attackDurations;
        protected float[] attackStrengths;
        protected bool[] attackFlags;

        protected float attackStart;
        public bool isWindingUp;
        public bool isAttacking;

        //used for initialization
        protected void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            attackDurations = new float[3];
            attackStrengths = new float[3];
            attackFlags = new bool[3];
            canHitAttack = false;
            attackStart = 0.0f;
            isWindingUp = false;
            isAttacking = false;
        }

        protected void Update()
        { }

        protected void FixedUpdate()
        { }

        public void SetAttack(int index, float strength, float duration)
        {
            attackDurations[index] = duration;
            attackStrengths[index] = strength;
        }

        //sends boolean values describing character state to the animator
        protected virtual void SendToAnimator()
        {
            anim.SetBool("isWindingUp", isWindingUp);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isAttack1", attackFlags[0]);
            anim.SetBool("isAttack2", attackFlags[1]);
            anim.SetBool("isAttack3", attackFlags[2]);
        }

        protected IEnumerator Attack(int index)
        {
            InitializeAttack(index);
            attackStart = Time.time;
            isWindingUp = false;
            isAttacking = true;
            attackFlags[index] = true;
            SendToAnimator();
            List<GameObject> targetsHit = new List<GameObject>();
            while (Time.time - attackStart < attackDurations[index])
            {
                if (canHitAttack)
                {
                    ApplyDamage(attackTrigger.currentObjects, attackStrengths[index], ref targetsHit);
                }
                yield return new WaitForFixedUpdate();
            }
            FinishedAttacking();
        }

        protected void FinishedAttacking()
        {
            isAttacking = false;
            attackFlags[0] = false;
            attackFlags[1] = false;
            attackFlags[2] = false;
            SendToAnimator();
            FinalizeAttack();
        }

        //applies damage to each character in the attack range
        protected void ApplyDamage(List<GameObject> targets, float damage, ref List<GameObject> targetsHit)
        {
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (!targetsHit.Contains(targets[i]))
                {
                    targetsHit.Add(targets[i]);
                    //TODO: ensure that only active characters are in the triggers
                    targets[i].GetComponent<Attackable>().TakeDamage(gameObject, Mathf.RoundToInt(damage));
                }
            }
        }

        protected virtual void InitializeAttack(int index)
        {
            //put things that happen before an attack here
            rb.velocity = new Vector2(0.0f, 0.0f);
        }

        protected virtual void FinalizeAttack()
        {
            //put things that happen after an attack here
        }
    }
}
