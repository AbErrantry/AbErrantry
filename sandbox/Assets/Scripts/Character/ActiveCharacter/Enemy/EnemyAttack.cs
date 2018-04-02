using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class EnemyAttack : CharacterAttack
    {
        public bool canAttack;
        private Enemy enemy;

        // Use this for initialization
        protected new void Start()
        {
            enemy = GetComponent<Enemy>();
            base.Start();
        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }

        protected new void FixedUpdate()
        {
            base.FixedUpdate();
            if (canAttack && !isAttacking && !isWindingUp && !enemy.isDormant)
            {
                ChooseRandomAttack();
            }
        }

        public void ChooseRandomAttack()
        {
            CharacterAttackInfo chosenAttack = new CharacterAttackInfo();
            float randomValue = Random.Range(0.0f, 1.0f);
            foreach (CharacterAttackInfo atk in character.fields.attacks)
            {
                if (randomValue >= atk.oddsThreshold)
                {
                    chosenAttack = atk;
                }
            }
            StartCoroutine(WindUpDelay(chosenAttack.id));
        }

        private IEnumerator WindUpDelay(int index)
        {
            isWindingUp = true;
            SendToAnimator();
            yield return new WaitForSeconds(character.fields.attacks[index].windupTime);
            StartCoroutine(Attack(index));
        }

        public void PlayAttack()
        {
            anim.Play("POWER");
        }
    }
}
