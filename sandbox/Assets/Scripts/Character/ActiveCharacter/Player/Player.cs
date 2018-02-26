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
        public PlayerMovement playerMovement;
        public PlayerInput playerInput;

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
            healthText.text = currentVitality + "/" + maxVitality;
        }

        protected override void InitializeDeath()
        {
            //take away player input
            playerInput.DisableInput();
            isDying = true;
            anim.SetBool("isDying", isDying); //death animation
            weaponAnim.SetBool("isDying", isDying); //death animation
            //enemies no longer target player
            //screen overlay of death?
        }

        public override void FinalizeDeath()
        {
            //enemies target player
            //give player back input
            //death penalty: 25% of gold?
            isDying = false;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            playerInput.EnableInput();
            Respawn();
        }

        private void Respawn()
        {
            ToggleCamera(false);
            currentVitality = maxVitality;
            healthText.text = currentVitality + "/" + maxVitality;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = spawnPoint;
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
            }
            ToggleCamera(true);
        }

        public void ToggleCamera(bool isActive)
        {
            virtualCamera.enabled = isActive;
        }

        public void SetSpawn(Vector2 loc, SpawnManager mgr)
        {
            spawnPoint = loc;
            spawnManager = mgr;
        }
    }
}
