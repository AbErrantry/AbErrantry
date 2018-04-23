using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class StalactiteHit : MonoBehaviour
	{
		public float destroyDelay;
		public float damage;
		public bool shouldDestroy;
		private GameObject parent;

		private FMOD.Studio.EventInstance hitNoise;

		public void Start()
		{
			hitNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/stalactite");
			hitNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			hitNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
		}

		public void SetVar(float setDamage, GameObject par, float delay)
		{
			damage = setDamage;
			parent = par;
			destroyDelay = delay;
		}

		public void OnCollisionEnter2D(Collision2D col)
		{

			if (col.gameObject.GetComponent<Attackable>() != null) //If it is an attackable it will need to give damage
			{
				StartCoroutine(ObjectDestroy());
				col.gameObject.GetComponent<Attackable>().TakeDamage(gameObject, Mathf.RoundToInt(damage), false);
			}
			else if (col.gameObject != parent) //if it hits anything other than its parent, Destroy it.
			{
				StartCoroutine(ObjectDestroy());
				//			Debug.Log("HIT with damage: " + damage);
				//Destroy(this.gameObject);
			}

		}

		public IEnumerator ObjectDestroy()
		{
			hitNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
			hitNoise.start();
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			GetComponent<Animator>().SetTrigger("Hit"); //Sets the Animation to the next Transition
			AnimatorClipInfo[] animInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0); //Gets Current clip info (Hit info)
			float clipLength = animInfo[0].clip.length; //Gets length of the animation.

			yield return new WaitForSeconds(clipLength * destroyDelay); //You will have to mess around with the delay to get a smooth delete, The value is changed in Falling Stalactite script
			Destroy(this.gameObject);
		}
	}
}
