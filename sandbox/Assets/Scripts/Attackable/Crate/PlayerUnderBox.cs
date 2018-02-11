using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderBox : Trigger
{
    public BoxMove boxMove;

    private void Start()
    {
        objectTag = "Player";
        layerTag = "None";
    }

    //fires upon an object entering/exiting the trigger box
    protected override void TriggerAction(bool isInTrigger)
    {
        if (isInTrigger)
        {
            boxMove.SetPlayerUnder(true);
        }
        else
        {
            boxMove.SetPlayerUnder(false);
        }
    }
}
