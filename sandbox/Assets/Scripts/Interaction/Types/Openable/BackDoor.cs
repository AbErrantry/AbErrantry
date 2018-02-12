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

    public IEnumerator EnterDoor(GameObject character, bool isFirst)
    {
        float startTime;
        startTime = Time.time;
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
            yield return StartCoroutine(doorPair.GetComponent<BackDoor>().EnterDoor(character, false));
        }
        else
        {
            isLocked = false; //checkpoint unlocked
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
