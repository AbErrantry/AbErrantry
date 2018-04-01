using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class PersistentLevelLoad : MonoBehaviour
	{
		public bool isDebug;

		void Start()
		{
			if (!isDebug)
			{
				Player.instance.InitialLoad();
			}
		}

		void OnDestroy()
		{
			SpawnManager.managerDictionary.Clear();
			LevelStreamManager.InitializeActiveScenes();
		}
	}
}
