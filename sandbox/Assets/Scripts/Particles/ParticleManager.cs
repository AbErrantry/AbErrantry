using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	public static ParticleManager instance;
	public GameObject effectPrefab;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void SpawnParticle(GameObject character, string anim)
	{
		var newParticle = Instantiate(effectPrefab, character.transform.position, Quaternion.identity);
		newParticle.GetComponent<Animator>().Play(anim);
	}
}
