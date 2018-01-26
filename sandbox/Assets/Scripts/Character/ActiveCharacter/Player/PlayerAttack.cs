using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class PlayerAttack : ActiveCharacterAttack
    {
        public Animator weaponAnim;

        public PlayerInput playerInput;

        protected PlayerMovement playerMovement;
        public bool canAttack;
        
        public bool attackInputDown;
        public bool attackInputUp;

        protected bool isInitAttack;

        protected float attackPress;
        protected float attackRelease;

        //used for initialization
        protected new void Start()
        {
            base.Start();

            attackInputDown = false;
            attackInputUp = true;

            isInitAttack = false;

            attackPress = 0.0f;
            attackRelease = 0.0f;

            playerMovement = GetComponent<PlayerMovement>();
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
                else if(playerMovement.isFalling)
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

            if(isInitAttack)
            {
                isInitAttack = false;
                float attackDuration = attackRelease - attackPress;
                if (attackDuration > swingThreshold)
                {
                    StartCoroutine(SwingAttack());
                }
                else
                {
                    StartCoroutine(StabAttack());
                }
            }
            else if(isWindingUp && !playerMovement.isFalling)
            {
                if(Time.time - attackPress > powerThreshold)
                {
                    StartCoroutine(PowerAttack());
                }
            }
        }

        protected override void InitializeAttack()
        {
            playerInput.DisableInput(false);
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
            anim.SetBool("isStabAttacking", isStabAttacking);
            anim.SetBool("isSwingAttacking", isSwingAttacking);
            anim.SetBool("isPowerAttacking", isPowerAttacking);

            weaponAnim.SetBool("isWindingUp", isWindingUp);
            weaponAnim.SetBool("isAttacking", isAttacking);
            weaponAnim.SetBool("isStabAttacking", isStabAttacking);
            weaponAnim.SetBool("isSwingAttacking", isSwingAttacking);
            weaponAnim.SetBool("isPowerAttacking", isPowerAttacking);
        }
    }
}
