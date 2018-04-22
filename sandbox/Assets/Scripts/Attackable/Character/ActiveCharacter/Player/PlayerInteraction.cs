using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character2D
{
    public class PlayerInteraction : MonoBehaviour
    {
        public InteractionTrigger interactionTrigger;
        private PlayerInput playerInput;
        private Dialogue2D.DialogueManager dialogueManager;
        private PlayerInventory playerInventory;

        public GameObject interactBar; //reference to the interact popup bar that asks for input to interact
        public GameObject interactButton; //reference to the interact button prefab
        public GameObject interactList; //reference to the interact list which contains interact button prefabs
        public GameObject interactContainer; //reference to the interact container which contains the interact list

        private RectTransform interactTransform;

        public ScrollRect scrollRect; //the default y position of the interact list (to scroll back to the top)
        public EventSystem eventSystem;

        public TMP_Text interactBarText; //the text displayed on the interact bar
        public Button collectAllButton;

        public bool interactionInput; //whether the character is trying to interact or not
        public bool isInteracting;
        public float interactTime;

        private float xMinLeft;
        private float xMaxLeft;
        private float xMinRight;
        private float xMaxRight;

        public bool isOpen;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInventory = GetComponent<PlayerInventory>();
            dialogueManager = GetComponent<Dialogue2D.DialogueManager>();

            interactTransform = interactContainer.GetComponent<RectTransform>();

            xMinLeft = 0.007f;
            xMaxLeft = 0.50f;
            xMinRight = 0.50f;
            xMaxRight = 0.993f;

            isInteracting = false;
            interactionInput = false;
            interactBar.SetActive(false);
            interactContainer.SetActive(false);

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
            StopCoroutine();

            playerInput.DisableInput(true);
            int numberOfItems = 0;
            //iterate through the list of interactables spawning buttons on screen in a list
            for (int i = 0; i < interactionTrigger.currentObjects.Count; i++)
            {
                //instantiate a prefab for the interact button
                GameObject newButton = Instantiate(interactButton) as GameObject;
                InteractionPrefabReference temp = newButton.GetComponent<InteractionPrefabReference>();

                //set the text for the interactable onscreen
                temp.interactText.text = interactionTrigger.currentObjects[i].GetComponent<Interactable>().type +
                    " " + interactionTrigger.currentObjects[i].GetComponent<Interactable>().name;
                temp.indexInList = i;

                //put the interactable in the list
                newButton.transform.SetParent(interactList.transform);

                //for some reason Unity does not use full scale for the instantiated object by default
                newButton.transform.localScale = Vector3.one;

                if (interactionTrigger.currentObjects[i].GetComponent<Interactable>().typeOfInteractable == Interactable.Types.Pickup)
                {
                    numberOfItems++;
                }
            }

            if (numberOfItems > 0)
            {
                collectAllButton.interactable = true;
            }
            else
            {
                collectAllButton.interactable = false;
            }

            isOpen = true;

            Player.instance.PlayOpenMenuNoise();

            interactBar.SetActive(false);
            interactContainer.SetActive(true);
            ElementFocus.focus.SetMenuFocus(interactList.transform.GetChild(0).gameObject, scrollRect, interactList.GetComponent<RectTransform>());

            if (CameraShift.instance.ShiftCameraLeft(false))
            {
                interactTransform.anchorMin = new Vector2(xMinLeft, interactTransform.anchorMin.y);
                interactTransform.anchorMax = new Vector2(xMaxLeft, interactTransform.anchorMax.y);
            }
            else
            {
                interactTransform.anchorMin = new Vector2(xMinRight, interactTransform.anchorMin.y);
                interactTransform.anchorMax = new Vector2(xMaxRight, interactTransform.anchorMax.y);
            }

            interactContainer.GetComponent<Animator>().SetBool("IsOpen", true);

            //move the scrollbar back to the top of the list
            scrollRect.verticalNormalizedPosition = 1.0f;
        }

        //performs the interaction with the selected item
        public void Interact(int index)
        {
            GameObject interactable = interactionTrigger.currentObjects[index];
            CloseContainer(false);

            switch (interactable.GetComponent<Interactable>().typeOfInteractable)
            {
                case Interactable.Types.Pickup:
                    playerInventory.CollectPickup(interactable.GetComponent<Pickup>());
                    interactable.GetComponent<Pickup>().StartCollectRoutine(gameObject);
                    StartCoroutine(InteractDelay(false));
                    break;
                case Interactable.Types.NPC:
                    StartCoroutine(InteractDelay(true));
                    dialogueManager.StartDialogue(interactable.GetComponent<NPC>().name, interactable.GetComponent<NPC>().currentDialogueState, interactable.gameObject);
                    break;
                case Interactable.Types.BackDoor:
                    //toggle item open/closed based on current state
                    if (!interactable.GetComponent<Openable>().isLocked)
                    {
                        interactable.GetComponent<BackDoor>().StartEnterDoorRoutine(gameObject, true);
                        StartCoroutine(InteractDelay(false));
                    }
                    else
                    {
                        interactable.GetComponent<Openable>().TryUnlock();
                    }
                    break;
                case Interactable.Types.SideDoor:
                    if (!interactable.GetComponent<Openable>().isLocked)
                    {
                        interactable.GetComponent<SideDoor>().ToggleState();
                        StartCoroutine(InteractDelay(false));
                    }
                    else
                    {
                        interactable.GetComponent<Openable>().TryUnlock();
                    }
                    break;
                case Interactable.Types.Chest:
                    if (!interactable.GetComponent<Openable>().isLocked)
                    {
                        Chest chest = interactable.GetComponent<Chest>();
                        chest.ToggleState();
                        playerInventory.InstantiateItem(GameData.data.itemData.itemDictionary[chest.itemName], chest.transform.position);
                        StartCoroutine(InteractDelay(false));
                    }
                    else
                    {
                        interactable.GetComponent<Openable>().TryUnlock();
                    }
                    break;
                case Interactable.Types.Sign:
                    StartCoroutine(InteractDelay(true));
                    dialogueManager.StartDialogue(interactable.GetComponent<Sign>().name, interactable.GetComponent<Sign>().currentDialogueState, interactable.gameObject);
                    break;
                default:
                    Debug.LogError("Interact type is unknown. Please add its behavior to CharacterInteraction.");
                    break;
            }
        }

        private IEnumerator InteractDelay(bool isExtended)
        {
            playerInput.DisableInput(false);
            isInteracting = true;
            yield return new WaitForSeconds(interactTime);
            isInteracting = false;
            if (!isExtended)
            {
                playerInput.EnableInput();
            }
        }

        //cleans up the screen after an interactable is chosen
        public void CloseContainer(bool closedOnOwn = true)
        {
            if (isOpen)
            {
                interactContainer.GetComponent<Animator>().SetBool("IsOpen", false);
                StartCoroutine(WaitForClose());

                CameraShift.instance.ResetCamera();

                //display the interact popup
                DisplayText();

                //delete ui elements from the list for the next iteration
                var children = new List<GameObject>();
                foreach (Transform child in interactList.transform)
                {
                    children.Add(child.gameObject);
                }
                children.ForEach(child => Destroy(child));
                playerInput.EnableInput(closedOnOwn);
                ElementFocus.focus.RemoveFocus();

                isOpen = false;

                Player.instance.PlayCloseMenuNoise();
            }
        }

        private IEnumerator WaitForClose()
        {
            yield return new WaitForSeconds(0.5f);
            interactContainer.SetActive(false);
        }

        private void StopCoroutine()
        {
            StopAllCoroutines();
            interactContainer.SetActive(false);
        }

        //collects all items in the player's vicinity
        public void CollectAllItems()
        {
            for (int i = interactionTrigger.currentObjects.Count - 1; i >= 0; i--)
            {
                Interactable io = interactionTrigger.currentObjects[i].GetComponent<Interactable>();
                if (io.typeOfInteractable == Interactable.Types.Pickup)
                {
                    var pick = io.gameObject.GetComponent<Pickup>();
                    playerInventory.CollectPickup(pick);
                    interactionTrigger.currentObjects[i].GetComponent<Pickup>().StartCollectRoutine(gameObject);
                }
            }
            CloseContainer();
            StartCoroutine(InteractDelay(false));
        }

        private void SetInteractBarText(string interactType, string interactItem, bool isMultiple)
        {
            string press = "<color=white>Press ";
            string key = "<color=red>" + NameConversion.ConvertSymbol(GetKeyName.Input("Interact", KeyTarget.PositivePrimary));
            string type = "<color=white> to " + interactType;
            if (isMultiple)
            {
                interactBarText.text = press + key + type;
            }
            else
            {
                string item = "<color=red> " + interactItem;
                interactBarText.text = press + key + type + NameConversion.ConvertSymbol(item);
            }
        }
    }
}
