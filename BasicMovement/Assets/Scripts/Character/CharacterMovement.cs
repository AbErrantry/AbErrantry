using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class CharacterMovement : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D rb;

        [SerializeField] private bool isJumping;
        [SerializeField] private bool isCrouching;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isMoving;

        private float mvmtSpeed; //horizontal movement speed

        private float tJump; //time since the last jump
        private bool isInitJump; //initial frame of a jump (to apply jump force)

        private float maxSpeed = 3.0f;

        private float crouchMovementMultiplier = 0.50f;
        private float jumpMovementMultiplier = 0.80f;
        private float runMovementMultiplier = 2.0f;

        //for checking if the player is grounded (creates collision box at base)
        public Transform top_left;
        public Transform bottom_right;
        public LayerMask ground_layers;

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            isJumping = false;
            isCrouching = false;
            isGrounded = true;
            isInitJump = true;
        }

        //called once per frame (for input)
        private void Update()
        {
            HandlePlayerInput();
            SendToAnimator();
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
            if (!isJumping && !isCrouching && isGrounded)
            {
                isJumping = CrossPlatformInputManager.GetButtonDown("Jump"); //spacebar
                //if jumping, set the timer
                if(isJumping)
                {
                    tJump = Time.time;
                }
            }
            if (!isJumping && !isCrouching && isGrounded)
            {
                isRunning = CrossPlatformInputManager.GetButton("Fire3"); //left shift
            }
            if (!isJumping && isGrounded)
            {
                isCrouching = CrossPlatformInputManager.GetButton("Fire1"); //ctrl
            }
            mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //A and D
            isMoving = (mvmtSpeed != 0) ? true : false; //sets whether or not the player is moving on this frame
        }

        //sends boolean values describing player state to the animator
        private void SendToAnimator()
        {
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isCrouching", isCrouching);
            anim.SetBool("isMoving", isMoving);
        }

        //moves the player
        private void Move()
        {
            if(isJumping)
            {
                //player jumps
                //reduce max speed a bit
                mvmtSpeed *= jumpMovementMultiplier;
                if(isInitJump)
                {
                    rb.AddForce(new Vector2(0f, 400f));
                    isInitJump = false;
                } 
            }
            if(isCrouching)
            {
                //player crouches
                //reduce max speed a lot
                mvmtSpeed *= crouchMovementMultiplier;
            }
            if(isRunning)
            {
                //player runs
                //increase max speed a lot
                mvmtSpeed *= runMovementMultiplier;
            }
            //finally, move the player
            rb.velocity = new Vector2(mvmtSpeed * maxSpeed, rb.velocity.y);
        }

        //checks if the player is in contact with the ground
        private void CheckIfGrounded()
        {
            isGrounded = Physics2D.OverlapArea(top_left.position, bottom_right.position, ground_layers);
            if (isGrounded && isJumping && Time.time - tJump > 0.5f)
            {
                isJumping = false;
                isInitJump = true;
            }
        }

        //these listeners below are for later
        //private void OnCollisionEnter2D(Collision2D collision){}
        //private void OnCollisionStay2D(Collision2D collision){}
        //private void OnCollisionExit2D(Collision2D collision){}
        //private void OnTriggerEnter2D(Collider2D collision){}
        //private void OnTriggerStay2D(Collider2D collision){}
        //private void OnTriggerExit2D(Collider2D collision){}
    }
}
