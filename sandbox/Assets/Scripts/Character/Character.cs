using UnityEngine;
using UnityEngine.Collections;
using System.Linq;
using System;
using System.IO;

namespace Character2D
{
    public class Character : MonoBehaviour
    {
        public enum Types //enumeration of character types
        {
            Knight, Goblin, Villager, Skeleton, Orc
        };

        public Types type; //the type of character

        public float vitality; //the vitality of the character
        public float strength; //the strength of the character
        public float agility; //the agility of the character
        public float weight; //the weight of the character

        public Vector2 spawnPoint; //the spawnpoint upon death
        private float maxVitality; //the maximum value of health

        //used for initialization
        private void Start()
        {
            //set from CharacterData
            vitality = maxVitality;
        }

        //applies damage to the player
        public void TakeDamage(float damage)
        {
            vitality = vitality - damage;
            if (vitality <= 0f)
            {
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
