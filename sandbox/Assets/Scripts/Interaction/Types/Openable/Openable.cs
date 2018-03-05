using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : Interactable
{
	public UnlockAction.Types unlockActionType;

	protected Animator anim;

	public static event Action<int, OpenableTuple> OnOpenableStateChanged;
	public event Action OnDoorOpened;

	public int id;
	public bool isOpen;
	public bool isLocked;

	// Use this for initialization
	protected new void Start()
	{
		base.Start();
		anim = GetComponent<Animator>();

		OpenableTuple tuple = GameData.data.saveData.ReadOpenableState(id, gameObject.name);
		isOpen = tuple.isOpen;
		isLocked = tuple.isLocked;

		anim.SetBool("isOpen", isOpen);
		anim.SetBool("isLocked", isLocked);
	}

	protected void ToggleState()
	{
		OpenableTuple tuple = new OpenableTuple();
		tuple.isOpen = isOpen;
		tuple.isLocked = isLocked;
		OnOpenableStateChanged(id, tuple);
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
