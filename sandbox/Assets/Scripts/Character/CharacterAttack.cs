using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        public bool attackInput;
        public bool isAttacking;
        public bool isInitAttack;
        public bool isWindingUp;

        private float attackPress;
        private float attackRelease;

        //used for initialization
        private void Start()
        {
            attackInput = false;
            isAttacking = false;
            isInitAttack = false;
            isWindingUp = false;
        }

        //
        private void Update()
        {
            if (attackInput && !isWindingUp)
            {
                attackPress = Time.time;
                isWindingUp = true;
            }
            else if(!attackInput && isWindingUp)
            {
                attackRelease = Time.time;
                isWindingUp = false;
                isInitAttack = true;
            }
        }

        //
        private void FixedUpdate()
        {
            if(isInitAttack)
            {
                isAttacking = true;
                isInitAttack = false;
                if (attackRelease - attackPress > 0.25)
                {
                    SwingAttack();
                    Debug.Log("swing");
                    isAttacking = false;
                }
                else
                {
                    StabAttack();
                    Debug.Log("stab");
                    isAttacking = false;
                }
            }
        }

        //invokes the stab attack
        public void StabAttack()
        {

        }

        //invokes the swing attack
        public void SwingAttack()
        {

        }

        //invokes the power attack
        public void PowerAttack()
        {

        }
    }
}
