using UnityEngine;
using TMPro;

namespace Character2D
{
    public class InteractionFire : MonoBehaviour
    {
        private CharacterInteraction characterInteraction; //reference to the interaction manager

        //used for initialization
        void Start()
        {
            //TODO: fix with knight prefab
            characterInteraction = GameObject.Find("Knight").GetComponent<CharacterInteraction>();
        }

        //player clicks on an interactable in the interactable list
        //parameter: the text of the clicked interactable
        public void TriggerInteraction(TMP_Text clickedText)
        {
            //iterate through the interactable list
            for(int i = 0; i < characterInteraction.interactionTrigger.currentObjects.Count; i++)
            {
                //if the current interactable's text is equal to the clicked text, then we have found the clicked interactable 
                if(characterInteraction.interactionTrigger.currentObjects[i].GetComponent<InteractableObject>().interactText == clickedText.text)
                {
                    //interact with the selected interactable
                    characterInteraction.Interact(i);
                    break;
                }
            }
        }

        //player clicks the escape button in the interactable list
        public void Escape()
        {
            //since the player did not choose to interact with anything, we just close the container
            characterInteraction.CloseContainer();
        }
    }
}
