using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class PlayerAttack : ActiveCharacterAttack
    {
        public bool canAttack;
        
        public bool attackInputDown;
        public bool attackInputUp;

        protected bool isInitAttack;

        protected float attackPress;
        protected float attackRelease;

        //used for initialization
        protected new void Start()
        {
            base.Start();

            attackInputDown = false;
            attackInputUp = true;

            isInitAttack = false;

            attackPress = 0.0f;
            attackRelease = 0.0f;
        }

        //
        protected new void Update()
        {
            base.Update();

            if (!isAttacking && canAttack)
            {
                if (attackInputDown && !isWindingUp)
                {
                    attackPress = Time.time;
                    isWindingUp = true;
                    SendToAnimator();
                }
                else if (attackInputUp && isWindingUp)
                {
                    attackRelease = Time.time;
                    isWindingUp = false;
                    isInitAttack = true;
                    SendToAnimator();
                }
            }
        }

        //
        protected new void FixedUpdate()
        {
            base.FixedUpdate();

            if(isInitAttack)
            {
                isInitAttack = false;
                float attackDuration = attackRelease - attackPress;
                if (attackDuration > swingThreshold)
                {
                    StartCoroutine(SwingAttack());
                }
                else
                {
                    StartCoroutine(StabAttack());
                }
            }
            else if(isWindingUp)
            {
                if(Time.time - attackPress > powerThreshold)
                {
                    StartCoroutine(PowerAttack());
                }
            }
        }
    }
}
