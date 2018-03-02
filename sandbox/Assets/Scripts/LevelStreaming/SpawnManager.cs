using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
		leftLevel.OnRefreshComplete -= LeftLevelFinished;
		rightLevel.OnRefreshComplete -= RightLevelFinished;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Player.instance.SetSpawn(transform.position, this);
		}
	}

	public void RefreshLevels()
	{
		leftLevel.RefreshLevel();
		rightLevel.RefreshLevel();
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
		PlayerInput.instance.EnableSpawn();
	}
}
