using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Openable
{
    public string itemName;

    private new void Start()
    {
        typeOfInteractable = Types.Chest;
        base.Start();
        if (isOpen)
        {
            DisableHitbox();
        }
    }

    public new void ToggleState()
    {
        print("Opened: " + itemName + ".");
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
