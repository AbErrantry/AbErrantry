using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class EnemyAttack : CharacterAttack
    {
        public bool canAttack;
        private Enemy enemy;

        private FMOD.Studio.EventInstance attackNoise;

        private float timeSinceLast;

        // Use this for initialization
        protected new void Start()
        {
            enemy = GetComponent<Enemy>();
            base.Start();

            attackNoise = FMODUnity.RuntimeManager.CreateInstance("event:/" + character.fields.type + "/attack");
            attackNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            timeSinceLast = Time.time;
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
            if (Time.time - timeSinceLast > 2.0f)
            {
                timeSinceLast = Time.time;
                anim.Play("POWER");
                attackNoise.start();
            }
        }

        protected override void InitializeAttack(int index)
        {
            attackNoise.start();
        }
    }
}
