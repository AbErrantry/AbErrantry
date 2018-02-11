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
            canTakeDamage = false;
            canKnockBack = false;
            canFlinch = true;
        }

        protected override void InitializeDeath()
        { }

        public override void FinalizeDeath()
        { }
    }
}
