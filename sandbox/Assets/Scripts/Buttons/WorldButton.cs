using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : Trigger
{
	public event Action OnButtonPressed;

	private Animator anim;

	public bool isHoldButton;
	public bool isPressed;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		isPressed = false;
		objectTag = "World";
		layerTag = "Player";
	}

	protected override void TriggerAction(bool isInTrigger)
	{
		if (isInTrigger)
		{
			if (!isPressed)
			{
				isPressed = true;
				OnButtonPressed();
			}
		}
		else if (isHoldButton)
		{
			isPressed = false;
		}
		anim.SetBool("isPressed", isPressed);
	}
}
