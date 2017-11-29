using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public InteractionManager interactionManager; //reference to the interaction manager
        public CharacterMovement characterMovement;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                characterMovement.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input
                characterMovement.crouchInput = CrossPlatformInputManager.GetButton("Fire1"); //send crouch input
                characterMovement.runInput = CrossPlatformInputManager.GetButton("Fire3"); //send run input
                characterMovement.mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //send movement speed
                if (CrossPlatformInputManager.GetButtonDown("Fire2"))
                {
                    interactionManager.InteractPress();
                }
            }
        }
    }
}
