using System;
using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public abstract class Boss : Attackable
{
	public static event Action<string> OnBossDefeated;

	protected new string name;
	public Transform player;
	public bool isFacingRight;

	// Use this for initialization
	protected new void Start()
	{
		player = GameObject.Find("Knight").GetComponent<Transform>();
		base.Start();
		bool bossDefeated = GameData.data.saveData.ReadBossState(name);
		if (!bossDefeated)
		{
			SpawnBoss();
		}

	}

	// Update is called once per frame
	protected void Update()
	{
		if (player.position.x >= transform.position.x)
		{
			isFacingRight = true;
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		else
		{
			isFacingRight = false;
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}

	protected void SpawnBoss()
	{
		anim.Play("Spawn");
	}

	protected void BossDefeated()
	{
		if (!Player.instance.isDying)
		{
			OnBossDefeated(name);
			EventDisplay.instance.AddEvent("Defeated " + name + " boss.");
		}
		Destroy(gameObject);
	}

}
