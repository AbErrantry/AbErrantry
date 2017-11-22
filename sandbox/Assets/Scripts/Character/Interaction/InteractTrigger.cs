using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character2D
{
    public class InteractTrigger : MonoBehaviour
    {
        InteractionManager interactionManager; //reference to the interaction manager

        //used for initialization
        void Start()
        {
            interactionManager = GameObject.Find("Knight/TriggerBoxes/InteractTrigger").GetComponent<InteractionManager>();
        }

        //player clicks on an interactable in the interactable list
        //parameter: the text of the clicked interactable
        public void TriggerInteraction(TMP_Text clickedText)
        {
            //iterate through the interactable list
            for(int i = 0; i < interactionManager.currentInteractables.Count; i++)
            {
                //if the current interactable's text is equal to the clicked text, then we have found the clicked interactable 
                if(interactionManager.currentInteractables[i].GetComponent<InteractableObject>().interactText == clickedText.text)
                {
                    //interact with the selected interactable
                    interactionManager.Interact(i);
                    break;
                }
            }
        }

        //player clicks the escape button in the interactable list
        public void Escape()
        {
            //since the player did not choose to interact with anything, we just close the container
            interactionManager.CloseContainer();
        }
    }
}
