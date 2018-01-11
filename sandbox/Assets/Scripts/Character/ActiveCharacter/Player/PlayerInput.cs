using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public CharacterInteraction characterInteraction;
        public CharacterAttack characterAttack;

        public BackpackMenu backpackMenu;

        public bool acceptInput;

        //used for initialization
        private void Start()
        {
            acceptInput = true; //to be used for menu navigation
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.timeScale != 0)
            {
                playerMovement.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input pressed
                playerMovement.crouchInput = CrossPlatformInputManager.GetButton("Crouch"); //send crouch input
                playerMovement.runInput = CrossPlatformInputManager.GetButton("Run"); //send run input
                playerMovement.mvmtSpeed = CrossPlatformInputManager.GetAxis("Move"); //send movement speed
                characterInteraction.interactionInput = CrossPlatformInputManager.GetButtonDown("Interact"); //send interaction input

                characterAttack.attackInputDown = CrossPlatformInputManager.GetButtonDown("Attack"); //send attack input pressed
                characterAttack.attackInputUp = CrossPlatformInputManager.GetButtonUp("Attack"); //send attack input released
            }

            if (CrossPlatformInputManager.GetButtonDown("Backpack"))
            {
                backpackMenu.ToggleBackpack();
            }
        }
    }
}
