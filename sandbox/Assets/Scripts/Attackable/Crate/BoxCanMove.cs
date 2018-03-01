using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCanMove : Trigger<Standable>
{
    public BoxMove boxMove;

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
