using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class PersistentLevelLoad : MonoBehaviour
	{
		public bool isDebug;
		public LevelStreamManager persistentLevel;

		void Start()
		{
			if (!isDebug)
			{
				Player.instance.InitialLoad();
				persistentLevel.LoadPersistentData();
			}
		}

		void OnDestroy()
		{
			SpawnManager.managerDictionary.Clear();
			LevelStreamManager.InitializeActiveScenes();
		}
	}
}
