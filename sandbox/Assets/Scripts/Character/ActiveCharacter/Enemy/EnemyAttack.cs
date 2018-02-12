using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class EnemyAttack : CharacterAttack
    {
        public bool canAttack;

        // Use this for initialization
        protected new void Start()
        {
            base.Start();
            //TODO: set from file
            SetAttack(0, 5.0f, 0.20f); // Stab attack setup
            SetAttack(1, 7.0f, 0.30f); // Swing attack setup
            SetAttack(2, 15.0f, 0.80f); // Power attack setup
        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }

        protected new void FixedUpdate()
        {
            base.FixedUpdate();
            if (canAttack && !isAttacking && !isWindingUp)
            {
                ChooseRandomAttack();
            }
        }

        public void ChooseRandomAttack()
        {
            int attackIndex = 0;
            float randomValue = Random.Range(0.0f, 100.0f);
            if (randomValue < 50.0f)
            {
                attackIndex = 0;
            }
            else if (randomValue < 80.0f)
            {
                attackIndex = 1;
            }
            else
            {
                attackIndex = 2;
            }
            StartCoroutine(WindUpDelay(attackIndex));
        }

        private IEnumerator WindUpDelay(int index)
        {
            isWindingUp = true;
            SendToAnimator();
            yield return new WaitForSeconds(attackDurations[index] * 2.0f);
            StartCoroutine(Attack(index));
        }
    }
}
