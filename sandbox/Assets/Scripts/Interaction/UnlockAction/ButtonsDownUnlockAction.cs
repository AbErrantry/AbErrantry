using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsDownUnlockAction : UnlockAction
{
	public List<WorldButton> buttons;

	//used for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.ButtonsDown;
	}

	private void OnEnable()
	{
		foreach (WorldButton button in buttons)
		{
			button.OnButtonPressed += CheckUnlock;
		}
	}

	private void OnDisable()
	{
		foreach (WorldButton button in buttons)
		{
			button.OnButtonPressed -= CheckUnlock;
		}
	}

	protected override void CheckUnlock()
	{
		foreach (WorldButton button in buttons)
		{
			if (!button.isPressed)
			{
				//cannot unlock yet
				return;
			}
		}
		UnlockOpenable(); //all buttons are pressed.
	}
}
