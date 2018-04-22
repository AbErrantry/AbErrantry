using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Enemy : Attackable
    {

        [System.Serializable]
        public class BeaconControl
        {
            public GameObject currTarget;
            public GameObject[] beacons;
            public int beaconNum;
        }
        private float maxSpeed;
        private EnemyMovement enemyMovement;
        public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;
        [Header("Beacon System")]
        public BeaconControl beacCon;
        public bool chasingPlayer;

        [Range(0.0f, 100.0f)]
        [Tooltip("How often should the AI stop and look around at each beacon, 0% - 100% (0% never, 100% max).")]
        public float stoppingPercentage; //How often should the AI stop and look around

        [Header("AI Scanning Masks")]
        [Space(5)]
        [Tooltip("Select the Players layer.")]
        public LayerMask playerMask;

        [Tooltip("Select the default layer or the layer the world is going to be in.")]
        public LayerMask defaultMask;

        [Tooltip("Select the Layer that has all the objects that the player can hide behind.")]
        public LayerMask hideablesMask;

        private LayerMask layers;

        private Vector2 boxCastDimensions;

        private int boxCastDirection;
        private int boxCastDistance;

        public bool playerInRange;

        public bool isDormant;

        private FMOD.Studio.EventInstance damageNoise;
        private FMOD.Studio.EventInstance deathNoise;

        void OnBecameVisible()
        {
            enabled = true;
        }

        void OnBecameInvisible()
        {
            enabled = false;
        }

        //used for initialization
        protected new void Start()
        {
            base.Start();

            isDormant = false;

            boxCastDimensions = new Vector2(1, 0.5f);
            boxCastDistance = 5;

            layers = playerMask | defaultMask | hideablesMask;
            enemyMovement = GetComponent<EnemyMovement>();
            canFlinch = false;
            canKnockBack = true;
            canTakeDamage = true;
            chasingPlayer = false;
            beacCon.beaconNum = 0;
            beacCon.currTarget = beacCon.beacons[beacCon.beaconNum];

            playerInRange = false;

            enabled = false;

            damageNoise = FMODUnity.RuntimeManager.CreateInstance("event:/" + character.fields.type + "/take_damage");
            deathNoise = FMODUnity.RuntimeManager.CreateInstance("event:/" + character.fields.type + "/death");

            damageNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
            deathNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        }

        public override void TakeDamage(GameObject attacker, int damage, bool appliesKnockback = true)
        {
            base.TakeDamage(attacker, damage, appliesKnockback);
            if (canTakeDamage)
            {
                damageNoise.start();
                if (attacker.GetComponent<Player>() != null && !chasingPlayer)
                {
                    SetPlayerTarget(attacker);
                }
            }
        }

        public void SetPlayerTarget(GameObject player)
        {
            enemyMovement.StopCoroutines();
            StopAllCoroutines();
            chasingPlayer = true;
            beacCon.currTarget = player;
            ParticleManager.instance.SpawnParticle(gameObject, "spot");
        }

        protected void FixedUpdate()
        {
            if (!isDying && !isDormant)
            {
                RaycastHit2D ray = Physics2D.BoxCast(this.transform.position, boxCastDimensions, 0.0f, transform.right, boxCastDistance, layers.value);

                DrawBox(transform.position, new Vector2(boxCastDimensions.x * boxCastDistance, boxCastDimensions.y));

                if (ray && ray.collider.gameObject.GetComponent<Player>() != null && !chasingPlayer)
                {
                    enemyMovement.StopCoroutines();
                    StopAllCoroutines();
                    chasingPlayer = true;
                    beacCon.currTarget = ray.collider.gameObject;
                    ParticleManager.instance.SpawnParticle(gameObject, "spot");
                }
                else if (!playerInRange && chasingPlayer)
                {
                    chasingPlayer = false;
                    StartCoroutine(enemyMovement.StopAndScan());
                    SwitchBeacon();
                }

                MoveTowardsTarget();
            }
        }

        private void MoveTowardsTarget()
        {
            if (beacCon.currTarget != null)
            {
                enemyMovement.MoveTowards(beacCon.currTarget.transform);
                if (Mathf.Abs(beacCon.currTarget.transform.position.x - this.gameObject.transform.position.x) > 5.0f && chasingPlayer)
                {
                    enemyMovement.runInput = true;
                }
                else
                {
                    enemyMovement.runInput = false;
                }
            }
        }

        //TODO: remove debug function.
        private void DrawBox(Vector2 position, Vector2 size)
        {
            boxCastDirection = enemyMovement.isFacingRight ? 1 : -1;

            Vector2 origin = position;
            Vector2 upperBound = position + new Vector2(0.0f, size.y);
            Vector2 outerUpperBound = position + new Vector2(size.x * boxCastDirection, size.y);
            Vector2 outerLowerBound = position + new Vector2(size.x * boxCastDirection, 0.0f);
            Debug.DrawLine(origin, outerLowerBound, Color.red, Time.fixedDeltaTime);
            Debug.DrawLine(origin, upperBound, Color.red, Time.fixedDeltaTime);
            Debug.DrawLine(upperBound, outerUpperBound, Color.red, Time.fixedDeltaTime);
            Debug.DrawLine(outerLowerBound, outerUpperBound, Color.red, Time.fixedDeltaTime);
        }

        protected override void InitializeDeath()
        {
            //take away enemy input
            //enemy no longer targets player
            //enemy no longer attackable
            isDying = true;
            anim.SetBool("isDying", isDying); //death animation
            deathNoise.start();
        }

        public override void FinalizeDeath()
        {
            //drop loot
            if (GetComponent<DormantCharacter>() != null)
            {
                EventDisplay.instance.AddEvent("Defeated " + GetComponent<DormantCharacter>().name + ".");
            }
            else
            {
                EventDisplay.instance.AddEvent("Defeated " + character.fields.type + ".");
            }
            Player.instance.SetGold(UnityEngine.Random.Range(1, 6) * character.fields.vitality);
            Destroy(gameObject);
        }

        //TODO: change to using fixed update to constantly monitor towards target (for edge cases)
        //TODO: move to a behavior AI class that will take care of figuring out where the enemy needs to go
        public void SwitchBeacon()
        {
            beacCon.beaconNum++;
            beacCon.currTarget = beacCon.beacons[beacCon.beaconNum % beacCon.beacons.Length];
            ShouldScan();
        }

        private void ShouldScan()
        {
            if (UnityEngine.Random.Range(1.0f, 100.0f) <= stoppingPercentage && enemyMovement.isScanning == false)
            {
                enemyMovement.isScanning = true;
                StartCoroutine(enemyMovement.StopAndScan());
            }
        }
    }
}
