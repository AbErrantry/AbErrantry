using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class PlayerAttack : CharacterAttack
    {
        public Animator weaponAnim;
        private PlayerInput playerInput;
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
            playerMovement = GetComponent<PlayerMovement>();

            attackInputDown = false;
            attackInputUp = true;
            isInitAttack = false;
            attackPress = 0.0f;
            attackRelease = 0.0f;
            attack2Threshold = 0.15f;
            attack3Threshold = 0.60f;

            SetAttack(0, 5.0f, 0.20f); // Stab attack setup
            SetAttack(1, 7.0f, 0.30f); // Swing attack setup
            SetAttack(2, 15.0f, 0.80f); // Power attack setup
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
                }
                else
                {
                    StartCoroutine(Attack(0));
                }
            }
            else if (isWindingUp && !playerMovement.isFalling)
            {
                if (Time.time - attackPress > attack3Threshold)
                {
                    StartCoroutine(Attack(2));
                }
            }
        }

        protected override void InitializeAttack(int index)
        {
            playerInput.DisableInput(false);
            rb.velocity = new Vector2(0.0f, 0.0f);
        }

        protected override void FinalizeAttack()
        {
            playerInput.EnableInput();
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
