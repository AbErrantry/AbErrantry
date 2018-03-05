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
		if (Player.instance.gameObject.GetComponent<PlayerInventory>().CheckForItem(keyName, 1))
		{
			UnlockOpenable(); //the key has been used.
			//TODO: add event for unlocked <openable> with keyName
		}
		else
		{
			Debug.Log("Locked."); //TODO: replace with locked eventSystem
		}
	}

	public void TryKey()
	{
		CheckUnlock();
	}
}
