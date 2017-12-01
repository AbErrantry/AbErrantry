using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class CharacterMovement : MonoBehaviour
    {
        Collider2D groundTrigger; //TODO: refactor
        Collider2D uncrouchTrigger; //TODO: refactor

        private Animator anim; //the animator component of the player character
        private Rigidbody2D rb; //rigidbocdy component of the player character

        public bool jumpInput;
        public bool crouchInput;
        public bool runInput;
        public float mvmtSpeed; //horizontal movement speed
        private float frameSpeed;

        private bool isJumping; //whether the player is jumping or not
        private bool isCrouching; //whether the player is crouching or not
        private bool isRunning; //whether the player is running or not
        public bool isGrounded; //whether the player is on the ground or not
        public bool canUncrouch; //
        private bool isMoving; //whether the player is moving or not
        private bool isFacingRight; //

        private float tJump; //time since the last jump
        private bool isInitJump; //initial frame of a jump (to apply jump force)

        private float crouchDelay; //the amount of time before the player can uncrouch
        private float slideDelay; //the duration of the slide animation
        private float jumpDelay; //the amount of time before the player can jump again

        private float jumpForce; //the upwards force of a jump

        private float tCrouch; //time since the last crouch
        private bool isInitCrouch; //initial frame of a crouch (to apply crouch logic)

        private float maxSpeed; //the maximum speed of the player

        private float crouchSpeed; //how much crouching decreases the movement speed
        private float jumpSpeed; //how much jumping decreases the movement speed
        private float runSpeed; //how much running increases the movement speed

        private float multiplier;

        //used for initialization
        void Start()
        {
            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 1;

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            isJumping = false;
            isCrouching = false;
            isGrounded = false;
            isInitJump = true;
            isInitCrouch = true;
            canUncrouch = true;

            isFacingRight = true;

            maxSpeed = 150f;

            crouchSpeed = 0.50f;
            jumpSpeed = 0.80f;
            runSpeed = 2.0f;

            crouchDelay = 0.75f;
            slideDelay = 0.25f;
            jumpDelay = 0.5f;

            jumpForce = 500.0f;

            multiplier = 1;
    }

        //called once per frame (for input)
        private void Update()
        {
            HandlePlayerInput();
            SendToAnimator();
        }

        //called once per game tick (for physics)
        private void FixedUpdate()
        {
            Move();
            CheckIfGrounded();
        }

        //handles player input
        private void HandlePlayerInput()
        {
            //check if the player can jump
            if (!isJumping && !isCrouching && isGrounded)
            {
                isJumping = jumpInput;
                //if jumping, set the timer
                if(isJumping)
                {
                    tJump = Time.time;
                }
            }
            //check if the player can run (or stop running)
            if (!isJumping && !isCrouching && isGrounded)
            {
                isRunning = runInput; //left shift
            }
            //check if the player can crouch (or stop crouching)
            if (!isJumping && isGrounded)
            {
                //the player can only crouch when they are not crouching
                //the player can only uncrouch when they have room to stand up
                if(!isCrouching || CheckIfCanUncrouch())
                {
                    isCrouching = crouchInput; //ctrl
                    //start the crouch timer if this is the first crouch frame
                    if(isCrouching && isInitCrouch)
                    {
                        //started crouching
                        isInitCrouch = false;
                        tCrouch = Time.time;
                    }
                    if(!isCrouching)
                    {
                        //stopped crouching
                        isInitCrouch = true;
                    }
                }
            }
            isMoving = (mvmtSpeed != 0) ? true : false; //sets whether or not the player is moving on this frame

            if(mvmtSpeed < 0 && isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = false;
            }
            else if(mvmtSpeed > 0 && !isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                isFacingRight = true;
            }
        }

        //sends boolean values describing player state to the animator
        private void SendToAnimator()
        {
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isCrouching", isCrouching);
            anim.SetBool("isMoving", isMoving);
            anim.SetBool("isFacingRight", isFacingRight);
        }

        //moves the player
        private void Move()
        {
            if (isJumping)
            {
                if (isRunning)
                {
                    multiplier = jumpSpeed * runSpeed;
                }
                else
                {
                    multiplier = jumpSpeed;
                }

                //if this is the first jump frame, add the jump force
                if (isInitJump)
                {
                    rb.AddForce(new Vector2(0f, jumpForce));
                    isInitJump = false;
                }
            }
            else if (isCrouching)
            {
                if(isRunning && Time.time - tCrouch < slideDelay)
                {
                    multiplier = runSpeed * crouchSpeed;
                }
                else
                {
                    multiplier = crouchSpeed;
                } 
            }
            else if (isRunning)
            {
                multiplier = runSpeed;
            }
            else
            {
                multiplier = 1;
            }
            rb.velocity = new Vector2(mvmtSpeed * multiplier * maxSpeed * Time.deltaTime, rb.velocity.y);
        }

        //checks if the player is in contact with the ground
        private void CheckIfGrounded()
        {
            if (isGrounded && isJumping && Time.time - tJump > jumpDelay)
            {
                isJumping = false;
                isInitJump = true;
            }
        }

        //checks if the player has enough room to uncrouch
        private bool CheckIfCanUncrouch()
        {
            if (!canUncrouch || Time.time - tCrouch < crouchDelay)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
