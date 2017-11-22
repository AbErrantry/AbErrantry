using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class InteractCheck : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentInteractables;
        CharacterState characterState;
        GameObject interactBox;
        public GameObject interactButton;
        public GameObject interactList;
        public GameObject interactContainer;

        //used for initialization
        void Start()
        {
            currentInteractables = new List<GameObject>();
            characterState = GameObject.Find("Knight").GetComponent<CharacterState>();
            interactBox = GameObject.Find("Knight").GetComponent<CharacterState>().interactBox;
            interactContainer.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Interactable")
            {
                currentInteractables.Add(other.gameObject);
                DisplayText();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Interactable")
            {
                currentInteractables.Remove(other.gameObject);
                DisplayText();
            }
        }

        private void DisplayText()
        {
            if (currentInteractables.Count == 0)
            {
                interactBox.SetActive(false);
            }
            else if (currentInteractables.Count == 1)
            {
                interactBox.SetActive(true);
                characterState.interactType.text = currentInteractables[0].GetComponent<InteractableObject>().interactType;
            }
            else
            {
                interactBox.SetActive(true);
                characterState.interactType.text = "interact";
            }
        }

        public void InteractPress()
        {
            if(currentInteractables.Count == 0)
            {
                return;
            }
            else if (currentInteractables.Count == 1)
            {
                Interact(0);
            }
            else
            {
                ShowList();
            }
        }

        public void ShowList()
        {
            for(int i = 0; i < currentInteractables.Count; i++)
            {
                GameObject newButton = Instantiate(interactButton) as GameObject;
                InteractableListItem controller = newButton.GetComponent<InteractableListItem>();
                controller.interactableText.text = currentInteractables[i].GetComponent<InteractableObject>().interactText;
                newButton.transform.SetParent(interactList.transform);
                newButton.transform.localScale = Vector3.one;
            }
            interactBox.SetActive(false);
            interactContainer.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }

        public void Interact(int index)
        {
            //interact with the item at spot 0
            Debug.Log(currentInteractables[index].GetComponent<InteractableObject>().interactText);
            Destroy(currentInteractables[index]);

            CloseContainer();
        }

        public void CloseContainer()
        {
            interactContainer.SetActive(false);
            Time.timeScale = 1;

            DisplayText();

            //delete ui elements
            var children = new List<GameObject>();
            foreach (Transform child in interactList.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));
        }
    }
}
