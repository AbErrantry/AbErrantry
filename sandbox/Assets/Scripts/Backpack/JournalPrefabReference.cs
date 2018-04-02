using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
	public class JournalPrefabReference : MonoBehaviour
	{
		public QuestInstance quest;
		public TMP_Text questName;
		public TMP_Text questText;
		public TMP_Text questHint;
		public TMP_Text questStep;

		private BackpackMenu backpackMenu;

		// Use this for initialization
		private void Start()
		{
			backpackMenu = Player.instance.GetComponent<BackpackMenu>();
		}

		public void SelectQuest()
		{
			backpackMenu.SelectQuest(quest);
		}
	}
}
