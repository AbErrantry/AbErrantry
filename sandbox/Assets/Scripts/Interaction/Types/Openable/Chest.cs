using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Openable
{
    public string itemName;

    private FMOD.Studio.EventInstance openNoise;

    private new void Start()
    {
        openNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/chest_open");
        openNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        typeOfInteractable = Types.Chest;
        base.Start();
        if (isOpen)
        {
            DisableHitbox();
        }
    }

    public new void ToggleState()
    {
        openNoise.start();

        isOpen = true;
        anim.SetBool("isOpen", isOpen);
        DisableHitbox();

        base.ToggleState();
    }

    private void DisableHitbox()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
