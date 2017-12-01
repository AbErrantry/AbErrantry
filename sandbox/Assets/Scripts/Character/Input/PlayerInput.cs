using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public CharacterMovement cm;
        public CharacterInteraction ci;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                cm.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input
                cm.crouchInput = CrossPlatformInputManager.GetButton("Fire1"); //send crouch input
                cm.runInput = CrossPlatformInputManager.GetButton("Fire3"); //send run input
                cm.mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //send movement speed
                ci.interactionInput = CrossPlatformInputManager.GetButtonDown("Fire2"); //send interaction input
            }
        }
    }
}
