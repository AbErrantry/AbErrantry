using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

namespace Character2D
{
    public class CharacterInteraction : MonoBehaviour
    {
        InteractionManager interactionManager; //reference to the interaction manager

        // Use this for initialization
        void Start()
        {
            interactionManager = GameObject.Find("Knight/TriggerBoxes/InteractTrigger").GetComponent<InteractionManager>();
        }

        // Update is called once per frame
        void Update()
        {
            //if the game is not paused, get input for 
            if (Time.timeScale == 1)
            {
                if (CrossPlatformInputManager.GetButtonDown("Fire2"))
                {
                    interactionManager.InteractPress();
                }
            }
        }
    }
}
