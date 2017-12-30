using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class PlayerCharacterBehavior : CharacterBehavior
    {
        public Vector2 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        private float maxVitality; //the maximum value of health

        //used for initialization
        void Start()
        {
            //set from CharacterData
            maxVitality = vitality;
        }

        //applies damage to the player
        public override void TakeDamage(float damage)
        {
            vitality = vitality - damage;
            if (vitality <= 0f)
            {
                //kill
                Respawn();
            }
        }

        private void Respawn()
        {
            //take away player input
            //enemies no longer target player

            //death animation for player
            //screen overlay of death?
            //respawn at spawnPoint

            vitality = maxVitality;

            Debug.Log("enemy died. respawning.");
            //enemies target player
            //give player back input
        }

        
    }
}
