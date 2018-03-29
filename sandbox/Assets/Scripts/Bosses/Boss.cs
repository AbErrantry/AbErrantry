﻿using System;
using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public abstract class Boss : Attackable
{
	public static event Action<string> OnBossDefeated;

	protected new string name;

	// Use this for initialization
	protected new void Start()
	{
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
	}

}
