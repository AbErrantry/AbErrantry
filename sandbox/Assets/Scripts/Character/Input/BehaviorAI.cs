using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class BehaviorAI : MonoBehaviour
    {
        public CharacterMovement aiMovement;
        public CharacterInteraction aiInteraction;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 1)
            {
                aiMovement.jumpInput = true; //send jump input
                /*
                cm.crouchInput = CrossPlatformInputManager.GetButton("Fire1"); //send crouch input
                cm.runInput = CrossPlatformInputManager.GetButton("Fire3"); //send run input
                cm.mvmtSpeed = CrossPlatformInputManager.GetAxis("Horizontal"); //send movement speed
                ci.interactionInput = CrossPlatformInputManager.GetButtonDown("Fire2"); //send interaction input
                */
            }
        }
    }
}
