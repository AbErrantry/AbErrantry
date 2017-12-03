using UnityEngine;

namespace Character2D
{
    public class CharacterInteraction : MonoBehaviour
    {
        public InteractionManager interactionManager; //reference to the interaction manager
        public bool interactionInput; //whether the character is trying to interact or not

        void Start()
        {
            interactionInput = false;
        }

        // Update is called once per frame
        void Update()
        {
            //if the character inputs for an interaction
            if (interactionInput)
            {
                interactionManager.InteractPress();
                interactionInput = false;
            }
        }
    }
}
