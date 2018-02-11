using TMPro;
using UnityEngine;

namespace Character2D
{
    public class InteractionPrefabReference : MonoBehaviour
    {
        public TMP_Text interactText;
        public int indexInList;
        private PlayerInteraction playerInteraction; //reference to the interaction manager

        //used for initialization
        void Start()
        {
            //TODO: fix with knight prefab
            playerInteraction = GameObject.Find("Knight").GetComponent<PlayerInteraction>();
        }

        //player clicks on an interactable in the interactable list
        public void TriggerInteraction()
        {
            playerInteraction.Interact(indexInList);
        }
    }
}
