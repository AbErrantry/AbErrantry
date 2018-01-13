using UnityEngine;
using TMPro;

namespace Character2D
{
    public class InteractionPrefabReference : MonoBehaviour
    {
        public Interactable interactable;
        public TMP_Text interactText;

        private PlayerInteraction playerInteraction; //reference to the interaction manager

        //used for initialization
        void Start()
        {
            //TODO: fix with knight prefab
            playerInteraction = GameObject.Find("Knight").GetComponent<PlayerInteraction>();
        }

        //player clicks on an interactable in the interactable list
        //parameter: the text of the clicked interactable
        public void TriggerInteraction(TMP_Text clickedText)
        {
            //iterate through the interactable list
            for (int i = 0; i < playerInteraction.interactionTrigger.currentObjects.Count; i++)
            {
                string currentText = playerInteraction.interactionTrigger.currentObjects[i].GetComponent<Interactable>().type 
                    + " " + playerInteraction.interactionTrigger.currentObjects[i].GetComponent<Interactable>().name;
                //if the current interactable's text is equal to the clicked text, then we have found the clicked interactable 
                if (currentText == clickedText.text)
                {
                    //interact with the selected interactable
                    playerInteraction.Interact(i);
                    break;
                }
            }
        }
    }
}

