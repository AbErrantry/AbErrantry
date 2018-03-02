using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
    public class Player : Attackable
    {
        public static Player instance;

        public Animator weaponAnim;

        public CinemachineVirtualCamera virtualCamera;
        private PlayerInput playerInput;
        private TravelMenu travelMenu;

        public Vector2 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        public SpawnManager spawnManager;

        public TMP_Text healthText;

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
        protected new void Start()
        {
            base.Start();
            playerInput = GetComponent<PlayerInput>();
            travelMenu = GetComponent<TravelMenu>();

            healthText.text = currentVitality + "/" + maxVitality;

            //TODO: remove temp AssetBundle loading
            string path = Application.streamingAssetsPath + "/AssetBundles/default";
            AssetBundle.LoadFromFile(path);

            //set from CharacterData
            spawnPoint = transform.position; //todo: set from file
            canFlinch = false;
            canKnockBack = true;
            canTakeDamage = true;
        }

        public override void TakeDamage(GameObject attacker, float damage)
        {
            base.TakeDamage(attacker, damage);
            if (currentVitality < 0)
            {
                currentVitality = 0;
            }
            healthText.text = currentVitality + "/" + maxVitality;
        }

        protected override void InitializeDeath()
        {
            if (GetComponent<BackpackMenu>().isOpen)
            {
                GetComponent<BackpackMenu>().CloseBackpackMenu();
            }
            if (GetComponent<Dialogue2D.DialogueManager>().isOpen)
            {
                GetComponent<Dialogue2D.DialogueManager>().EndDialogue();
            }
            if (GetComponent<PlayerInteraction>().isOpen)
            {
                GetComponent<PlayerInteraction>().CloseContainer();
            }
            //take away player input
            ToggleCamera(false);
            playerInput.DisableInput();

            isDying = true;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            //enemies no longer target player
            StartCoroutine(TravelMenuDelay());
        }

        private IEnumerator TravelMenuDelay()
        {
            yield return new WaitForSeconds(2.0f);
            travelMenu.Open("You died");
        }

        public override void FinalizeDeath()
        {
            //enemies target player
            //give player back input
            //death penalty: 25% of gold?
            playerInput.InvokeSleep();
        }

        public void Respawn()
        {
            isDying = false;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            currentVitality = maxVitality;
            healthText.text = currentVitality + "/" + maxVitality;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = spawnPoint;
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
            }
            StartCoroutine(CameraToggleDelay());
        }

        public void ToggleCamera(bool isActive)
        {
            CameraShift.instance.ToggleDamping(isActive);
            virtualCamera.enabled = isActive;
        }

        public void SetSpawn(Vector2 loc, SpawnManager mgr)
        {
            spawnPoint = loc;
            spawnManager = mgr;
        }

        private IEnumerator CameraToggleDelay()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            ToggleCamera(true);
        }
    }
}
