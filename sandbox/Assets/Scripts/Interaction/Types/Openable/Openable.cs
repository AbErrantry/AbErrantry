using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : Interactable
{
	public UnlockAction.Types unlockActionType;

	protected Animator anim;

	public static event Action<int, bool, bool> OnOpenableStateChanged;

	public int id;
	public bool isOpen;
	public bool isLocked;

	// Use this for initialization
	protected new void Start()
	{
		base.Start();
		anim = GetComponent<Animator>();

		bool[] temp = GameData.data.saveData.ReadOpenableState(id, gameObject.name);
		isOpen = temp[0];
		isLocked = temp[1];

		anim.SetBool("isOpen", isOpen);
		anim.SetBool("isLocked", isLocked);
	}

	protected void ToggleState()
	{
		OnOpenableStateChanged(id, isOpen, isLocked);
	}

	public void Unlock()
	{
		isLocked = false;
		Debug.Log("Unlocked");
		anim.SetBool("isLocked", isLocked);
		ToggleState();
	}

	public void TryUnlock()
	{
		if (unlockActionType == UnlockAction.Types.HaveKey)
		{

		}
		else
		{
			Debug.Log("Locked.");
		}
	}
}
