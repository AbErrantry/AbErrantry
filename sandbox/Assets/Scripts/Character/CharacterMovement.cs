using UnityEngine;

namespace Character2D
{
    public class CharacterMovement : MonoBehaviour
    {
        //character components
        protected Animator anim; //the animator component of the character character
        protected Rigidbody2D rb; //rigidbocdy component of the character character

        //external input
        public bool jumpInput; //jump input from character
        public bool runInput; //run input from character
        public float mvmtSpeed; //horizontal movement speed input from character

        //booleans that drive the character states
        protected bool isGrounded; //whether the character is on the ground or not
        protected bool isJumping; //whether the character is jumping or not
        protected bool isRunning; //whether the character is running or not
        protected bool isMoving; //whether the character is moving or not
        protected bool isFalling; //whether the character is falling or not

        //values that affect the player's speed
        protected float speedMultiplier; //the multiplier to be applied to character movement speed
        protected float maxSpeed; //the maximum speed of the character

        //values that affect the character's ability to jump
        protected bool isInitJump; //initial frame of a jump (to apply jump force)
        protected float jumpForce; //the upwards force of a jump
        protected float tJump; //time since last jump

        //delays after each action
        protected float jumpDelay; //the amount of time before the character can jump again

        //movement speed in each state
        protected float jumpSpeed; //how much jumping decreases the movement speed
        protected float runSpeed; //how much running increases the movement speed

        //position of the player
        protected float lastPosition; //x position of the character on the previous tick
        protected bool isFacingRight; //whether the character is facing right or not

        //used for initialization
        protected void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            jumpInput = false;
            runInput = false;
            mvmtSpeed = 0f;

            isGrounded = true;

            isJumping = false;
            isRunning = false;
            isMoving = false;
            isFalling = false;

            speedMultiplier = 1;
            maxSpeed = 150f;

            isInitJump = true;
            jumpForce = 500.0f;
            tJump = 0.0f;

            jumpDelay = 0.05f;

            jumpSpeed = 0.80f;
            runSpeed = 2.0f;

            lastPosition = 0.0f;
            isFacingRight = true;
        }

        //called once per frame (for input)
        protected void Update()
        {
            HandleInput();
        }

        //called once per game tick (for physics)
        protected void FixedUpdate()
        {
            Move();
            SetMovementLogic();
            SendToAnimator();
        }

        //handles character input
        protected virtual void HandleInput()
        {
            //check if the character can jump
            if (!isJumping && isGrounded && Time.time - tJump > jumpDelay)
            {
                isJumping = jumpInput;
                SendToAnimator();
            }
            //check if the character can run (or stop running)
            if (!isJumping && isGrounded)
            {
                isRunning = runInput;
                SendToAnimator();
            }
        }

        //sends boolean values describing character state to the animator
        protected virtual void SendToAnimator()
        {
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isMoving", isMoving);
            anim.SetBool("isFalling", isFalling);
            anim.SetFloat("mvmtSpeed", Mathf.Abs(mvmtSpeed));
        }

        //moves the character
        protected virtual void Move()
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
            else if (isRunning)
            {
                speedMultiplier = runSpeed;
            }
            else
            {
                speedMultiplier = 1;
            }
            SmoothMove(mvmtSpeed * speedMultiplier * maxSpeed * Time.deltaTime, rb.velocity.y, false);
        }

        protected void SmoothMove(float x, float y, bool z)
        {
            if(Mathf.Abs(Mathf.Abs(rb.velocity.x) - Mathf.Abs(x)) > 0.25)
            {
                x = ((8.0f * rb.velocity.x) + (2.0f * x))/(10.0f);
            }
            rb.velocity = new Vector2(x, y);
            if(z)
            {
                //Debug.Log(rb.velocity.ToString());
            }
        }

        //sets the logic for values related to character movement
        protected virtual void SetMovementLogic()
        {
            //boolean expression that sets whether or not the player has moved this tick
            isMoving = Mathf.Abs(lastPosition - rb.transform.position.x) > 0.0001f ? true : false;

            //boolean expression that sets whether or not the character is falling on this tick
            isFalling = rb.velocity.y < 0 && !isGrounded ? true : false;

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
        public void SetGrounded()
        {
            isGrounded = true;
            isInitJump = true;
            isJumping = false;
            tJump = Time.time;
            SendToAnimator();
        }

        public void SetUngrounded()
        {
            isGrounded = false;
            SendToAnimator();
        }
    }
}
