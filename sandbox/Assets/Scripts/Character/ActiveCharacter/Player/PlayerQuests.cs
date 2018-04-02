using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue2D;
using UnityEngine;

namespace Character2D
{
	public class PlayerQuests : MonoBehaviour
	{
		private Player player;

		public List<QuestInstance> quests;
		public static event Action<string, int> OnQuestUpdate;

		// Use this for initialization
		private void Start()
		{
			player = GetComponent<Player>();
			quests = new List<QuestInstance>();
			foreach (Quest quest in GameData.data.questData.questDictionary.Values)
			{
				var instance = new QuestInstance();
				instance.quest = quest;
				instance.step = GameData.data.saveData.GetQuestStep(quest.name);
				quests.Add(instance);
			}
		}

		public void UpdateQuestStep(string name, int step)
		{
			foreach (QuestInstance quest in quests)
			{
				if (name == quest.quest.name)
				{
					if (quest.step >= 0)
					{
						quest.step = step;
						if (step < 0)
						{
							EventDisplay.instance.AddEvent("Quest " + NameConversion.ConvertSymbol(quest.quest.name) + " was completed.");
						}
						else
						{
							EventDisplay.instance.AddEvent("Quest " + NameConversion.ConvertSymbol(quest.quest.name) + " was updated.");
						}
						OnQuestUpdate(name, step);
						player.SetQuest(name, true);
						return;
					}
				}
			}
		}

		public QuestInstance GetQuestInfo(string name)
		{
			foreach (QuestInstance quest in quests)
			{
				if (name == quest.quest.name)
				{
					return quest;
				}
			}
			Debug.LogError("Error. No quest with name (" + name + ") can be found.");
			return null;
		}

		public string GetQuestString(string name)
		{
			foreach (QuestInstance quest in quests)
			{
				if (name == quest.quest.name)
				{
					return NameConversion.ConvertSymbol(name) + ": " + NameConversion.ConvertSymbol(quest.quest.segments[quest.step].text);
				}
			}
			Debug.LogError("Error. No quest with name (" + name + ") can be found.");
			return null;
		}

		public bool QuestIsActive(string name)
		{
			foreach (QuestInstance quest in quests)
			{
				if (name == quest.quest.name)
				{
					if (quest.step <= 0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			Debug.LogError("Error. No quest with name (" + name + ") can be found.");
			return false;
		}
	}
}
