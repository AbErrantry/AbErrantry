using UnityEngine;

namespace Character2D
{
    public class BehaviorAI : MonoBehaviour
    {
        public CharacterMovement aiMovement; //reference to the character movement component
        public CharacterInteraction aiInteraction; //reference to the character interaction component
        public AICrouchManager topCrouch; //reference to the top trigger ai crouch manager component
        public AICrouchManager botCrouch; //reference to the bottom trigger ai crouch manager component

        public float secondsToPatrol; //How many seconds should the AI patrol in one direction
        private float currDirection; 
        public float timeToDist;
        private static bool trackingPlayer;

        //used for initialization
        private void Start()
        {
            currDirection = 1;
            aiMovement.mvmtSpeed = 1;
            timeToDist = secondsToPatrol;
        }

        //occurs after all Update methods
        //TODO: implement extents for ai pathing as opposed to time (and move out of update)
        private void LateUpdate()
        {
            if (Time.timeScale == 1  && aiMovement.crouchInput == false && aiMovement.jumpInput == false)
            {
                //As Long as the AI is not tracking the player then it can change directions.
                if (timeToDist <= 0 && trackingPlayer == false) 
                {
                    currDirection = aiMovement.mvmtSpeed * -1; //switch direction
                    aiMovement.mvmtSpeed = currDirection; 
                    timeToDist = secondsToPatrol; //Reset the time
                }
                else
                {
                    timeToDist -= Time.deltaTime;
                }
            }
        }

        //sets whether the character is tracking a target or not
        public void AITrack(bool shouldWalk)
        {
            if (shouldWalk)
            {
                aiMovement.runInput = true;
                trackingPlayer = true;
            }
            else
            {
                aiMovement.runInput = false;
                trackingPlayer = false;
            }
        }

        //ai tries to send input to jump or crouch depending on situation
        public void InputAttempt()
        {
            aiMovement.jumpInput = false;
            aiMovement.crouchInput = false;
            if (topCrouch.currentObstacles.Count != 0 && botCrouch.currentObstacles.Count == 0)
            {
                aiMovement.crouchInput = true; //send crouch input
            }
            else if(topCrouch.currentObstacles.Count == 0 && botCrouch.currentObstacles.Count != 0)
            {
                aiMovement.jumpInput = true; //send jump input
            }
        }
    }
}
