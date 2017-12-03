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
        public float weight;

        //used for initialization
        void Start()
        {
            //initialize stats with the character type.
            switch(type)
            {
                //TODO: set values from file instead of directly in code.
                case Types.Knight:
                    vitality = 100;
                    strength = 10;
                    agility = 10;
                    weight = 10;
                    break;
                case Types.Goblin:
                    vitality = 80;
                    strength = 12;
                    agility = 5;
                    weight = 15;
                    break;
                case Types.Villager:
                    vitality = 50;
                    strength = 2;
                    agility = 10;
                    weight = 10;
                    break;
                case Types.Skeleton:
                    vitality = 50;
                    strength = 15;
                    agility = 5;
                    weight = 5;
                    break;
                case Types.Orc:
                    vitality = 200;
                    strength = 15;
                    agility = 3;
                    weight = 20;
                    break;
                default:
                    Debug.Log("Error. Character type could not be found.");
                    break;
            }
        }
    }
}
