using Character2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public Player player;
	public PlayerInput playerInput;

	public LevelStreamManager leftLevel;
	public LevelStreamManager rightLevel;

	private bool leftLevelCompleted;
	private bool rightLevelCompleted;

	private void OnEnable()
	{
		leftLevel.OnRefreshComplete += LeftLevelFinished;
		rightLevel.OnRefreshComplete += RightLevelFinished;
	}

	private void OnDisable()
	{

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			player.SetSpawn(transform.position, this);
		}
	}

	public void RefreshLevels()
	{
		playerInput.DisableInput(false);
		playerInput.ToggleLoadingContainer(true);
		leftLevel.RefreshLevels();
		rightLevel.RefreshLevels();
	}

	private void LeftLevelFinished()
	{
		leftLevelCompleted = true;
		if (rightLevelCompleted)
		{
			RefreshFinished();
		}
	}

	private void RightLevelFinished()
	{
		rightLevelCompleted = true;
		if (leftLevelCompleted)
		{
			RefreshFinished();
		}
	}

	private void RefreshFinished()
	{
		playerInput.ToggleLoadingContainer(false);
		playerInput.EnableInput();
	}
}
