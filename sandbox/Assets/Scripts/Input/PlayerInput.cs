using System.Collections;
using UnityEngine;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput instance;

        private PlayerMovement playerMovement;
        private PlayerInteraction playerInteraction;
        private PlayerAttack playerAttack;

        // public PlayerPause playerPause;
        private BackpackMenu backpackMenu;
        public InteractionTrigger interactTrigger;
        public bool acceptInput;

        public GameObject loadingContainer;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        //used for initialization
        private void Start()
        {
            acceptInput = true; //to be used for menu navigation
            playerMovement = GetComponent<PlayerMovement>();
            playerInteraction = GetComponent<PlayerInteraction>();
            playerAttack = GetComponent<PlayerAttack>();
            backpackMenu = GetComponent<BackpackMenu>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (acceptInput)
            {
                playerMovement.jumpInput = Input.GetButtonDown("Jump"); //send jump input pressed
                playerMovement.crouchInput = Input.GetButton("Crouch"); //send crouch input
                playerMovement.runInput = Input.GetButton("Run"); //send run input
                playerMovement.mvmtSpeed = Input.GetAxis("Move"); //send movement speed
                playerMovement.climbSpeedInput = Input.GetAxis("Vertical"); //send movement speed
                playerInteraction.interactionInput = Input.GetButtonDown("Interact"); //send interaction input
                playerAttack.attackInputDown = Input.GetButtonDown("Attack"); //send attack input pressed
                playerAttack.attackInputUp = Input.GetButtonUp("Attack"); //send attack input released
                // playerPause.pauseInput = Input.GetButtonDown("Pause"); //send pause input
            }

            if (Input.GetButtonDown("Backpack"))
            {
                if ((!acceptInput && backpackMenu.isOpen) || acceptInput)
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

        public void EnableInput(bool menu = false)
        {
            if (menu)
            {
                //have to wait for the end of the frame
                StartCoroutine(EnableInputRoutine());
            }
            else
            {
                acceptInput = true;
                interactTrigger.EnableTrigger();
            }

        }

        private IEnumerator EnableInputRoutine()
        {
            yield return new WaitForEndOfFrame();
            acceptInput = true;
            interactTrigger.EnableTrigger();
        }

        public void ToggleLoadingContainer(bool toggle)
        {
            loadingContainer.SetActive(toggle);
        }
    }
}
