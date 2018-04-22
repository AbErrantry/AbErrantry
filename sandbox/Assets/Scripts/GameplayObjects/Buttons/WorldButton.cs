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

	private FMOD.Studio.EventInstance buttonNoise;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		isPressed = false;

		buttonNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/button_click");
		buttonNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

		FMODUnity.RuntimeManager.AttachInstanceToGameObject(buttonNoise, GetComponent<Transform>(), GetComponent<Rigidbody>());
	}

	protected override void TriggerAction(bool isInTrigger)
	{
		if (isInTrigger)
		{
			if (!isPressed)
			{
				buttonNoise.start();
				isPressed = true;
				if (OnButtonPressed != null)
				{
					OnButtonPressed();
				}
			}
		}
		else if (isHoldButton)
		{
			buttonNoise.start();
			isPressed = false;
			if (OnButtonReleased != null)
			{
				OnButtonReleased();
			}
		}
		anim.SetBool("isPressed", isPressed);
	}
}
