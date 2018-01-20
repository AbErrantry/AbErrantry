using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Character2D
{
    public class Player : ActiveCharacter
    {
        public CinemachineVirtualCamera virtualCamera;

        public PlayerMovement playerMovement;
        public PlayerInput playerInput;

        public Vector3 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        private float maxVitality; //the maximum value of health

        //used for initialization
        protected new void Start()
        {
			base.Start();
            //set from CharacterData
            spawnPoint = transform.position; //todo: set from file
            maxVitality = vitality; //todo: set from file
        }

        protected override void InitializeDeath()
        {
            //take away player input
            playerInput.DisableInput();
            isDying = true;
            anim.SetBool("isDying", isDying); //death animation 
            //enemies no longer target player
            //screen overlay of death?
        }

        public override void FinalizeDeath()
        {
            //enemies target player
            //give player back input
            isDying = false;
            anim.SetBool("isDying", isDying);
            Respawn();
            playerInput.EnableInput();
        }

        private void Respawn()
        {
            ToggleCamera(false);
            vitality = maxVitality; //TODO: uncomment
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

