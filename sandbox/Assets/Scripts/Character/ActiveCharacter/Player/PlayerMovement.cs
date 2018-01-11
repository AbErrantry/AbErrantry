using UnityEngine;

namespace Character2D
{
    public class PlayerMovement : ActiveCharacterMovement
    {
        //external input
        public bool crouchInput; //crouch input from character

        //external booleans that drive the character states
        public bool canUncrouch; //whether the character can uncrouch or not

        //booleans that drive the character states
        protected bool isCrouching; //whether the character is crouching or not

        //values that affect the character's ability to crouch/uncrouch
        protected float tCrouch; //time since the last crouch
        protected bool isInitCrouch; //initial frame of a crouch (to apply crouch logic)

        //delays after each action
        protected float crouchDelay; //the amount of time before the character can uncrouch
        protected float slideDelay; //the duration of the slide animation

        //movement speed in each state
        protected float crouchSpeed; //how much crouching decreases the movement speed

        //used for initialization
        protected new void Start()
        {
            base.Start();

            crouchInput = false;

            canUncrouch = true;

            isCrouching = false;

            tCrouch = 0.0f;
            isInitCrouch = true;

            crouchDelay = 0.75f;
            slideDelay = 0.25f;

            crouchSpeed = 0.50f;
        }

        //called once per frame (for input)
        protected new void Update()
        {
            HandlePlayerInput();
        }

        //called once per game tick (for physics)
        protected new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        //handles player input
        protected void HandlePlayerInput()
        {
            //check if the character can jump
            if (!isJumping && !isCrouching && isGrounded && Time.time - tJump > jumpDelay)
            {
                isJumping = jumpInput;
                SendMovementToAnimator();
            }
            //check if the character can run (or stop running)
            if (!isJumping && !isCrouching && isGrounded)
            {
                isRunning = runInput;
                SendMovementToAnimator();
            }
            //check if the character can crouch (or stop crouching)
            if (!isJumping && isGrounded)
            {
                //the character can only crouch when they are not crouching
                //the character can only uncrouch when they have room to stand up
                if(!isCrouching || CheckIfCanUncrouch())
                {
                    isCrouching = crouchInput;
                    SendCrouchToAnimator();
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
        protected void SendCrouchToAnimator()
        {
            anim.SetBool("isCrouching", isCrouching);
        }

        //moves the character
        protected override bool ExtraMovement()
        {
            if (isCrouching)
            {
                if(isRunning && Time.time - tCrouch < slideDelay)
                {
                    speedMultiplier = runSpeed * crouchSpeed;
                }
                else
                {
                    speedMultiplier = crouchSpeed;
                }
                return true;
            }
            return false;
        }
        

        //checks if the character has enough room to uncrouch
        protected bool CheckIfCanUncrouch()
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
