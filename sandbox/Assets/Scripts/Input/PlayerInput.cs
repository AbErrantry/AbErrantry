using System.Collections;
using UnityEngine;

namespace Character2D
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput instance;

        private Animator anim;
        public Animator weaponAnim;

        private PlayerMovement playerMovement;
        private PlayerInteraction playerInteraction;
        private PlayerAttack playerAttack;

        private TravelMenu travelMenu;

        private Dialogue2D.DialogueManager dialogueManager;

        // public PlayerPause playerPause;
        private BackpackMenu backpackMenu;
        public InteractionTrigger interactTrigger;
        public bool acceptInput;

        public GameObject loadingContainer;

        public bool isAsleep;
        public bool awakeInvoked;

        private float timeToSleep;
        private float sleepTimer;

        public InputManager inputManager;

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
            travelMenu = GetComponent<TravelMenu>();
            dialogueManager = GetComponent<Dialogue2D.DialogueManager>();
            anim = GetComponent<Animator>();

            sleepTimer = Time.time;
            timeToSleep = 30.0f;

            isAsleep = false;
            awakeInvoked = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (acceptInput)
            {
                playerMovement.jumpInput = Input.GetButtonDown("Jump"); //send jump input pressed
                playerMovement.crouchInput = Input.GetButton("Crouch"); //send crouch input
                playerMovement.runInput = Input.GetButton("Run"); //send run input
                playerMovement.mvmtSpeed = Input.GetAxisRaw("Move"); //send movement speed
                playerMovement.climbSpeedInput = Input.GetAxisRaw("Vertical"); //send climb speed
                playerInteraction.interactionInput = Input.GetButtonDown("Interact"); //send interaction input
                playerAttack.attackInputDown = Input.GetButtonDown("Attack"); //send attack input pressed
                playerAttack.attackInputUp = Input.GetButtonUp("Attack"); //send attack input released
                playerInteraction.pauseInput = Input.GetButtonDown("Pause");
            }

            if (Input.GetButtonDown("Jump") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButton("Crouch") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButton("Run") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetAxisRaw("Move") != 0.0f)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetAxisRaw("Vertical") != 0.0f)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButtonDown("Interact") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButtonDown("Attack") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButtonUp("Attack") != false)
            {
                sleepTimer = Time.time;
            }
            if (Input.GetButtonDown("Pause") != false)
            {
                sleepTimer = Time.time;
            }

            if (Input.GetButtonDown("Backpack"))
            {
                sleepTimer = Time.time;
                if ((!acceptInput && backpackMenu.isOpen) || acceptInput)
                {
                    backpackMenu.ToggleBackpack();
                }
            }
            else if (Input.GetButtonDown("Interact"))
            {
                sleepTimer = Time.time;
                if ((!acceptInput && playerInteraction.isOpen) || acceptInput)
                {
                    playerInteraction.CloseContainer();
                }
            }

            if (playerMovement.isClimbing || playerMovement.isCrouching || playerMovement.isFalling)
            {
                sleepTimer = Time.time;
            }

            if (Time.time - sleepTimer > timeToSleep && !isAsleep)
            {
                InvokeSleep();
            }
            else if (Time.time - sleepTimer < timeToSleep && isAsleep && !awakeInvoked)
            {
                if (travelMenu.isTravelling)
                {
                    sleepTimer = Time.time - timeToSleep - 1.0f;
                }
                else
                {
                    InvokeAwake();
                }
            }
        }

        public void InvokeSleep()
        {
            isAsleep = true;
            DisableInput();
            weaponAnim.SetBool("isSleeping", true);
            anim.SetBool("isSleeping", true);
        }

        public void InvokeAwake()
        {
            awakeInvoked = true;
            EnableInput();
            weaponAnim.SetBool("isSleeping", false);
            anim.SetBool("isSleeping", false);
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
            playerMovement.climbSpeedInput = 0.0f;
            playerInteraction.interactionInput = false;
            playerAttack.attackInputDown = false;
            playerAttack.attackInputUp = false;
        }

        public void EnableInput(bool menu = false)
        {
            if (menu && !isAsleep)
            {
                //have to wait for the end of the frame
                StartCoroutine(EnableInputRoutine());
            }
            else if (menu && isAsleep)
            {
                StartCoroutine(WakeUp(false));
            }
            else if (isAsleep)
            {
                if (backpackMenu.isOpen || playerInteraction.isOpen || dialogueManager.isOpen)
                {
                    StartCoroutine(WakeUp(true));
                }
                else
                {
                    StartCoroutine(WakeUp(false));
                }
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

        private IEnumerator WakeUp(bool menu)
        {
            yield return new WaitForSeconds(2.0f);
            isAsleep = false;
            awakeInvoked = false;
            if (!menu)
            {
                acceptInput = true;
                interactTrigger.EnableTrigger();
            }
        }

        public void ToggleLoadingContainer(bool toggle)
        {
            loadingContainer.SetActive(toggle);
        }

        public void EnableSpawn()
        {
            travelMenu.EnableButtons();
        }
    }
}
