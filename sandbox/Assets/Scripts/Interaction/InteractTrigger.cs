using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character2D
{
    public class InteractTrigger : MonoBehaviour
    {
        InteractCheck interactCheck;

        // Use this for initialization
        void Start()
        {
            interactCheck = GameObject.Find("Knight/TriggerBoxes/InteractTrigger").GetComponent<InteractCheck>();
        }

        public void TriggerInteraction(TMP_Text clickedText)
        {
            for(int i = 0; i < interactCheck.currentInteractables.Count; i++)
            {
                if(interactCheck.currentInteractables[i].GetComponent<InteractableObject>().interactText == clickedText.text)
                {
                    interactCheck.Interact(i);
                    break;
                }
            }
        }

        public void Escape()
        {
            interactCheck.CloseContainer();
        }
    }
}
