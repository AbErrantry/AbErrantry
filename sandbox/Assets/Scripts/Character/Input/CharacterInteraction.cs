using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

namespace Character2D
{
    public class CharacterInteraction : MonoBehaviour
    {
        public InteractionManager interactionManager; //reference to the interaction manager
        public bool interactionInput;

        void Start()
        {
            interactionInput = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (interactionInput)
            {
                interactionManager.InteractPress();
                interactionInput = false;
            }
        }
    }
}
