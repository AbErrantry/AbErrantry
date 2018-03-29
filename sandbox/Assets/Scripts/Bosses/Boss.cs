using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
	protected Animator anim;
	protected new string name;
	protected int health;

	// Use this for initialization
	protected void Start()
	{
		anim = GetComponent<Animator>();
		// TODO: if the boss is not yet defeated, spawn them.
	}

	// Update is called once per frame
	protected void Update()
	{

	}

	private void SpawnBoss()
	{

	}

	private void BossDefeated()
	{
		if (!Player.instance.isDying)
		{
			// TODO: write the boss defeated if the player is still alive at this point.
			//
		}
	}
}
