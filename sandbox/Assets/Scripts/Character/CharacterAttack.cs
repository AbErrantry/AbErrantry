using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        public bool attackInput;
        private float attackTime;

        // Use this for initialization
        void Start()
        {
            attackInput = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(attackInput)
            {

            }
        }
    }
}
