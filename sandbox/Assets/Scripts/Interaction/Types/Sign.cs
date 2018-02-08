using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : Interactable 
{
	public int currentDialogueState;

	//used for initialization
    private new void Start()
    {
        typeOfInteractable = Types.Sign;
        base.Start();
    }
}
