using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public CharacterMovement characterMovement;
        public CharacterInteraction characterInteraction;

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                characterMovement.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input
                characterMovement.crouchInput = CrossPlatformInputManager.GetButton("Fire1"); //send crouch input
                characterMovement.runInput = CrossPlatformInputManager.GetButton("Fire3"); //send run input
                characterMovement.mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //send movement speed
                characterInteraction.interactionInput = CrossPlatformInputManager.GetButtonDown("Fire2"); //send interaction input
            }
        }
    }
}
