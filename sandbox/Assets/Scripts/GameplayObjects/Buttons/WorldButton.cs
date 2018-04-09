using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : Trigger<Character2D.Attackable>
{
	public event Action OnButtonPressed;
	public event Action OnButtonReleased;

	private Animator anim;

	public bool isHoldButton;
	public bool isPressed;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		isPressed = false;
	}

	protected override void TriggerAction(bool isInTrigger)
	{
		if (isInTrigger)
		{
			if (!isPressed)
			{
				isPressed = true;
				if (OnButtonPressed != null)
				{
					OnButtonPressed();
				}
			}
		}
		else if (isHoldButton)
		{
			isPressed = false;
			if (OnButtonReleased != null)
			{
				OnButtonReleased();
			}
		}
		anim.SetBool("isPressed", isPressed);
	}
}
