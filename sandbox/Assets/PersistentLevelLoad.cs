using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
	public class PersistentLevelLoad : MonoBehaviour
	{
		void Start()
		{
			Player.instance.InitialLoad();
		}

		void OnDestroy()
		{
			SpawnManager.managerDictionary.Clear();
			AssetBundle.UnloadAllAssetBundles(true);
			LevelStreamManager.InitializeActiveScenes();
		}
	}
}
