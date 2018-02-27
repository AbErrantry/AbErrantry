using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public int currentDialogueState;
    public bool changesDirection = true;

    public bool isFacingRight;

    //used for initialization
    private new void Start()
    {
        typeOfInteractable = Types.NPC;
        base.Start();

        isFacingRight = true;
        FaceRight(true);
    }

    public void FaceRight(bool faceRight)
    {
        if (changesDirection)
        {
            if (faceRight)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                isFacingRight = false;
            }
        }
    }
}
