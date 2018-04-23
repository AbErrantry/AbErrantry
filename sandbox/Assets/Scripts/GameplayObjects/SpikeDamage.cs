using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class SpikeDamage : MonoBehaviour
	{
		private FMOD.Studio.EventInstance hitNoise;

		[Tooltip("Set the amount of damage this set of spikes does (2 min, 1000 max).")]
		[Range(1.0f, 50.0f)]
		public float spikeDamage;
		public bool shouldKill;

		private void Start()
		{
			hitNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/spike_impale");
			hitNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
			hitNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.gameObject.GetComponent<Attackable>() != null)
			{
				hitNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
				hitNoise.start();

				if (shouldKill)
				{
					coll.gameObject.GetComponent<Attackable>().Kill();
				}
				else
				{
					coll.gameObject.GetComponent<Attackable>().TakeDamage(gameObject, Mathf.RoundToInt(spikeDamage));
				}
			}
		}
	}
}
