using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class Crate : Attackable
    {
        public BoxMove boxMove;

        protected FMOD.Studio.EventInstance crateHit;
        protected FMOD.Studio.EventInstance crateDeath;

        // Use this for initialization
        protected new void Start()
        {
            base.Start();
            canTakeDamage = true;
            canKnockBack = false;
            canFlinch = true;

            crateHit = FMODUnity.RuntimeManager.CreateInstance("event:/Crate/take_damage");
            crateHit.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            crateDeath = FMODUnity.RuntimeManager.CreateInstance("event:/Crate/death");
            crateDeath.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            FMODUnity.RuntimeManager.AttachInstanceToGameObject(crateHit, GetComponent<Transform>(), GetComponent<Rigidbody>());
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(crateDeath, GetComponent<Transform>(), GetComponent<Rigidbody>());
        }

        public override void TakeDamage(GameObject attacker, int damage, bool appliesKnockback = true)
        {
            base.TakeDamage(attacker, damage, appliesKnockback);
            crateHit.start();
        }

        protected override void InitializeDeath()
        {
            boxMove.enabled = false;
            Destroy(GetComponent<CompositeCollider2D>());
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            anim.Play("DESTROY");
            crateDeath.start();
        }

        public override void FinalizeDeath()
        {
            Destroy(gameObject);
        }

        public virtual void DestroyCrate()
        {
            InitializeDeath();
        }
    }
}
