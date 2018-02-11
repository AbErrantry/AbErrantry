using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Player : Attackable
    {
        public Animator weaponAnim;
        public CinemachineVirtualCamera virtualCamera;
        public PlayerMovement playerMovement;
        public PlayerInput playerInput;
        public Vector3 spawnPoint; //the spawnpoint upon death (one of the fast travel points)

        //used for initialization
        protected new void Start()
        {
            base.Start();
            //set from CharacterData
            spawnPoint = transform.position; //todo: set from file
            canFlinch = false;
            canKnockBack = true;
            canTakeDamage = true;
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
            Respawn();
            playerInput.EnableInput();
        }

        private void Respawn()
        {
            ToggleCamera(false);
            currentVitality = maxVitality; //TODO: uncomment
            transform.position = spawnPoint;
            Debug.Log("player died. respawning.");
            ToggleCamera(true);
        }

        public void ToggleCamera(bool isActive)
        {
            virtualCamera.enabled = isActive;
        }
    }
}
