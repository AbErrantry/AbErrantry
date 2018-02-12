using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointUnlockAction : UnlockAction
{

	//used for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.CheckPoint;
	}

	protected override void CheckUnlock()
	{
		//if all buttons in
	}
}
