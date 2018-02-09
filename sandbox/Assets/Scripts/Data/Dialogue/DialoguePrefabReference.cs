using UnityEngine;
using TMPro;

namespace Dialogue2D
{
	public class DialoguePrefabReference : MonoBehaviour
	{
		public int choiceNext;
		public TMP_Text choiceText;

		private DialogueManager dialogueManager;

		//used for initialization
        void Start()
        {
            //TODO: fix with knight prefab
            dialogueManager = GameObject.Find("Knight").GetComponent<DialogueManager>();
        }

		//triggers a choice in a dialogue
        public void TriggerChoice()
        {
            dialogueManager.SubmitChoice(choiceNext);
        }
	}
}
