using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectsUnlockAction : UnlockAction
{

	//used for initialization
	private new void Start()
	{
		base.Start();
		GetComponent<Openable>().unlockActionType = Types.DestroyGameObjects;
	}

	protected override void CheckUnlock()
	{
		//if all buttons in
	}
}
