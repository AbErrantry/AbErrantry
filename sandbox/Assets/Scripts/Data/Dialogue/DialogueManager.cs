using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Character2D;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue2D
{
    public class DialogueManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        private PlayerInventory playerInventory;
        private PlayerQuests playerQuests;
        private Player player;

        private StoreMenu storeMenu;
        private RequestMenu requestMenu;

        public CameraShift cameraShift;
        public FollowTarget followTarget;

        public TMP_Text nameText;
        public TMP_Text dialogueText;
        public GameObject choiceButton;
        public GameObject choiceList;

        public Animator dialogueAnimator;
        public Animator choiceAnimator;
        public Animator continueAnimator;
        public Animator fadeAnimator;

        public RectTransform dialogueTransform;
        public GameObject dialogueContainer;
        private List<DialogueSegment> dialogueSegments;
        private DialogueSegment currentSegment;
        public ScrollRect scrollRect;

        private GameObject character;

        public float xMinLeft;
        public float xMaxLeft;
        public float xMinRight;
        public float xMaxRight;

        public float textSpeed;

        public bool isOpen;

        private bool isWaiting;

        //used for initialization
        private void Start()
        {
            dialogueSegments = new List<DialogueSegment>();
            currentSegment = new DialogueSegment();

            playerInput = GetComponent<PlayerInput>();
            playerMovement = GetComponent<PlayerMovement>();
            playerInventory = GetComponent<PlayerInventory>();
            playerQuests = GetComponent<PlayerQuests>();
            player = GetComponent<Player>();

            storeMenu = GetComponent<StoreMenu>();
            requestMenu = GetComponent<RequestMenu>();

            dialogueContainer.SetActive(false);

            textSpeed = 1.0f;
            xMinLeft = 0.007f;
            xMaxLeft = 0.50f;
            xMinRight = 0.50f;
            xMaxRight = 0.993f;
        }

        //finishes the dialogue
        public void EndDialogue()
        {
            followTarget.SetTarget(playerMovement.gameObject.transform);
            playerMovement.StopCoroutine();
            playerInput.EnableInput(true);
            dialogueAnimator.SetBool("IsOpen", false);
            StartCoroutine(WaitForClose());
            cameraShift.ResetCamera();
            ElementFocus.focus.RemoveFocus();
            if (currentSegment.choices.Count > 0)
            {
                FlushChoices();
            }
        }

        private IEnumerator WaitForClose()
        {
            yield return new WaitForSeconds(0.5f);
            dialogueContainer.SetActive(false);
            isOpen = false;
        }

        private void StopCoroutine()
        {
            StopAllCoroutines();
            dialogueContainer.SetActive(false);
            fadeAnimator.SetBool("isDisappearing", false);
            isOpen = false;
        }

        //submits the choice picked by the user to get the next segment
        public void SubmitChoice(int InChoice)
        {
            choiceAnimator.SetBool("isOpen", false);
            currentSegment.next = InChoice;
            if (currentSegment.choices.Count > 0)
            {
                FlushChoices();
            }
            GetNextSegment();
        }

        private void DisplaySegment()
        {
            StopAllCoroutines();
            if (character.GetComponent<Animator>() != null)
            {
                character.GetComponent<Animator>().SetBool("isTalking", true);
            }
            StartCoroutine(TypeSentence(NameConversion.ConvertSymbol(currentSegment.text)));
        }

        private void DisplayChoices()
        {
            if (character.GetComponent<Animator>() != null)
            {
                character.GetComponent<Animator>().SetBool("isTalking", false);
            }
            if (currentSegment.choices.Count > 0)
            {
                foreach (DialogueChoice choice in currentSegment.choices)
                {
                    CreateChoiceButton(NameConversion.ConvertSymbol(choice.text), choice.next);
                }
                choiceAnimator.SetBool("isOpen", true);
            }
            else
            {
                continueAnimator.SetBool("isActive", true);
            }
        }

        public void FocusOnChoice()
        {
            foreach (Transform child in choiceList.transform)
            {
                child.GetComponent<Button>().interactable = true;
            }
            ElementFocus.focus.SetMenuFocus(choiceList.transform.GetChild(0).gameObject, scrollRect, choiceList.GetComponent<RectTransform>());
        }

        private void DoActions()
        {
            foreach (DialogueAction action in currentSegment.actions)
            {
                switch (action.type)
                {
                    case ActionTypes.AffectKarma:
                        player.SetKarma(action.number);
                        break;
                    case ActionTypes.BecomeHostile:
                        character.GetComponent<NPC>().MakeHostile();
                        break;
                    case ActionTypes.Disappear:
                        character.GetComponent<NPC>().Disappear();
                        StartCoroutine(DisappearRoutine(true));
                        break;
                    case ActionTypes.GiveGold:
                        player.SetGold(action.number);
                        break;
                    case ActionTypes.GiveItem:
                        for (int index = 0; index < action.number; index++)
                        {
                            playerInventory.AddItem(action.name);
                        }
                        break;
                    case ActionTypes.None:
                        //do nothing
                        break;
                    case ActionTypes.OpenShopMenu:
                        isWaiting = true;
                        storeMenu.ToggleStore(character.GetComponent<CharacterInventory>());
                        break;
                    case ActionTypes.ProgressDialogue:
                        character.GetComponent<NPC>().SetDialogueState(action.number);
                        break;
                    case ActionTypes.ProgressQuest:
                        playerQuests.UpdateQuestStep(action.name, action.number);
                        break;
                    case ActionTypes.RequestGold:
                        isWaiting = true;
                        requestMenu.ToggleRequest(Convert.ToInt32(action.xloc), Convert.ToInt32(action.yloc), character.GetComponent<NPC>().name, true, action.number);
                        //TODO: can also progress to a dialogue that starts off here if this is the only continuing branch
                        break;
                    case ActionTypes.RequestItem:
                        isWaiting = true;
                        requestMenu.ToggleRequest(Convert.ToInt32(action.xloc), Convert.ToInt32(action.yloc), character.GetComponent<NPC>().name, false, action.number, action.name);
                        //TODO: can also progress to a dialogue that starts off here if this is the only continuing branch
                        break;
                    case ActionTypes.TakeGold:
                        int total = player.gold;
                        if (total < action.number)
                        {
                            action.number = total;
                        }
                        player.SetGold(-action.number, true);
                        break;
                    case ActionTypes.TransportLevel:
                        character.GetComponent<NPC>().CharacterInfoChanged(action.name, action.xloc, action.yloc);
                        StartCoroutine(DisappearRoutine(true));
                        break;
                    case ActionTypes.TransportLocation:
                        character.GetComponent<NPC>().CharacterInfoChanged(action.name, action.xloc, action.yloc);
                        StartCoroutine(DisappearRoutine(false, action.xloc, action.yloc));
                        break;
                    default:
                        Debug.LogError(nameText.text + " has an action of type " + action.type.ToString() + " which is undefined.");
                        break;
                }
            }
        }

        public void SetDoneWaiting(bool isRequest = false, int segment = -1)
        {
            if (isRequest)
            {
                currentSegment.next = segment;
            }
            isWaiting = false;
            GetNextSegment(false);
        }

        private IEnumerator DisappearRoutine(bool destroy, float xloc = 0.0f, float yloc = 0.0f)
        {
            string characterName = character.GetComponent<NPC>().name;
            isWaiting = true;

            fadeAnimator.SetBool("isDisappearing", true);
            yield return new WaitForSeconds(0.5f);

            isWaiting = false;
            EndDialogue();
            if (destroy)
            {
                Destroy(character);
            }
            else
            {
                character.transform.position = new Vector2(xloc, yloc);
            }

            yield return new WaitForSeconds(0.5f);

            fadeAnimator.SetBool("isDisappearing", false);
            yield return new WaitForSeconds(0.5f);

            EventDisplay.instance.AddEvent(characterName + " has disappeared.");

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

            newButton.GetComponent<Button>().interactable = false;

            //set the text for the choice onscreen
            controller.choiceText.text = text;
            controller.choiceNext = next;

            //put the interactable in the list
            newButton.transform.SetParent(choiceList.transform);

            //for some reason Unity does not use full scale for the instantiated object by default
            newButton.transform.localScale = Vector3.one;
        }

        //gets the next segment in dialogue
        public void GetNextSegment(bool doActions = true)
        {
            continueAnimator.SetBool("isActive", false);
            if (doActions)
            {
                DoActions();
            }
            if (!isWaiting)
            {
                if (currentSegment.next == -1)
                {
                    EndDialogue();
                }
                else
                {
                    //get the next segment
                    foreach (DialogueSegment segment in dialogueSegments)
                    {
                        if (currentSegment.next == segment.id)
                        {
                            currentSegment = segment;
                            break;
                        }
                    }
                    DisplaySegment();
                }
            }
        }

        //coroutine that animates the text on screen character by character
        private IEnumerator TypeSentence(string Segment)
        {
            dialogueText.text = "";
            foreach (char letter in Segment.ToCharArray())
            {
                dialogueText.text = dialogueText.text + letter; //add audio for letter being played
                yield return new WaitForSeconds(0.01f * textSpeed);
            }
            DisplayChoices();
        }

        //starts a dialogue once the character triggers it
        public void StartDialogue(string charName, int convName, GameObject conversingCharacter)
        {
            StopCoroutine();
            isOpen = true;
            character = conversingCharacter;
            nameText.text = NameConversion.ConvertSymbol(charName);

            dialogueContainer.SetActive(true);
            dialogueAnimator.SetBool("IsOpen", true);
            dialogueSegments.Clear();
            dialogueSegments = GameData.data.dialogueData.dialogueDictionary[charName].conversation[convName].segments.Values.ToList();

            playerMovement.StartAutoMoveRoutine(character);

            followTarget.SetTarget(character.transform);

            if (character.GetComponent<Animator>() != null)
            {
                character.GetComponent<Animator>().SetTrigger("isGreeting");
            }

            if (character.transform.position.x < transform.position.x)
            {
                if (character.gameObject.GetComponent<NPC>() != null)
                {
                    character.gameObject.GetComponent<NPC>().FaceRight(true);
                }
            }
            else
            {
                if (character.gameObject.GetComponent<NPC>() != null)
                {
                    character.gameObject.GetComponent<NPC>().FaceRight(false);
                }
            }

            if (cameraShift.ShiftCameraLeft(false))
            {
                dialogueTransform.anchorMin = new Vector2(xMinLeft, dialogueTransform.anchorMin.y);
                dialogueTransform.anchorMax = new Vector2(xMaxLeft, dialogueTransform.anchorMax.y);
            }
            else
            {
                dialogueTransform.anchorMin = new Vector2(xMinRight, dialogueTransform.anchorMin.y);
                dialogueTransform.anchorMax = new Vector2(xMaxRight, dialogueTransform.anchorMax.y);
            }

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
