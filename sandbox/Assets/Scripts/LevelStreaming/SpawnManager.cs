using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static Dictionary<string, SpawnManager> managerDictionary;

	public string managerName;
	public string managerDisplayName;

	public LevelStreamManager leftLevel;
	public LevelStreamManager rightLevel;
	public LevelStreamManager persistentLevel;

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

	private void Start()
	{
		if (managerDictionary == null)
		{
			managerDictionary = new Dictionary<string, SpawnManager>();
		}
		managerName = leftLevel.sceneName + "|" + rightLevel.sceneName;
		managerDictionary.Add(managerName, this);
	}

	public static SpawnManager SetSpawnManager(string name)
	{
		return managerDictionary[name];
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
		leftLevelCompleted = false;
		rightLevelCompleted = false;
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
