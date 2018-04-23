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
		buttonNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
	}

	protected override void TriggerAction(bool isInTrigger)
	{
		if (isInTrigger)
		{
			if (!isPressed)
			{
				buttonNoise.start();
				buttonNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));

				isPressed = true;
				if (OnButtonPressed != null)
				{
					OnButtonPressed();
				}
			}
		}
		else if (isHoldButton)
		{
			buttonNoise.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GetComponent<Transform>(), GetComponent<Rigidbody>()));
			buttonNoise.start();

			isPressed = false;
			if (OnButtonReleased != null)
			{
				OnButtonReleased();
			}
		}

		if (GetComponent<Animator>() != null)
		{
			anim.SetBool("isPressed", isPressed);
		}
	}
}
