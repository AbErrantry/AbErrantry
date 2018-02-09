﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dialogue2D
{
    public class DialogueManager : MonoBehaviour
    {
        private Character2D.PlayerInput playerInput;

        public CameraShift cameraShift;

        public TMP_Text nameText;
        public TMP_Text dialogueText;

        public GameObject choiceButton;
        public GameObject choiceList;

        public Animator dialogueAnimator;
        public RectTransform dialogueTransform;

        public GameObject dialogueContainer;

        private List<DialogueSegment> dialogueSegments;
        private DialogueSegment currentSegment;

        public ScrollRect scrollRect;

        public float xMinLeft;
        public float xMaxLeft;

        public float xMinRight;
        public float xMaxRight;

        public float textSpeed;

        //used for initialization
        private void Start ()
        {
            dialogueSegments = new List<DialogueSegment>();
            currentSegment = new DialogueSegment();
            playerInput = GetComponent<Character2D.PlayerInput>();

            dialogueContainer.SetActive(false);
            
            textSpeed = 2.0f;

            xMinLeft = 0.01f;
            xMaxLeft = 0.50f;

            xMinRight = 0.50f;
            xMaxRight = 0.99f;
        }

        //finishes the dialogue
        private void EndDialogue()
        {
            playerInput.EnableInput();
            dialogueAnimator.SetBool("IsOpen", false);
            cameraShift.ResetCamera();
            FlushChoices();
            dialogueContainer.SetActive(false);
        }

        //submits the choice picked by the user to get the next segment
        public void SubmitChoice(int InChoice)
        {
            currentSegment.next = InChoice;
            DoActions();
            GetNextSegment();
        }

        //Todo: fix implementation. Make choice list dynamic
        //      =0 choices - continue
        //      >0 choices - display all in text boxes
        private void DisplaySegment()
        {
            StopAllCoroutines();
            FlushChoices();
            StartCoroutine(TypeSentence(currentSegment.text));
        }

        private void DisplayChoices()
        {
            if(currentSegment.choices.Count > 0)
            {
                foreach(DialogueChoice choice in currentSegment.choices)
                {
                    CreateChoiceButton(choice.text, choice.next);
                }
            }
            else
            {
                CreateChoiceButton("Continue", currentSegment.next);
            }
            ElementFocus.focus.SetFocus(choiceList.transform.GetChild(0).gameObject, scrollRect, choiceList.GetComponent<RectTransform>());
        }

        private void DoActions()
        {
            foreach(DialogueAction action in currentSegment.actions)
            {
                switch(action.type)
                {
                    case ActionTypes.BecomeHostile:
                        Debug.Log(nameText.text + " became hostile.");
                        //call function to make character hostile
                        break;
                    case ActionTypes.Disappear:
                        Debug.Log(nameText.text + " vanished.");
                        //call function to make character disappear and remove from memory
                            //add a fade to black and back to let the character disappear
                        break;
                    case ActionTypes.GiveGold:
                        Debug.Log(nameText.text + " gave you " + action.number + " gold.");
                        //add action.amount gold to the player's gold supply and notify them
                        break;
                    case ActionTypes.GiveItem:
                        Debug.Log(nameText.text + " gave you " + action.number + " " + action.name + "(s).");
                        //add action.amount action.name to the player's inventory and notify them
                        break;
                    case ActionTypes.None:
                        break;
                    case ActionTypes.OpenShopMenu:
                        Debug.Log("Opened shop menu.");
                        //open shop menu where you can buy things from the shopkeep's inventory and sell things from yours
                            //buying adds items to your inventory and removes from theirs
                            //selling removes items from your inventory and adds to theirs
                                //TODO: after x amount of time played, add items to each shopkeep's inventory.
                        break;
                    case ActionTypes.ProgressDialogue:
                        Debug.Log("Dialogue progressed to conversation " + action.number + ".");
                        //move to a new conversation id that has been specified.
                        break;
                    case ActionTypes.ProgressQuest:
                        Debug.Log("Quest " + action.name + " progressed to step " + action.number);
                        //move to a new point in the specified quest.
                        break;
                    case ActionTypes.RequestGold:
                        Debug.Log(nameText.text + " requests " + action.number + " gold.");
                        //request gold to continue the specified conversation
                            //can also progress to a dialogue that starts off here if this is the only continuing branch
                        break;
                    case ActionTypes.RequestItem:
                        Debug.Log(nameText.text + " requests " + action.number + " " + action.name + "(s).");
                        //request items to continue the specified conversation
                            //can also progress to a dialogue that starts off here if this is the only continuing branch
                        break;
                    case ActionTypes.TakeGold:
                        Debug.Log(nameText.text + " took " + action.number + " gold.");
                        //take/steal action.number gold from the player. If they do not have that much, take as much as they have.
                        break;
                    case ActionTypes.TransportLevel:
                        Debug.Log(nameText.text + " transported to " + action.name + " level at location x=" + action.xloc + ", y=" + action.yloc + ".");
                        //transport the character to the specified level at the coordinates provided.
                            //they will be added to a list of characters in that level with specified coordinates.
                            //add a fade to black and back to let the character disappear
                        break;
                    case ActionTypes.TransportLocation:
                        Debug.Log(nameText.text + " transported to " + action.name + " level at location x=" + action.xloc + ", y=" + action.yloc + ".");
                        //transport the character to the specified coordinates in the current level
                            //they will be added to a list of characters in this level with new specified coordinates.
                            //they will stay instantiated and actually be transported in this case.
                            //add a fade to black and back to let the character disappear
                        break;
                    default:
                        Debug.LogError(nameText.text + " has an action of type " + action.type.ToString() + " which is undefined.");
                        break;
                }
            }
        }

        //
        private void FlushChoices()
        {
            var children = new List<GameObject>();
            foreach (Transform child in choiceList.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));
            ElementFocus.focus.RemoveFocus();
        }

        private void CreateChoiceButton(string text, int next)
        {
            //TODO: fix comments
            //instantiate a prefab for the interact button
            GameObject newButton = Instantiate(choiceButton) as GameObject;
            DialoguePrefabReference controller = newButton.GetComponent<DialoguePrefabReference>();

            //set the text for the choice onscreen 
            controller.choiceText.text = text;
            controller.choiceNext = next;

            //put the interactable in the list
            newButton.transform.SetParent(choiceList.transform);

            //for some reason Unity does not use full scale for the instantiated object by default
            newButton.transform.localScale = Vector3.one;
        }

        //gets the next segment in dialogue
        public void GetNextSegment()
        {
            if (currentSegment.next == -1)
            {
                EndDialogue();
            }
            else
            {
                //get the next segment
                foreach(DialogueSegment segment in dialogueSegments)
                {
                    if(currentSegment.next == segment.id)
                    {
                        currentSegment = segment;
                        break;
                    }
                }
                DisplaySegment();
            }
        }

        //coroutine that animates the text on screen character by character
        private IEnumerator TypeSentence(string Segment)
        {
            dialogueText.text = "";
            foreach(char letter in Segment.ToCharArray())
            {
                dialogueText.text = dialogueText.text + letter; //add audio for letter being played
                yield return new WaitForSeconds(0.01f * textSpeed);
            }
            DisplayChoices();
        }

        //starts a dialogue once the character triggers it
        public void StartDialogue(string charName, int convName)
        {
            dialogueContainer.SetActive(true);
            dialogueAnimator.SetBool("IsOpen", true);
            dialogueSegments.Clear();
            dialogueSegments = GameData.data.dialogueData.dialogueDictionary[charName].conversation[convName].segments.Values.ToList();

            if(cameraShift.ShiftCameraLeft(false))
            {
                dialogueTransform.anchorMin = new Vector2(xMinLeft, dialogueTransform.anchorMin.y);
                dialogueTransform.anchorMax = new Vector2(xMaxLeft, dialogueTransform.anchorMax.y);
            }
            else
            {
                dialogueTransform.anchorMin = new Vector2(xMinRight, dialogueTransform.anchorMin.y);
                dialogueTransform.anchorMax = new Vector2(xMaxRight, dialogueTransform.anchorMax.y);
            }

            nameText.text = charName;

            if (dialogueSegments.Count > 0)
            {
                currentSegment = dialogueSegments.First();
            }
            else
            {
                //error handling of empty conversation
                Debug.LogError("Conversation has zero segments." + charName + " " + convName);
                return;
            }

            DisplaySegment();
        }
    }
}