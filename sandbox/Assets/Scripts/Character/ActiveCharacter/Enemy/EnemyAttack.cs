using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class EnemyAttack : CharacterAttack
    {
        public bool canAttack;
        [SerializeField] public List<Attack> attackList;

        // Use this for initialization
        protected new void Start()
        {
            base.Start();
            //TODO: set from file
            for (int i = 0; i < attackList.Count; i++)
            {
                SetAttack(i, attackList[i].power, attackList[i].duration);
            }
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
            if (randomValue > 80.0f && attackList.Count >= 3)
            {
                attackIndex = 2;
            }
            else if (randomValue > 50.0f && attackList.Count >= 2)
            {
                attackIndex = 1;
            }
            else
            {
                if (attackList.Count >= 1)
                {
                    attackIndex = 0;
                }
                else
                {
                    Debug.LogError("Enemy " + gameObject.name + " does not have any registered attacks.");
                }
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

        public void PlayAttack()
        {
            anim.Play("POWER");
        }
    }
}
