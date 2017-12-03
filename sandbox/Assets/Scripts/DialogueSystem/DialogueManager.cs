using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//extension methods to null check XElements from a LINQ query
public static class XElementExtensionMethods
{
    //null checks a string value
    public static string ElementValueNull_String(this XElement element)
    {
        if (element != null)
        {
            return element.Value;
        }
        else
        {
            return "";
        }
    }

    //null checks an integer value
    public static int ElementValueNull_Integer(this XElement element)
    {
        if (element != null)
        {
            return int.Parse(element.Value);
        }
        else
        {
            return -1;
        }
    }

    //null checks a string attribute
    public static string AttributeValueNull_String(this XElement element, string attributeName)
    {
        if (element != null)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr != null)
            {
                return attr.Value;
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    //null checks an integer attribute
    public static int AttributeValueNull_Integer(this XElement element, string attributeName)
    {
        if (element != null)
        {
            XAttribute attr = element.Attribute(attributeName);
            if(attr != null)
            {
                return int.Parse(attr.Value);
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
}

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //TODO: remove for a dynamic list
    public TMP_Text Choice_0_Text;
    public TMP_Text Choice_1_Text;
    public TMP_Text Choice_2_Text;
    public TMP_Text Choice_3_Text;
    public TMP_Text ContinueText;
    public GameObject Choice_0_Button;
    public GameObject Choice_1_Button;
    public GameObject Choice_2_Button;
    public GameObject Choice_3_Button;
    public GameObject ContinueButton;

    public Animator dialogueAnimator;

    private List<DialogueSegment> dialogueSegments;
    private DialogueSegment currentSegment;
    private string characterName;
    private string dialogueFile;

	// Use this for initialization
	private void Start ()
    {
        dialogueSegments = new List<DialogueSegment>();
        currentSegment = new DialogueSegment();
        characterName = "";
        dialogueFile = "";
    }

    //finishes the dialogue
    private void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        //close dialogue and etc etc
    }

    //submits the choice picked by the user to get the next segment
    public void SubmitChoice(int InChoice)
    {
        foreach (DialogueChoice choice in currentSegment.choices)
        {
            if (InChoice == choice.id)
            {
                currentSegment.next = choice.next;
                break;
            }
        }
        GetNextSegment();
    }

    //Todo: fix implementation. Make choice list dynamic
    //      =0 choices - continue
    //      >0 choices - display all in text boxes
    private void DisplaySegment()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSegment.text));
        DisableButtons();
        switch (currentSegment.type)
        {
            case 0: //end of dialogue
                ContinueButton.SetActive(true);
                break;
            case 1: //continue
                ContinueButton.SetActive(true);
                break;
            case 2: //2 choice response
                Choice_0_Text.text = currentSegment.choices.ElementAt(0).text;
                Choice_1_Text.text = currentSegment.choices.ElementAt(1).text;
                Choice_0_Button.SetActive(true);
                Choice_1_Button.SetActive(true);
                break;
            case 3: //4 choice response
                Choice_0_Text.text = currentSegment.choices.ElementAt(0).text;
                Choice_1_Text.text = currentSegment.choices.ElementAt(1).text;
                Choice_2_Text.text = currentSegment.choices.ElementAt(2).text;
                Choice_3_Text.text = currentSegment.choices.ElementAt(3).text;
                Choice_0_Button.SetActive(true);
                Choice_1_Button.SetActive(true);
                Choice_2_Button.SetActive(true);
                Choice_3_Button.SetActive(true);
                break;
            default:
                Debug.Log("Error. Reached default in switch statement that should not have been reached.");
                break;
        }
    }

    //gets the next segment in dialogue
    public void GetNextSegment()
    {
        if (currentSegment.type == 0)
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
            //add audio for letter being played
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForFixedUpdate(); //Todo: change to a WaitForSeconds to allow for different text speeds.
        }
    }

    //starts a dialogue once the character triggers it
    public void StartDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", true);

        DisableButtons();

        dialogueSegments.Clear();
        GetDialogueFile(); //todo: implement
        GetDialogue();
        //PrintDialogue(); //todo: remove since only for debugging

        //get the character's name
        nameText.text = characterName; //Todo: pull from character in actual dialogue. maybe as a parameter here?

        if (dialogueSegments.Count > 0)
        {
            currentSegment = dialogueSegments.First();
        }
        else
        {
            //error handling of empty conversation
            Debug.Log("Error. Conversation has zero segments.");
            return;
        }

        DisplaySegment();
    }

    //locates the dialogue file that is needed for the specific conversation
    private void GetDialogueFile()
    {
        //todo: implement
        //get the file that is associated with the NPC and game state
        dialogueFile = "SampleShopkeepDialogue.xml"; //TODO: abstract
        characterName = "Villager"; //TODO: abstract
    }

    //gets the current conversation into memory from a file
    private void GetDialogue()
    {
        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(dialogueFile);
        dialogueSegments = (from segment in XDoc.Root.Elements("segment")
                            select new DialogueSegment
                            {
                                id = segment.AttributeValueNull_Integer("id"),
                                text = segment.Element("content").ElementValueNull_String(),
                                type = segment.Element("type").ElementValueNull_Integer(),
                                next = segment.Element("next").ElementValueNull_Integer(),
                                //SegmentAction = TODO: implement
                                choices = segment.Elements("choice")
                                    .Select(choice => new DialogueChoice
                                    {
                                        id = choice.AttributeValueNull_Integer("id"),
                                        next = choice.Element("next").ElementValueNull_Integer(),
                                        text = choice.Element("response").ElementValueNull_String(),
                                    }).OrderBy(x => x.id).ToList()
                            }).OrderBy(x => x.id).ToList();
    }

    //TODO: remove debug function
    //prints out the dialogue information
    private void PrintDialogue()
    {
        foreach(DialogueSegment segment in dialogueSegments)
        {
            Debug.Log(segment.id + " " + segment.text + " " + segment.type + " " + segment.next + " " + segment.choices.Count);
            if(segment.choices.Count != 0)
            {
                foreach(DialogueChoice choice in segment.choices)
                {
                    Debug.Log(choice.id + " " + choice.next + " " + choice.text);
                }
            }
        }
    }

    //disables the dialogue buttons for the next segment
    private void DisableButtons()
    {
        Choice_0_Button.SetActive(false);
        Choice_1_Button.SetActive(false);
        Choice_2_Button.SetActive(false);
        Choice_3_Button.SetActive(false);
        ContinueButton.SetActive(false);
    }
}
