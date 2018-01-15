using System.Collections;
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
        public GameData gameData;
        private Character2D.PlayerInput playerInput;

        public TMP_Text nameText;
        public TMP_Text dialogueText;

        public GameObject choiceButton;
        public GameObject choiceList;

        public Animator dialogueAnimator;

        private List<DialogueSegment> dialogueSegments;
        private DialogueSegment currentSegment;

        public float textSpeed;

        //used for initialization
        private void Start ()
        {
            dialogueSegments = new List<DialogueSegment>();
            currentSegment = new DialogueSegment();
            playerInput = GetComponent<Character2D.PlayerInput>();
            
            textSpeed = 2.0f;
        }

        //finishes the dialogue
        private void EndDialogue()
        {
            dialogueAnimator.SetBool("IsOpen", false);
            playerInput.EnableInput();
            //close dialogue and etc etc
        }

        //submits the choice picked by the user to get the next segment
        public void SubmitChoice(int InChoice)
        {
            currentSegment.next = InChoice;
            GetNextSegment();
        }

        //Todo: fix implementation. Make choice list dynamic
        //      =0 choices - continue
        //      >0 choices - display all in text boxes
        private void DisplaySegment()
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSegment.text));
            FlushChoices();

            //flush buttons from UI

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
        }

        //starts a dialogue once the character triggers it
        public void StartDialogue(string charName, int convName)
        {
            dialogueAnimator.SetBool("IsOpen", true);
            dialogueSegments.Clear();
            dialogueSegments = gameData.dialogueData.dialogueDictionary[charName].conversation[convName].segments.Values.ToList();

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
