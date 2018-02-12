using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : Openable
{

    private new void Start()
    {
        typeOfInteractable = Types.SideDoor;
        base.Start();
    }

    public new void ToggleState()
    {
        if (!isOpen)
        {
            isOpen = true;
            type = "close";
        }
        else
        {
            isOpen = false;
            type = "open";
        }
        anim.SetBool("isOpen", isOpen);
        base.ToggleState();
    }
}
