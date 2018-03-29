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
            Skeleton,
            Slime,
            Rat,
            Crate,
            Dummy,
            ExplodingCrate,
            DestroyableTile,
            Bear,
            Ogre,
            Golem,
            Wraith,
            Snowman,
            Dragon,
        }

        public Types type; //the type of character

        public CharacterFields fields;

        public void Start()
        {
            fields = GameData.data.characterData.characterDictionary[type.ToString()];
        }
    }
}
