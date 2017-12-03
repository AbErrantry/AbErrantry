using UnityEngine;

namespace Character2D
{
    public class CharacterMovement : MonoBehaviour
    {
        //character components
        private Animator anim; //the animator component of the character character
        private Rigidbody2D rb; //rigidbocdy component of the character character

        //external input
        public bool jumpInput; //jump input from character
        public bool crouchInput; //crouch input from character
        public bool runInput; //run input from character
        public float mvmtSpeed; //horizontal movement speed input from character

        //external booleans that drive the character states
        public bool isGrounded; //whether the character is on the ground or not
        public bool canUncrouch; //whether the character can uncrouch or not

        //booleans that drive the character states
        private bool isJumping; //whether the character is jumping or not
        private bool isCrouching; //whether the character is crouching or not
        private bool isRunning; //whether the character is running or not
        private bool isMoving; //whether the character is moving or not
        private bool isFacingRight; //whether the character is facing right or not
        private bool isFalling; //whether the character is falling or not

        //values that affect the player's speed
        private float speedMultiplier; //the multiplier to be applied to character movement speed
        private float maxSpeed; //the maximum speed of the character

        //values that affect the character's ability to crouch/uncrouch
        private float tCrouch; //time since the last crouch
        private bool isInitCrouch; //initial frame of a crouch (to apply crouch logic)

        //values that affect the character's ability to jump
        [SerializeField] private bool isInitJump; //initial frame of a jump (to apply jump force)
        private float jumpForce; //the upwards force of a jump
        private float tJump; //time since last jump

        //delays after each action
        private float crouchDelay; //the amount of time before the character can uncrouch
        private float slideDelay; //the duration of the slide animation
        private float jumpDelay; //the amount of time before the character can jump again

        //movement speed in each state
        private float crouchSpeed; //how much crouching decreases the movement speed
        private float jumpSpeed; //how much jumping decreases the movement speed
        private float runSpeed; //how much running increases the movement speed

        //position of the player
        private float lastPosition; //x position of the character on the previous tick

        //used for initialization
        private void Start()
        {
            Application.targetFrameRate = 144;
            QualitySettings.vSyncCount = 1;

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            jumpInput = false;
            crouchInput = false;
            runInput = false;
            mvmtSpeed = 0f;

            isGrounded = true;
            canUncrouch = true;

            isJumping = false;
            isCrouching = false;
            isRunning = false;
            isMoving = false;
            isFacingRight = true;
            isFalling = false;

            speedMultiplier = 1;
            maxSpeed = 150f;

            tCrouch = 0.0f;
            isInitCrouch = true;

            isInitJump = true;
            jumpForce = 500.0f;
            tJump = 0.0f;

            crouchDelay = 0.75f;
            slideDelay = 0.25f;
            jumpDelay = 0.05f;

            crouchSpeed = 0.50f;
            jumpSpeed = 0.80f;
            runSpeed = 2.0f;

            lastPosition = 0.0f;
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
            SetMovementLogic();
        }

        //handles character input
        private void HandlePlayerInput()
        {
            //check if the character can jump
            if (!isJumping && !isCrouching && isGrounded && Time.time - tJump > jumpDelay)
            {
                isJumping = jumpInput;
            }
            //check if the character can run (or stop running)
            if (!isJumping && !isCrouching && isGrounded)
            {
                isRunning = runInput;
            }
            //check if the character can crouch (or stop crouching)
            if (!isJumping && isGrounded)
            {
                //the character can only crouch when they are not crouching
                //the character can only uncrouch when they have room to stand up
                if(!isCrouching || CheckIfCanUncrouch())
                {
                    isCrouching = crouchInput;
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
        }

        //sends boolean values describing character state to the animator
        private void SendToAnimator()
        {
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isCrouching", isCrouching);
            anim.SetBool("isMoving", isMoving);
            anim.SetBool("isFacingRight", isFacingRight);
            anim.SetBool("isFalling", isFalling);
        }

        //moves the character
        private void Move()
        {
            if (isJumping)
            {
                if (isRunning)
                {
                    speedMultiplier = jumpSpeed * runSpeed;
                }
                else
                {
                    speedMultiplier = jumpSpeed;
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
                    speedMultiplier = runSpeed * crouchSpeed;
                }
                else
                {
                    speedMultiplier = crouchSpeed;
                } 
            }
            else if (isRunning)
            {
                speedMultiplier = runSpeed;
            }
            else
            {
                speedMultiplier = 1;
            }
            rb.velocity = new Vector2(mvmtSpeed * speedMultiplier * maxSpeed * Time.deltaTime, rb.velocity.y);
        }

        //sets the logic for values related to character movement
        private void SetMovementLogic()
        {
            //boolean expression that sets whether or not the player has moved this tick
            isMoving = Mathf.Abs(lastPosition - rb.transform.position.x) > 0.001f ? true : false;

            //boolean expression that sets whether or not the character is falling on this tick
            isFalling = rb.velocity.y < 0 ? true : false;

            lastPosition = rb.transform.position.x; //set the last position for the next tick

            //change the player's direction if they are moving in another direction
            if (mvmtSpeed < 0 && isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = false;
            }
            else if (mvmtSpeed > 0 && !isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                isFacingRight = true;
            }
        }

        //resets values related to jumping after the player touches the ground
        public void GroundedReset()
        {
            isGrounded = true;
            isInitJump = true;
            isJumping = false;
            tJump = Time.time;
        }

        //checks if the character has enough room to uncrouch
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
