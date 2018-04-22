﻿using System;
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
		canFlinch = true;
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

	protected override void Flinch()
	{
		anim.Play("Hurt");

	}

	public IEnumerator ColorHit()
	{
		canTakeDamage = false;
		Color baseColor = gameObject.GetComponent<Renderer>().material.color;
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp(baseColor, Color.red, Mathf.PingPong(Time.time, 1));
		yield return new WaitForSeconds(2);
		gameObject.GetComponent<Renderer>().material.color = baseColor;
		canTakeDamage = true;
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
