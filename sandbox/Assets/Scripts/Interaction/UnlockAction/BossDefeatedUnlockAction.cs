using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedUnlockAction : UnlockAction
{
	public string bossName;

	// Use this for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.BossDefeated;
	}

	private void OnEnable()
	{
		Boss.OnBossDefeated += TryUnlock;
	}

	private void OnDisable()
	{
		Boss.OnBossDefeated -= TryUnlock;
	}

	protected override void CheckUnlock()
	{
		UnlockOpenable(); // the selected boss has been defeated.
	}

	private void TryUnlock(string name)
	{
		if (name == bossName)
		{
			CheckUnlock();
		}
	}
}
