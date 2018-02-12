using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public PlayerInteraction playerInteraction;
        public PlayerAttack playerAttack;

        // public PlayerPause playerPause;
        public BackpackMenu backpackMenu;
        public InteractionTrigger interactTrigger;
        public bool acceptInput;

        public GameObject loadingContainer;

        //used for initialization
        private void Start()
        {
            acceptInput = true; //to be used for menu navigation
        }

        // Update is called once per frame
        private void Update()
        {
            if (acceptInput)
            {
                playerMovement.jumpInput = CrossPlatformInputManager.GetButtonDown("Jump"); //send jump input pressed
                playerMovement.crouchInput = CrossPlatformInputManager.GetButton("Crouch"); //send crouch input
                playerMovement.runInput = CrossPlatformInputManager.GetButton("Run"); //send run input
                playerMovement.mvmtSpeed = CrossPlatformInputManager.GetAxis("Move"); //send movement speed
                playerMovement.climbSpeedInput = CrossPlatformInputManager.GetAxis("Vertical"); //send movement speed
                playerInteraction.interactionInput = CrossPlatformInputManager.GetButtonDown("Interact"); //send interaction input
                playerAttack.attackInputDown = CrossPlatformInputManager.GetButtonDown("Attack"); //send attack input pressed
                playerAttack.attackInputUp = CrossPlatformInputManager.GetButtonUp("Attack"); //send attack input released
                // playerPause.pauseInput = CrossPlatformInputManager.GetButtonDown("Pause"); //send pause input
            }

            if (CrossPlatformInputManager.GetButtonDown("Backpack"))
            {
                if ((!acceptInput && backpackMenu.isOpen)|| acceptInput)
                {
                    backpackMenu.ToggleBackpack();
                }
            }
        }

        public void DisableInput(bool isInteractList = false)
        {
            acceptInput = false;
            if (!isInteractList)
            {
                interactTrigger.DisableTrigger();
            }

            playerMovement.jumpInput = false;
            playerMovement.crouchInput = false;
            playerMovement.runInput = false;
            playerMovement.mvmtSpeed = 0.0f;
            playerMovement.climbSpeed = 0.0f;
            playerInteraction.interactionInput = false;
            playerAttack.attackInputDown = false;
            playerAttack.attackInputUp = false;
        }

        public void EnableInput()
        {
            acceptInput = true;
            interactTrigger.EnableTrigger();
        }

        public void ToggleLoadingContainer(bool toggle)
        {
            loadingContainer.SetActive(toggle);
        }
    }
}
