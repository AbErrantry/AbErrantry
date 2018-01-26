using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class ActiveCharacterAttack : MonoBehaviour
    {
        //character components
        protected Animator anim; //the animator component of the character character

        public bool canHitStab;
        public bool canHitSwing;
        public bool canHitPower;

        public StabbingTrigger stabbingTrigger;
        public SwingingTrigger swingingTrigger;
        public PowerTrigger powerTrigger;

        protected float stabAttackDuration;
        protected float swingAttackDuration;
        protected float powerAttackDuration;

        protected float stabAttackStrength;
        protected float swingAttackStrength;
        protected float powerAttackStrength;

        protected float attackStart;

        public bool isWindingUp;
        public bool isAttacking;
        protected bool isStabAttacking;
        protected bool isSwingAttacking;
        protected bool isPowerAttacking;

        protected float swingThreshold;
        protected float powerThreshold;

        //used for initialization
        protected void Start()
        {
            anim = GetComponent<Animator>();

            canHitStab = false;
            canHitSwing = false;
            canHitPower = false;

            stabAttackDuration = 0.20f;
            swingAttackDuration = 0.30f;
            powerAttackDuration = 0.80f;

            stabAttackStrength = 5.0f;
            swingAttackStrength = 7.0f;
            powerAttackStrength = 15.0f;

            attackStart = 0.0f;

            isWindingUp = false;
            isAttacking = false;
            isStabAttacking = false;
            isSwingAttacking = false;
            isPowerAttacking = false;

            swingThreshold = 0.15f;
            powerThreshold = 0.60f;
        }

        protected void Update()
        {

        }

        protected void FixedUpdate()
        {

        }

        //sends boolean values describing character state to the animator
        protected virtual void SendToAnimator()
        {
            anim.SetBool("isWindingUp", isWindingUp);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isStabAttacking", isStabAttacking);
            anim.SetBool("isSwingAttacking", isSwingAttacking);
            anim.SetBool("isPowerAttacking", isPowerAttacking);
        }

        protected IEnumerator StabAttack()
        {
            InitializeAttack();

            attackStart = Time.time;
            isAttacking = true;
            isStabAttacking = true;
            SendToAnimator();

            List<GameObject> targetsHit = new List<GameObject>();
            while(Time.time - attackStart < stabAttackDuration / 3f)
            {
                yield return new WaitForFixedUpdate();
            }
            while(Time.time - attackStart < stabAttackDuration)
            {
                if (canHitStab)
                {
                    ApplyDamage(stabbingTrigger.currentObjects, stabAttackStrength, ref targetsHit);
                }
                yield return new WaitForFixedUpdate();
            }

            FinishedAttacking();
        }

        protected IEnumerator SwingAttack()
        {
            InitializeAttack();

            attackStart = Time.time;
            isAttacking = true;
            isSwingAttacking = true;
            SendToAnimator();

            List<GameObject> targetsHit = new List<GameObject>();
            while(Time.time - attackStart < swingAttackDuration / 3f)
            {
                yield return new WaitForFixedUpdate();
            }
            while(Time.time - attackStart < swingAttackDuration)
            {
                if (canHitSwing)
                {
                    ApplyDamage(swingingTrigger.currentObjects, swingAttackStrength, ref targetsHit);
                }
                yield return new WaitForFixedUpdate();
            }

            FinishedAttacking();
        }

        protected IEnumerator PowerAttack()
        {
            InitializeAttack();

            attackStart = Time.time;
            isWindingUp = false;
            isAttacking = true;
            isPowerAttacking = true;
            SendToAnimator();

            List<GameObject> targetsHit = new List<GameObject>();
            while(Time.time - attackStart < powerAttackDuration / 2f)
            {
                yield return new WaitForFixedUpdate();
            }
            while(Time.time - attackStart < powerAttackDuration)
            {
                if (canHitPower)
                {
                    ApplyDamage(powerTrigger.currentObjects, powerAttackStrength, ref targetsHit);
                }
                yield return new WaitForFixedUpdate();
            }
            
            FinishedAttacking();
        }

        protected void FinishedAttacking()
        {
            isAttacking = false;
            isStabAttacking = false;
            isSwingAttacking = false;
            isPowerAttacking = false;
			SendToAnimator();
            FinalizeAttack();
        }

        //applies damage to each character in the attack range
        protected void ApplyDamage(List<GameObject> targets, float damage, ref List<GameObject> targetsHit)
        {
            for(int i = targets.Count - 1; i >= 0; i--)
            {
                if(!targetsHit.Contains(targets[i]))
                {
                    targetsHit.Add(targets[i]);

                    //TODO: ensure that only active characters are in the triggers
                    targets[i].GetComponent<ActiveCharacter>().TakeDamage(gameObject, damage);
                }
            }
        }

        protected virtual void InitializeAttack()
        {
            //put things that happen before an attack here
        }

        protected virtual void FinalizeAttack()
        {
            //put things that happen after an attack here
        }
    }
}
