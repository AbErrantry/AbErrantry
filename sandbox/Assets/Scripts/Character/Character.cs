using UnityEngine;
using UnityEngine.Collections;

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
            //initialize stats with the character type.
            switch(type)
            {
                //TODO: set values from file instead of directly in code.
                case Types.Knight:
                    maxVitality = 100;
                    strength = 10;
                    agility = 10;
                    weight = 10;
                    break;
                case Types.Goblin:
                    maxVitality = 80;
                    strength = 12;
                    agility = 5;
                    weight = 15;
                    break;
                case Types.Villager:
                    maxVitality = 50;
                    strength = 2;
                    agility = 10;
                    weight = 10;
                    break;
                case Types.Skeleton:
                    maxVitality = 50;
                    strength = 15;
                    agility = 5;
                    weight = 5;
                    break;
                case Types.Orc:
                    maxVitality = 200;
                    strength = 15;
                    agility = 3;
                    weight = 20;
                    break;
                default:
                    Debug.Log("Error. Character type could not be found.");
                    break;
            }
            vitality = maxVitality;
        }

        //applies damage to the player
        public void TakeDamage(float damage)
        {
            vitality = vitality - damage;
            if (vitality < 0)
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
