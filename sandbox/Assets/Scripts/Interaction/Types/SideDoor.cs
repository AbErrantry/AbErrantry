using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : Interactable
{
    private Animator anim;
    public bool isOpen;

    public static event Action<int, bool, bool> OnSideDoorStateChanged;

    private new void Start()
    {
        typeOfInteractable = Types.SideDoor;
        base.Start();
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    public void ToggleState()
    {
        if (!isOpen)
        {
            isOpen = true;
            type = "close";
            anim.SetBool("isOpen", true);
            OnSideDoorStateChanged(id, isOpen, false);
        }
        else
        {
            isOpen = false;
            type = "open";
            anim.SetBool("isOpen", false);
            OnSideDoorStateChanged(id, isOpen, false);
        }
    }
}
