using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Character : MonoBehaviour
    {
        public enum Types //enumeration of character types
        {
            Knight,
            Goblin,
            Villager,
            Skeleton,
            Orc
        }

        public Types type; //the type of character

        public float vitality; //the vitality of the character
        public float strength; //the strength of the character
        public float agility; //the agility of the character
        public float weight; //the weight of the character
        public bool isActive; //whether or not the character is active
    }
}
