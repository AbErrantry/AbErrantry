using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
	public class MapPrefabReference : MonoBehaviour
	{
		public string locationName;
		public TMP_Text locationText;

		private BackpackMenu backpackMenu;

		// Use this for initialization
		private void Start()
		{
			backpackMenu = Player.instance.GetComponent<BackpackMenu>();
		}

		public void SelectLocation()
		{
			backpackMenu.SelectLocation(locationName);
		}
	}
}
