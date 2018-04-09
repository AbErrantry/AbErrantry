using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class BearMovement : CharacterMovement
	{
		// Use this for initialization
		private new void Start()
		{
			base.Start();
			mvmtSpeed = 1.0f;
		}
	}
}
