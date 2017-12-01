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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                //aiMovement.jumpInput = true; //send jump input

                //cm.crouchInput = CrossPlatformInputManager.GetButton("Fire1"); //send crouch input
                // cm.runInput = CrossPlatformInputManager.GetButton("Fire3"); //send run input

                
                aiMovement.mvmtSpeed = -.5f; //send movement speed
               // ci.interactionInput = CrossPlatformInputManager.GetButtonDown("Fire2"); //send interaction input
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
