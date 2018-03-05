using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
public class SpikeDamage : MonoBehaviour {

	[Tooltip("Set the amount of damage this set of spikes does (2 min, 1000 max).")]
	[Range(2.0f, 1000.0f)]
	public float spikeDamage;
	 private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.GetComponent<Attackable>().TakeDamage(gameObject, Mathf.RoundToInt(spikeDamage));
    }
}
}