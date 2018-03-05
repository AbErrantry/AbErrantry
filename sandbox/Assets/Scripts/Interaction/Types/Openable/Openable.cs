using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : Interactable
{
	public UnlockAction.Types unlockActionType;

	protected Animator anim;

	public static event Action<int, bool, bool> OnOpenableStateChanged;
	public event Action OnDoorOpened;

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
		if (isOpen)
		{
			if (OnDoorOpened != null)
			{
				OnDoorOpened();
			}
		}
	}

	public void Unlock()
	{
		if (isLocked)
		{
			isLocked = false;
			Debug.Log("Unlocked");
			anim.SetBool("isLocked", isLocked);
			ToggleState();
		}
	}

	public void TryUnlock()
	{
		if (unlockActionType == UnlockAction.Types.HaveKey)
		{
			GetComponent<HaveKeyUnlockAction>().TryKey();
		}
		else
		{
			Debug.Log("Locked."); //TODO: replace with locked eventSystem
		}
	}
}
