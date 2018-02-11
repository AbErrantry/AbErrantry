using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public string itemName;
    private Animator anim;

    public static event Action<int, bool, bool> OnChestStateChanged;

    private new void Start()
    {
        typeOfInteractable = Types.Chest;
        base.Start();
        anim = GetComponent<Animator>();
        anim.SetBool("isOpen", false); //TODO: set isOpen based on id in save state (if open, disable chest and set to open state.)
    }

    public void OpenChest()
    {
        print("Opened: " + itemName + ".");
        anim.SetBool("isOpen", true);
        GetComponent<BoxCollider2D>().enabled = false;
        OnChestStateChanged(id, true, false);
    }
}
