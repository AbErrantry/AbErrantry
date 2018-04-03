using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithBoss : Boss 
{

	protected new void Start()
	{
		name = "Wraith";
		base.Start();
		canTakeDamage = true;
	}
	
	protected new void Update()
	{

	}
}
