using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : Interactable
{
    private Animator anim;
    public bool isOpen;

    private new void Start()
    {
        typeOfInteractable = Types.SideDoor;
        base.Start();
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    public void ToggleState()
    {
        if(!isOpen)
        {
            isOpen = true;
            type = "close";
            anim.SetBool("isOpen", true);
        }
        else
        {
            isOpen = false;
            type = "open";
            anim.SetBool("isOpen", false);
        }
    }
}
