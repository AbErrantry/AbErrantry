using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class BehaviorAI : MonoBehaviour
    {
        public CharacterMovement aiMovement;
        public CharacterInteraction aiInteraction;
        public AICrouchManager topCrouch;
        public AICrouchManager botCrouch;
        public AIMoveManager aiMoving;

        public float secondsToPatrol; //How many seconds should the AI patrol in one direction
        private float currDirection;
        public float timeToDist;
        private static bool trackingPlayer;

        // Use this for initialization
        void Start()
        {
            currDirection = 1;
            aiMovement.mvmtSpeed = 1;
            timeToDist = secondsToPatrol;
        }

        void LateUpdate()
        {
            if (Time.timeScale == 1  && aiMovement.crouchInput == false && aiMovement.jumpInput == false)
            {
                if (timeToDist <= 0 && trackingPlayer == false) //As Long as the AI is not tracking the player then it can change directions.
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


        public void AIMove(bool shouldWalk)
        {
            if (shouldWalk)
            {
                
                aiMovement.mvmtSpeed = currDirection * 2f; //Speed up if tracking the player
                trackingPlayer = true;
            }
            else
            {
                aiMovement.mvmtSpeed = currDirection; //Go back to patrolling speed.
                trackingPlayer = false;
            }
        }


        public void Crouch()
        {
            if (topCrouch.currentCeiling.Count != 0 && botCrouch.currentCeiling.Count == 0)
            {
                aiMovement.crouchInput = true; //send crouch input
                aiMovement.jumpInput = false; //send jump input
            }
            else if(topCrouch.currentCeiling.Count == 0 && botCrouch.currentCeiling.Count != 0)
            {
                aiMovement.jumpInput = true; //send jump input
                aiMovement.crouchInput = false; //send un-crouch input
            }
            else
            {
                aiMovement.jumpInput = false; //send jump input
                aiMovement.crouchInput = false; //send un-crouch input
            }
        }
    }
}
