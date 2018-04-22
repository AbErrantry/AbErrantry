using System;
using System.Collections;
using UnityEngine;

public class BackDoor : Openable
{
    public GameObject doorPair;
    private float openTime;

    private FMOD.Studio.EventInstance openNoise;

    private new void Start()
    {
        openNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/door_open");
        openNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        typeOfInteractable = Types.BackDoor;
        base.Start();
        openTime = 0.25f;

        if (isOpen)
        {
            isOpen = false;
            anim.SetBool("isOpen", isOpen);
            ToggleState();
        }
    }

    public new void ToggleState()
    {
        //toggle on unlock
        base.ToggleState();
    }

    public void StartEnterDoorRoutine(GameObject character, bool isFirst)
    {
        StartCoroutine(EnterDoor(character, isFirst));
    }

    private IEnumerator EnterDoor(GameObject character, bool isFirst)
    {
        openNoise.start();

        isOpen = true;
        anim.SetBool("isOpen", isOpen);
        yield return new WaitForSeconds(openTime);
        isOpen = false;
        anim.SetBool("isOpen", isOpen);

        if (isFirst)
        {
            character.GetComponent<Character2D.Player>().ToggleCamera(false);
            character.transform.position = doorPair.transform.position;
            doorPair.GetComponent<BackDoor>().StartEnterDoorRoutine(character, false);
            yield return new WaitForSeconds(0.1f);
            character.GetComponent<Character2D.Player>().ToggleCamera(true);
            yield return null;
        }
        else
        {
            isLocked = false; //checkpoint unlocked. Pseudo-UnlockAction. Unlocks door pair.
            anim.SetBool("isLocked", isLocked);
            ToggleState();
        }
    }

    public void LockDoor()
    {
        isLocked = true;
        anim.SetBool("isLocked", isLocked);
        ToggleState();
    }
}
