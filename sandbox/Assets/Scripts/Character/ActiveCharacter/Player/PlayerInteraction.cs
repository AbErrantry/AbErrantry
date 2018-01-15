using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

namespace Character2D
{
    public class PlayerInteraction : MonoBehaviour
    {
        public InteractionTrigger interactionTrigger;

        private PlayerInput playerInput;
        private Dialogue2D.DialogueManager dialogueManager;

        public Character character;

        public GameObject interactBar; //reference to the interact popup bar that asks for input to interact
        public GameObject interactButton; //reference to the interact button prefab
        public GameObject interactList; //reference to the interact list which contains interact button prefabs
        public GameObject interactContainer; //reference to the interact container which contains the interact list

        public TMP_Text interactBarText; //the text displayed on the interact bar

        public Button collectAllButton;

        private string interactKey; //the key for interaction

        public ScrollRect scrollRect; //the default y position of the interact list (to scroll back to the top)

        public bool interactionInput; //whether the character is trying to interact or not

        public bool isInteracting;
        public float interactTime;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            dialogueManager = GetComponent<Dialogue2D.DialogueManager>();

            isInteracting = false; 

            interactionInput = false;

            interactBar.SetActive(false);
            interactContainer.SetActive(false);

            //TODO: set default interactBarKey
            interactKey = "Q";

            interactTime = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            //if the character inputs for an interaction
            if (interactionInput)
            {
                InteractPress();
                interactionInput = false;
            }
        }

        //displays the interaction popup depending on the current item count
        public void DisplayText()
        {
            //if there are no current interactables, don't display the popup
            if (interactionTrigger.currentObjects.Count == 0)
            {
                interactBar.SetActive(false);
            }
            //if there is one interactable, display its unique popup
            else if (interactionTrigger.currentObjects.Count == 1)
            {
                interactBar.SetActive(true);
                SetInteractBarText(interactionTrigger.currentObjects[0].GetComponent<Interactable>().type, 
                    interactionTrigger.currentObjects[0].GetComponent<Interactable>().name, false);
            }
            //if there exists more than one interactable, display a generic popup
            else
            {
                interactBar.SetActive(true);
                SetInteractBarText("choose interaction", "", false);
            }
        }

        //handles a press of the interact button given the amount of current interactables
        public void InteractPress()
        {
            //if there are no interactables, do nothing
            if (interactionTrigger.currentObjects.Count == 0)
            {
                return;
            }
            //if there is one interactable, interact with it
            else if (interactionTrigger.currentObjects.Count == 1)
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
            playerInput.DisableInput(true);

            int numberOfItems = 0;
            //iterate through the list of interactables spawning buttons on screen in a list
            for (int i = 0; i < interactionTrigger.currentObjects.Count; i++)
            {
                //instantiate a prefab for the interact button
                GameObject newButton = Instantiate(interactButton) as GameObject;
                InteractionPrefabReference temp = newButton.GetComponent<InteractionPrefabReference>();

                //set the text for the interactable onscreen 
                temp.interactText.text = interactionTrigger.currentObjects[i].GetComponent<Interactable>().type 
                    + " " + interactionTrigger.currentObjects[i].GetComponent<Interactable>().name;

                temp.indexInList = i;

                //put the interactable in the list
                newButton.transform.SetParent(interactList.transform);

                //for some reason Unity does not use full scale for the instantiated object by default
                newButton.transform.localScale = Vector3.one;

                if(interactionTrigger.currentObjects[i].GetComponent<Interactable>().typeOfInteractable == Interactable.Types.Pickup)
                {
                    numberOfItems++;
                }
            }

            if(numberOfItems > 0)
            {
                collectAllButton.interactable = true;
            }
            else
            {
                collectAllButton.interactable = false;
            }

            interactBar.SetActive(false);
            interactContainer.SetActive(true);

            //move the scrollbar back to the top of the list
            scrollRect.verticalNormalizedPosition = 1.0f;

            //pause game time (in part to prevent user input)
            //TODO: decide if we want to keep time paused here or just disable user action/motion input
            Time.timeScale = 0;
            Cursor.visible = true;
        }

        //performs the interaction with the selected item
        public void Interact(int index)
        {
            GameObject interactable = interactionTrigger.currentObjects[index];
            CloseContainer();
            switch(interactable.GetComponent<Interactable>().typeOfInteractable)
            {
                case Interactable.Types.Pickup:
                    character.AddItem(interactable.GetComponent<Interactable>().name);
                    StartCoroutine(interactable.GetComponent<Pickup>().Collect(gameObject));
                    StartCoroutine(InteractDelay(false));
                    break;
                case Interactable.Types.NPC:
                    //open talk dialogue
                    string name = interactable.GetComponent<NPC>().name;
                    int state = interactable.GetComponent<NPC>().currentDialogueState;
                    dialogueManager.StartDialogue(name, state);
                    StartCoroutine(InteractDelay(true));
                    break;
                case Interactable.Types.BackDoor:
                    //toggle item open/closed based on current state
                    StartCoroutine(interactable.GetComponent<BackDoor>().EnterDoor(gameObject, true));
                    StartCoroutine(InteractDelay(false));
                    break;
                case Interactable.Types.SideDoor:
                    interactable.GetComponent<SideDoor>().ToggleState();
                    StartCoroutine(InteractDelay(false));
                    break;
                case Interactable.Types.Chest:
                    //open chest
                    StartCoroutine(InteractDelay(false));
                    break;
                default:
                    Debug.Log("Error: interact type is unknown. Please add its behavior to CharacterInteraction.");
                    break;
            }
        }

        private IEnumerator InteractDelay(bool isExtended)
        {
            playerInput.DisableInput(false);
            isInteracting = true;
            yield return new WaitForSeconds(interactTime);
            isInteracting = false;
            if(!isExtended)
            {
                playerInput.EnableInput();
            }
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

            playerInput.EnableInput();
        }

        //collects all items in the player's vicinity
        public void CollectAllItems()
        {
            for (int i = interactionTrigger.currentObjects.Count - 1; i >= 0; i--)
            {
                Interactable io = interactionTrigger.currentObjects[i].GetComponent<Interactable>();
                if (io.typeOfInteractable == Interactable.Types.Pickup)
                {
                    character.AddItem(io.name);
                    StartCoroutine(interactionTrigger.currentObjects[i].GetComponent<Pickup>().Collect(gameObject));
                }
            }
            CloseContainer();
            StartCoroutine(InteractDelay(false));
        }

        //changes the interact key for display
        //TODO: implement
        public void InteractKeyChange(string newKey)
        {
            interactKey = newKey;
        }

        private void SetInteractBarText(string interactType, string interactItem, bool isMultiple)
        {
            string press = "<color=black>Press ";
            string key = "<color=red>" + interactKey;
            string type = "<color=black> to " + interactType;
            if(isMultiple)
            {
                interactBarText.text = press + key + type;
            }
            else
            {
                //TODO: can set the item color based on item rarity
                string item = "<color=red> " + interactItem;
                interactBarText.text = press + key + type + item;
            }
        }  
    }
}
