using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Dummy : Attackable
    {
        // Use this for initialization
        private new void Start()
        {
            base.Start();
            canTakeDamage = true;
            canKnockBack = false;
            canFlinch = true;
        }

        protected override void InitializeDeath()
        {
            //TODO: add destruction animation
        }

        public override void FinalizeDeath()
        {
            Destroy(gameObject);
        }
    }
}
