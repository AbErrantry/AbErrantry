using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : Openable
{
    public bool isTimed;

    private FMOD.Studio.EventInstance openNoise;

    private new void Start()
    {
        openNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/door_open");
        openNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        typeOfInteractable = Types.SideDoor;
        base.Start();

        if (isTimed)
        {
            isOpen = false;
            type = "open";
        }
        else if (isOpen)
        {
            type = "close";
        }
        anim.SetBool("isOpen", isOpen);
        base.ToggleState();
    }

    public new void ToggleState()
    {
        openNoise.start();
        if (!isOpen)
        {
            isOpen = true;
            type = "close";
            if (isTimed)
            {
                StartCoroutine(CloseDelay());
            }
        }
        else
        {
            isOpen = false;
            type = "open";
        }
        anim.SetBool("isOpen", isOpen);
        base.ToggleState();
    }

    private IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(2.5f);
        isOpen = false;
        type = "open";
        anim.SetBool("isOpen", isOpen);
        base.ToggleState();
    }
}
