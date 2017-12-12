using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        public bool attackInputDown;
        public bool attackInputUp;

        public bool canHitStab;
        public bool canHitSwing;
        public bool canHitPower;

        public StabbingTrigger stabbingTrigger;
        public SwingingTrigger swingingTrigger;
        public PowerTrigger powerTrigger;

        public bool isWindingUp;
        public bool isAttacking;

        private bool isInitAttack;

        private float attackPress;
        private float attackRelease;

        private float swingThreshold;
        private float powerThreshold;

        //used for initialization
        private void Start()
        {
            attackInputDown = false;
            attackInputUp = true;

            isWindingUp = false;
            isAttacking = false;

            isInitAttack = false;

            attackPress = 0.0f;
            attackRelease = 0.0f;

            swingThreshold = 0.25f;
            powerThreshold = 1.00f;
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
        }

        //
        private void FixedUpdate()
        {
            if(isInitAttack)
            {
                isAttacking = true;
                isInitAttack = false;
                if (attackRelease - attackPress > swingThreshold)
                {
                    SwingAttack();
                    Debug.Log("swing");
                }
                else
                {
                    StabAttack();
                    Debug.Log("stab");
                }
                isAttacking = false;
            }
            else if(isWindingUp)
            {
                if(Time.time - attackPress > powerThreshold)
                {
                    isWindingUp = false;
                    isAttacking = true;

                    PowerAttack();
                    Debug.Log("power");

                    isAttacking = false;
                }
            }
        }

        //invokes the stab attack
        public void StabAttack()
        {
            if(canHitStab)
            {
                ApplyDamage(stabbingTrigger.currentObjects, 5.0f);
            }
            else
            {
                //whiff
            }
        }

        //invokes the swing attack
        public void SwingAttack()
        {
            if(canHitSwing)
            {
                ApplyDamage(swingingTrigger.currentObjects, 7.0f);
            }
            else
            {
                //whiff
            }
        }

        //invokes the power attack
        public void PowerAttack()
        {
            if(canHitPower)
            {
                ApplyDamage(powerTrigger.currentObjects, 20.0f);
            }
            else
            {
                //whiff
            }
        }

        //applies damage to each character in the attack range
        public void ApplyDamage(List<GameObject> enemies, float damage)
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<Character>().TakeDamage(damage);
            }
        }
    }
}
