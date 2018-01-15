using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public int currentDialogueState;

    //used for initialization
    private new void Start()
    {
        base.Start();
        currentDialogueState = 1;
    }
}
