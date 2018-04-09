using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class PlayerAttack : CharacterAttack
    {
        public Animator weaponAnim;
        private PlayerInput playerInput;
        private Player player;
        protected PlayerMovement playerMovement;

        public bool canAttack;
        protected float attack2Threshold;
        protected float attack3Threshold;

        public bool attackInputDown;
        public bool attackInputUp;

        protected bool isInitAttack;

        protected float attackPress;
        protected float attackRelease;

        //used for initialization
        protected new void Start()
        {
            base.Start();

            playerInput = GetComponent<PlayerInput>();
            player = GetComponent<Player>();
            playerMovement = GetComponent<PlayerMovement>();

            attackInputDown = false;
            attackInputUp = true;
            isInitAttack = false;
            attackPress = 0.0f;
            attackRelease = 0.0f;
            attack2Threshold = 0.15f;
            attack3Threshold = 0.60f;
        }

        //
        protected new void Update()
        {
            base.Update();
            if (!isAttacking && canAttack)
            {
                if (attackInputDown && !isWindingUp && !playerMovement.isFalling)
                {
                    attackPress = Time.time;
                    isWindingUp = true;
                    SendToAnimator();
                }
                else if (attackInputUp && isWindingUp && !playerMovement.isFalling)
                {
                    attackRelease = Time.time;
                    isWindingUp = false;
                    isInitAttack = true;
                    SendToAnimator();
                }
                else if (playerMovement.isFalling)
                {
                    isWindingUp = false;
                    isInitAttack = false;
                    isAttacking = false;
                    SendToAnimator();
                }
            }
        }

        //
        protected new void FixedUpdate()
        {
            base.FixedUpdate();
            if (isInitAttack)
            {
                isInitAttack = false;
                float attackDuration = attackRelease - attackPress;
                if (attackDuration > attack2Threshold)
                {
                    StartCoroutine(Attack(1));
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/swing");
                }
                else
                {
                    StartCoroutine(Attack(0));
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/stab");
                }
            }
            else if (isWindingUp && !playerMovement.isFalling)
            {
                if (Time.time - attackPress > attack3Threshold)
                {
                    StartCoroutine(Attack(2));
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/power");
                }
            }
        }

        protected override int GetAttackPower(int damage)
        {
            return character.fields.strength * damage * Mathf.RoundToInt(GameData.data.itemData.itemDictionary[player.equippedWeapon].strength);
        }

        protected override void InitializeAttack(int index)
        {
            playerInput.DisableInput(false);
            rb.velocity = new Vector2(0.0f, 0.0f);
            playerMovement.vLast = 0.0f;
        }

        protected override void FinalizeAttack()
        {
            playerInput.EnableInput();
            rb.velocity = new Vector2(0.0f, 0.0f);
            playerMovement.vLast = 0.0f;
        }

        //sends boolean values describing character state to the animator
        protected override void SendToAnimator()
        {
            anim.SetBool("isWindingUp", isWindingUp);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isStabAttacking", attackFlags[0]);
            anim.SetBool("isSwingAttacking", attackFlags[1]);
            anim.SetBool("isPowerAttacking", attackFlags[2]);
            weaponAnim.SetBool("isWindingUp", isWindingUp);
            weaponAnim.SetBool("isAttacking", isAttacking);
            weaponAnim.SetBool("isStabAttacking", attackFlags[0]);
            weaponAnim.SetBool("isSwingAttacking", attackFlags[1]);
            weaponAnim.SetBool("isPowerAttacking", attackFlags[2]);
        }
    }
}
