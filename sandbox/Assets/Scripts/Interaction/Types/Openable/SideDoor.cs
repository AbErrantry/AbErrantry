using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : Openable
{
    public bool isTimed;

    private new void Start()
    {
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
