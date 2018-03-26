using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderBox : Trigger<Character2D.Player>
{
    public Character2D.Crate crate;

    //fires upon an object entering/exiting the trigger box
    protected override void TriggerAction(bool isInTrigger)
    {
        if (isInTrigger)
        {
            crate.DestroyCrate();
        }
    }
}
