using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class DestroyAttackablesUnlockAction : UnlockAction
{
	public List<Attackable> attackables;

	//used for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.DestroyAttackables;
	}

	private void OnEnable()
	{
		foreach (Attackable attackable in attackables)
		{
			attackable.OnAttackableDestroyed += RemoveAttackable;
		}
	}

	private void OnDisable()
	{
		foreach (Attackable attackable in attackables)
		{
			attackable.OnAttackableDestroyed -= RemoveAttackable;
		}
	}

	protected override void CheckUnlock()
	{
		if (attackables.Count <= 0)
		{
			UnlockOpenable(); //all attackables have been destroyed.
		}
	}

	protected void RemoveAttackable(Attackable destroyedAttackable)
	{
		destroyedAttackable.OnAttackableDestroyed -= RemoveAttackable;
		attackables.Remove(destroyedAttackable);
		CheckUnlock();
	}
}
