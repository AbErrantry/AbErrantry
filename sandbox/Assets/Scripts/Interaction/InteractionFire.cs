using UnityEngine;

namespace Character2D
{
    public class InteractionFire : MonoBehaviour
    {
        public CharacterInteraction characterInteraction; //reference to the interaction manager

        //player clicks the escape button in the interactable list
        public void Escape()
        {
            //since the player did not choose to interact with anything, we just close the container
            characterInteraction.CloseContainer();
        }

        //player clicks the collect all button in the interactable list
        public void CollectAll()
        {
            //since the player did not choose to interact with anything, we just close the container
            characterInteraction.CollectAllItems();
        }
    }
}
