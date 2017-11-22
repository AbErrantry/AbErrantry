using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class CharacterMovement : MonoBehaviour
    {
        Collider2D groundTrigger;
        Collider2D uncrouchTrigger;

        private Animator anim; //the animator component of the player character
        private Rigidbody2D rb; //rigidbocdy component of the player character

        private bool isJumping; //whether the player is jumping or not
        private bool isCrouching; //whether the player is crouching or not
        private bool isRunning; //whether the player is running or not
        [SerializeField] public bool isGrounded; //whether the player is on the ground or not
        [SerializeField] public bool canUncrouch; //
        private bool isMoving; //whether the player is moving or not
        private bool isFacingRight;

        private float mvmtSpeed; //horizontal movement speed

        private float tJump; //time since the last jump
        private bool isInitJump; //initial frame of a jump (to apply jump force)

        private float crouchDelay; //the amount of time before the player can uncrouch
        private float slideDelay; //the duration of the slide animation
        private float jumpDelay; //the amount of time before the player can jump again

        private float jumpForce; //the upwards force of a jump

        private float tCrouch; //time since the last crouch
        private bool isInitCrouch; //initial frame of a crouch (to apply crouch logic)

        private float maxSpeed; //the maximum speed of the player

        private float crouchMovementMultiplier; //how much crouching decreases the movement speed
        private float jumpMovementMultiplier; //how much jumping decreases the movement speed
        private float runMovementMultiplier; //how much running increases the movement speed

        //used for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            isJumping = false;
            isCrouching = false;
            isGrounded = false;
            isInitJump = true;
            isInitCrouch = true;
            canUncrouch = true;

            maxSpeed = 3.0f;

            crouchMovementMultiplier = 0.50f;
            jumpMovementMultiplier = 0.80f;
            runMovementMultiplier = 2.0f;

            crouchDelay = 0.5f;
            slideDelay = 0.25f;
            jumpDelay = 0.5f;

            jumpForce = 500.0f;
    }

        //called once per frame (for input)
        private void Update()
        {
            if(Time.timeScale == 1)
            {
                HandlePlayerInput();
                SendToAnimator();
            }
        }

        //called once per game tick (for game physics)
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
                isJumping = CrossPlatformInputManager.GetButtonDown("Jump"); //spacebar
                //if jumping, set the timer
                if(isJumping)
                {
                    tJump = Time.time;
                }
            }
            //check if the player can run (or stop running)
            if (!isJumping && !isCrouching && isGrounded)
            {
                isRunning = CrossPlatformInputManager.GetButton("Fire3"); //left shift
            }
            //check if the player can crouch (or stop crouching)
            if (!isJumping && isGrounded)
            {
                //the player can only crouch when they are not crouching
                //the player can only uncrouch when they have room to stand up
                if(!isCrouching || CheckIfCanUncrouch())
                {
                    isCrouching = CrossPlatformInputManager.GetButton("Fire1"); //ctrl
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
            mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //A and D
            isMoving = (mvmtSpeed != 0) ? true : false; //sets whether or not the player is moving on this frame

            if(mvmtSpeed < 0 && isFacingRight)
            {
                isFacingRight = false;
            }
            else if(mvmtSpeed >0 && !isFacingRight)
            {
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
            if(isJumping)
            {
                mvmtSpeed *= jumpMovementMultiplier; //reduce max speed a bit
                //if this is the first jump frame, add the jump force
                if (isInitJump)
                {
                    rb.AddForce(new Vector2(0f, jumpForce));
                    isInitJump = false;
                } 
            }
            if(isCrouching)
            {
                mvmtSpeed *= crouchMovementMultiplier; //reduce max speed by a lot
            }
            if(isRunning)
            {
                if(!isCrouching || Time.time - tCrouch < slideDelay)
                {
                    //player runs or slides into a crawl
                    mvmtSpeed *= runMovementMultiplier; //increase max speed by a lot
                }   
            }
            //finally, move the player
            rb.velocity = new Vector2(mvmtSpeed * maxSpeed, rb.velocity.y);
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

        //these listeners below are for later ;)
        //private void OnCollisionEnter2D(Collision2D collision){}
        //private void OnCollisionStay2D(Collision2D collision){}
        //private void OnCollisionExit2D(Collision2D collision){}
        //private void OnTriggerEnter2D(Collider2D collision){}
        //private void OnTriggerStay2D(Collider2D collision){}
        //private void OnTriggerExit2D(Collider2D collision){}
    }
}
