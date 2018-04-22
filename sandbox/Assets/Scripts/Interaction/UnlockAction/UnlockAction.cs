using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlockAction : MonoBehaviour
{
    protected FMOD.Studio.EventInstance unlockNoise;
    protected FMOD.Studio.EventInstance lockedNoise;

    public enum Types
    {
        None,
        ButtonsDown,
        DestroyAttackables,
        HaveKey,
        BossDefeated,
    }

    protected Openable openable;

    protected void Start()
    {
        openable = GetComponent<Openable>();

        unlockNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/unlock");
        unlockNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        lockedNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/locked");
        lockedNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
    }

    protected void UnlockOpenable()
    {
        if (openable.isLocked)
        {
            openable.Unlock();
            unlockNoise.start();
            EventDisplay.instance.AddEvent("A door or chest has unlocked somewhere.");
        }
    }

    protected abstract void CheckUnlock();
}
