using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public CharacterMovement characterMovement;
        public CharacterInteraction characterInteraction;
        public CharacterAttack characterAttack;

        public bool acceptInput;

        //used for initialization
        private void Start()
        {
            acceptInput = true; //to be used for menu navigation
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.timeScale == 1)
            {
                characterMovement.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input
                characterMovement.crouchInput = CrossPlatformInputManager.GetButton("Crouch"); //send crouch input
                characterMovement.runInput = CrossPlatformInputManager.GetButton("Run"); //send run input
                characterMovement.mvmtSpeed = CrossPlatformInputManager.GetAxis("Move"); //send movement speed
                characterInteraction.interactionInput = CrossPlatformInputManager.GetButtonDown("Interact"); //send interaction input

                characterAttack.attackInputDown = CrossPlatformInputManager.GetButtonDown("Attack"); //send attack input
                characterAttack.attackInputUp = CrossPlatformInputManager.GetButtonUp("Attack"); //send attack input
            }
        }
    }
}
