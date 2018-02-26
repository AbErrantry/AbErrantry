using System;
using System.Collections;
using UnityEngine;

public class BackDoor : Openable
{
    public GameObject doorPair;
    private float openTime;

    private new void Start()
    {
        typeOfInteractable = Types.BackDoor;
        base.Start();
        openTime = 0.25f;
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
        isOpen = true;
        anim.SetBool("isOpen", isOpen);
        yield return new WaitForSeconds(openTime);
        isOpen = false;
        anim.SetBool("isOpen", isOpen);

        if (isFirst)
        {
            character.GetComponent<Character2D.Player>().ToggleCamera(false);
            character.transform.position = doorPair.transform.position;
            character.GetComponent<Character2D.Player>().ToggleCamera(true);
            doorPair.GetComponent<BackDoor>().StartEnterDoorRoutine(character, false);
            yield return null;
        }
        else
        {
            isLocked = false; //checkpoint unlocked TODO: move to checkpoint class
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
