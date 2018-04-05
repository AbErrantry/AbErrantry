using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
	public class MapPrefabReference : MonoBehaviour
	{
		public SpawnManager spawn;

		public TMP_Text locationText;

		private BackpackMenu backpackMenu;

		// Use this for initialization
		private void Start()
		{
			backpackMenu = Player.instance.GetComponent<BackpackMenu>();
		}

		public void SelectLocation()
		{
			backpackMenu.SelectLocation(spawn);
		}
	}
}
