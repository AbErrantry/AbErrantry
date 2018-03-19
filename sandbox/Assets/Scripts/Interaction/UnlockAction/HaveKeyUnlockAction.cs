using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class HaveKeyUnlockAction : UnlockAction
{
	public string keyName;

	//used for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.HaveKey;
	}

	protected override void CheckUnlock()
	{
		if (Player.instance.gameObject.GetComponent<PlayerInventory>().CheckForItemAndRemove(keyName, 1))
		{
			UnlockOpenable(); //the key has been used.
			EventDisplay.instance.AddEvent("Used the " + keyName + ".");
		}
		else
		{
			EventDisplay.instance.AddEvent("Locked.");
		}
	}

	public void TryKey()
	{
		CheckUnlock();
	}
}
