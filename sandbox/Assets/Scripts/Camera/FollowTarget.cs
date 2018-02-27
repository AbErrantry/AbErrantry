using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;

	private void FixedUpdate()
	{
		transform.position = target.position;
	}

	public void SetTarget(Transform targetToSet)
	{
		target = targetToSet;
	}
}
