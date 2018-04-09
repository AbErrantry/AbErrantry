using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
	public Platform platform;
	private WorldButton worldButton;

	public bool isDirectional;
	public bool isTowardsEnd;

	// Use this for initialization
	void Start()
	{

	}

	private void OnEnable()
	{
		worldButton = GetComponent<WorldButton>();
		worldButton.OnButtonPressed += MovePlatform;
		worldButton.OnButtonReleased += StopPlatform;
	}

	private void OnDestroy()
	{
		worldButton.OnButtonPressed -= MovePlatform;
		worldButton.OnButtonReleased -= StopPlatform;
	}

	private void MovePlatform()
	{
		platform.SetMovement(true, isDirectional, isTowardsEnd);
	}

	private void StopPlatform()
	{
		platform.SetMovement(false);
	}
}
