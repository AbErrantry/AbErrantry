using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCanMove : Trigger
{
    public BoxMove boxMove;

    private void Start()
    {
        objectTag = "None";
        layerTag = "Attackable";
    }

    //fires upon an object entering/exiting the trigger box
    protected override void TriggerAction(bool isInTrigger)
    {
        if (isInTrigger)
        {
            boxMove.SetCanMove(false);
        }
        else
        {
            boxMove.SetCanMove(true);
        }
    }
}
