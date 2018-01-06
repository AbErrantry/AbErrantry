using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        //character components
        private Animator anim; //the animator component of the character character
        private Rigidbody2D rb; //rigidbocdy component of the character character

        public bool attackInputDown;
        public bool attackInputUp;

        public bool canHitStab;
        public bool canHitSwing;
        public bool canHitPower;

        public StabbingTrigger stabbingTrigger;
        public SwingingTrigger swingingTrigger;
        public PowerTrigger powerTrigger;

        public float stabAttackDuration;
        public float swingAttackDuration;
        public float powerAttackDuration;

        public float stabAttackStrength;
        public float swingAttackStrength;
        public float powerAttackStrength;

        public bool isWindingUp;
        public bool isAttacking;

        public float attackStart;

        public bool isStabAttacking;
        public bool isSwingAttacking;
        public bool isPowerAttacking;

        private bool isInitAttack;

        private float attackPress;
        private float attackRelease;

        private float swingThreshold;
        private float powerThreshold;

        //used for initialization
        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            attackInputDown = false;
            attackInputUp = true;

            isWindingUp = false;
            isAttacking = false;

            isInitAttack = false;

            attackPress = 0.0f;
            attackRelease = 0.0f;

            swingThreshold = 0.15f;
            powerThreshold = 0.60f;

            stabAttackDuration = 0.20f;
            swingAttackDuration = 0.40f;
            powerAttackDuration = 1.0f;

            stabAttackStrength = 5.0f;
            swingAttackStrength = 7.0f;
            powerAttackStrength = 15.0f;
        }

        //
        private void Update()
        {
            if (!isAttacking)
            {
                if (attackInputDown && !isWindingUp)
                {
                    attackPress = Time.time;
                    isWindingUp = true;
                }
                else if (attackInputUp && isWindingUp)
                {
                    attackRelease = Time.time;
                    isWindingUp = false;
                    isInitAttack = true;
                }
            }
            SendToAnimator();
        }

        //
        private void FixedUpdate()
        {
            if(isInitAttack)
            {
                isAttacking = true;
                isInitAttack = false;
                float attackDuration = attackRelease - attackPress;
                attackStart = Time.time;
                if (attackDuration > swingThreshold)
                {
                    isSwingAttacking = true;
                    StartCoroutine(Attack(canHitSwing, swingAttackDuration, attackStart, swingingTrigger.currentObjects, swingAttackStrength));
                }
                else
                {
                    isStabAttacking = true;
                    StartCoroutine(Attack(canHitStab, stabAttackDuration, attackStart, stabbingTrigger.currentObjects, stabAttackStrength));
                }
            }
            else if(isWindingUp)
            {
                if(Time.time - attackPress > powerThreshold)
                {
                    attackStart = Time.time;
                    isWindingUp = false;
                    isAttacking = true;
                    isPowerAttacking = true;
                    StartCoroutine(Attack(canHitPower, powerAttackDuration, attackStart, powerTrigger.currentObjects, powerAttackStrength));
                }
            }
        }

        //sends boolean values describing character state to the animator
        private void SendToAnimator()
        {
            anim.SetBool("isWindingUp", isWindingUp);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isStabAttacking", isStabAttacking);
            anim.SetBool("isSwingAttacking", isSwingAttacking);
            anim.SetBool("isPowerAttacking", isPowerAttacking);
        }

        //invokes the stab attack
        private IEnumerator Attack(bool canHitAttack, float attackDuration, float attackStart, List<GameObject> currentObjects, float damage)
        {
            bool hasAttacked = false;
            while(Time.time - attackStart < attackDuration)
            {
                if (!hasAttacked)
                {
                    hasAttacked = true;
                    if (canHitStab)
                    {
                        ApplyDamage(currentObjects, damage);
                    }
                    else
                    {
                        //whiff
                    }
                }
                yield return null;
            }
            FinishedAttacking();
        }

        private void FinishedAttacking()
        {
            isAttacking = false;
            isStabAttacking = false;
            isSwingAttacking = false;
            isPowerAttacking = false;
        }

        //applies damage to each character in the attack range
        public void ApplyDamage(List<GameObject> enemies, float damage)
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<CharacterBehavior>().TakeDamage(damage);
            }
        }
    }
}
