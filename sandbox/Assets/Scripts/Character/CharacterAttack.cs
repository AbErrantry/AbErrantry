using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class CharacterAttack : MonoBehaviour
    {
        public bool attackInputDown;
        public bool attackInputUp;

        public bool isAttacking;
        public bool isInitAttack;
        public bool isWindingUp;

        private float attackPress;
        private float attackRelease;

        //used for initialization
        private void Start()
        {
            attackInputDown = false;
            attackInputUp = true;

            isAttacking = false;
            isInitAttack = false;
            isWindingUp = false;
        }

        //
        private void Update()
        {
            if (attackInputDown && !isWindingUp)
            {
                attackPress = Time.time;
                isWindingUp = true;
            }
            else if(attackInputUp && isWindingUp)
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
                if (attackRelease - attackPress > 0.20f) //TODO: fix
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
            else if(isWindingUp)
            {
                if(Time.time - attackPress > 1.0f) //TODO: fix
                {
                    isWindingUp = false;
                    isAttacking = true;
                    PowerAttack();
                    Debug.Log("power");
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
