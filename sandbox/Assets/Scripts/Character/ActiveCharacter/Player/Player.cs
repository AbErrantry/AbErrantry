using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Character2D
{
    public class Player : ActiveCharacter
    {
        public CinemachineVirtualCamera virtualCamera;

        public Vector3 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        private float maxVitality; //the maximum value of health

        //used for initialization
        protected new void Start()
        {
			base.Start();
            //set from CharacterData
            maxVitality = vitality;
        }

        protected override void Die()
        {
            //take away player input
            //enemies no longer target player

            //death animation for player
            //screen overlay of death?

            Respawn();

            //enemies target player
            //give player back input
        }

        private void Respawn()
        {
            vitality = maxVitality;
            transform.position = spawnPoint;
            Debug.Log("player died. respawning.");
        }

        public void ToggleCamera(bool isActive)
        {
            virtualCamera.enabled = isActive;
        }
    }
}

