using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Character2D
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> currentInteractables; //list of current interactable gameobjects
        public GameObject interactBar; //reference to the interact popup bar that asks for input to interact
        public GameObject interactButton; //reference to the interact button prefab
        public GameObject interactList; //reference to the interact list which contains interact button prefabs
        public GameObject interactContainer; //reference to the interact container which contains the interact list

        public TMP_Text interactBarType; //reference to the type of interaction
        public TMP_Text interactBarKey; //reference to the key pressed for the interaction

        private float interactListYPos; //the default y position of the interact list (to scroll back to the top)

        //used for initialization
        void Start()
        {
            currentInteractables = new List<GameObject>();
            interactListYPos = interactList.transform.position.y;
            interactBar.SetActive(false);
            interactContainer.SetActive(false);
            //TODO: set default interactBarKey
        }

        //detects when a new interactable has entered the interaction trigger box
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Interactable")
            {
                //adds the interactable to the list of current interactables
                currentInteractables.Add(other.gameObject);
                DisplayText();
            }
        }

        //detects when an interactable has left the interaction trigger box
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Interactable")
            {
                //removes the interactable from the list of current interactables
                currentInteractables.Remove(other.gameObject);
                DisplayText();
            }
        }

        //displays the interaction popup depending on the current item count
        private void DisplayText()
        {
            //if there are no current interactables, don't display the popup
            if (currentInteractables.Count == 0)
            {
                interactBar.SetActive(false);
            }
            //if there is one interactable, display its unique popup
            else if (currentInteractables.Count == 1)
            {
                interactBar.SetActive(true);
                interactBarType.text = currentInteractables[0].GetComponent<InteractableObject>().interactType;
            }
            //if there exists more than one interactable, display a generic popup
            else
            {
                interactBar.SetActive(true);
                interactBarType.text = "interact";
            }
        }

        //handles a press of the interact button given the amount of current interactables
        public void InteractPress()
        {
            //if there are no interactables, do nothing
            if(currentInteractables.Count == 0)
            {
                return;
            }
            //if there is one interactable, interact with it
            else if (currentInteractables.Count == 1)
            {
                Interact(0);
            }
            //if there exists more than one interactable, show the interact container
            else
            {
                ShowList();
            }
        }

        //shows the list of interactables that the player can select to interact with
        public void ShowList()
        {
            //iterate through the list of interactables spawning buttons on screen in a list
            for(int i = 0; i < currentInteractables.Count; i++)
            {
                //instantiate a prefab for the interact button
                GameObject newButton = Instantiate(interactButton) as GameObject;
                InteractableObject controller = newButton.GetComponent<InteractableObject>();

                //set the text for the interactable onscreen 
                controller.interactableText.text = currentInteractables[i].GetComponent<InteractableObject>().interactText;

                //put the interactable in the list
                newButton.transform.SetParent(interactList.transform);

                //for some reason Unity does not use full scale for the instantiated object by default
                newButton.transform.localScale = Vector3.one;
            }
            interactBar.SetActive(false);
            interactContainer.SetActive(true);

            //move the scrollbar back to the top of the list
            interactList.transform.position = new Vector3(interactList.transform.position.x, interactListYPos, 0);

            //pause game time (in part to prevent user input)
            //TODO: decide if we want to keep time paused here or just disable user action/motion input
            Time.timeScale = 0;
            Cursor.visible = true;
        }

        //performs the interaction with the selected item
        public void Interact(int index)
        {
            //interact with the selected item
            Debug.Log(currentInteractables[index].GetComponent<InteractableObject>().interactText);

            //destroy the item, removing it from the game and list
            Destroy(currentInteractables[index]);
            CloseContainer();
        }

        //cleans up the screen after an interactable is chosen
        public void CloseContainer()
        {
            //unpause time and hide the container
            //TODO: if changed above, change here too
            interactContainer.SetActive(false);
            Time.timeScale = 1;

            //display the interact popup
            DisplayText();

            //delete ui elements from the list for the next iteration
            var children = new List<GameObject>();
            foreach (Transform child in interactList.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));
        }

        //changes the interact key for display
        //TODO: implement
        public void interactKeyChange(string newKey)
        {
            interactBarKey.text = newKey;
        }
    }
}
