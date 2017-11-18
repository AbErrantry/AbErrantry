using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class XElementExtensionMethods
{
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
    public TMP_Text NameText;
    public TMP_Text DialogueText;

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

    public Animator DialogueAnimator;

    private List<DialogueSegment> DialogueSegments;
    private DialogueSegment CurrentSegment;
    private string CharacterName;
    private string DialogueFileName;

	// Use this for initialization
	void Start ()
    {
        DialogueSegments = new List<DialogueSegment>();
        CurrentSegment = new DialogueSegment();
        CharacterName = "";
        DialogueFileName = "";
    }

    private void EndDialogue()
    {
        DialogueAnimator.SetBool("IsOpen", false);
        //close dialogue and etc etc
    }

    public void SubmitChoice(int InChoice)
    {
        foreach (DialogueChoice Choice in CurrentSegment.SegmentChoices)
        {
            if (InChoice == Choice.ChoiceID)
            {
                CurrentSegment.SegmentNextID = Choice.ChoiceNextID;
                break;
            }
        }
        GetNextSegment();
    }

    //todo: fix implementation. Make choice list dynamic
    //      =0 choices - continue
    //      >0 choices - display all in text boxes
    private void DisplaySegment()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(CurrentSegment.SegmentText));
        DisableButtons();
        switch (CurrentSegment.SegmentType)
        {
            case 0: //end of dialogue
                ContinueButton.SetActive(true);
                break;
            case 1: //continue
                ContinueButton.SetActive(true);
                break;
            case 2: //2 choice response
                Choice_0_Text.text = CurrentSegment.SegmentChoices.ElementAt(0).ResponseText;
                Choice_1_Text.text = CurrentSegment.SegmentChoices.ElementAt(1).ResponseText;
                Choice_0_Button.SetActive(true);
                Choice_1_Button.SetActive(true);
                break;
            case 3: //4 choice response
                Choice_0_Text.text = CurrentSegment.SegmentChoices.ElementAt(0).ResponseText;
                Choice_1_Text.text = CurrentSegment.SegmentChoices.ElementAt(1).ResponseText;
                Choice_2_Text.text = CurrentSegment.SegmentChoices.ElementAt(2).ResponseText;
                Choice_3_Text.text = CurrentSegment.SegmentChoices.ElementAt(3).ResponseText;
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

    public void GetNextSegment()
    {
        if (CurrentSegment.SegmentType == 0)
        {
            EndDialogue();
        }
        else
        {
            //get the next segment
            foreach(DialogueSegment Segment in DialogueSegments)
            {
                if(CurrentSegment.SegmentNextID == Segment.SegmentID)
                {
                    CurrentSegment = Segment;
                    break;
                }
            }
            DisplaySegment();
        }
    }

    IEnumerator TypeSentence(string Segment)
    {
        DialogueText.text = "";
        foreach(char Letter in Segment.ToCharArray())
        {
            DialogueText.text = DialogueText.text + Letter;
            yield return new WaitForFixedUpdate(); //Todo: change to a WaitForSeconds to allow for different text speeds.
        }
    }

    public void StartDialogue()
    {
        DialogueAnimator.SetBool("IsOpen", true);

        NameText.text = "Villager"; //Todo: pull from character in actual dialogue. maybe as a parameter here?

        DisableButtons();

        DialogueSegments.Clear();
        GetDialogueFile(); //todo: implement
        GetDialogue();
        //PrintDialogue(); //todo: remove since only for debugging

        if(DialogueSegments.Count > 0)
        {
            CurrentSegment = DialogueSegments.First();
        }
        else
        {
            //error handling of empty conversation
            Debug.Log("Error. Conversation has zero segments.");
            return;
        }

        DisplaySegment();
    }

    private void GetDialogueFile()
    {
        //todo: implement
        //get the file that is associated with the NPC and game state
    }

    private void GetDialogue()
    {
        //parse XML with LINQ
        XDocument XDoc = XDocument.Load("SampleShopkeepDialogue.xml");
        DialogueSegments = (from Segment in XDoc.Root.Elements("segment")
                            select new DialogueSegment
                            {
                                SegmentID = Segment.AttributeValueNull_Integer("id"),
                                SegmentText = Segment.Element("content").ElementValueNull_String(),
                                SegmentType = Segment.Element("type").ElementValueNull_Integer(),
                                SegmentNextID = Segment.Element("next").ElementValueNull_Integer(),
                                //SegmentAction = TODO: implement
                                SegmentChoices = Segment.Elements("choice")
                                    .Select(Choice => new DialogueChoice
                                    {
                                        ChoiceID = Choice.AttributeValueNull_Integer("id"),
                                        ChoiceNextID = Choice.Element("next").ElementValueNull_Integer(),
                                        ResponseText = Choice.Element("response").ElementValueNull_String(),
                                    }).OrderBy(x => x.ChoiceID).ToList()
                            }).OrderBy(x => x.SegmentID).ToList();
    }

    //TODO: remove debug function
    private void PrintDialogue()
    {
        foreach(DialogueSegment Segment in DialogueSegments)
        {
            Debug.Log(Segment.SegmentID + " " + Segment.SegmentText + " " + Segment.SegmentType + " " + Segment.SegmentNextID + " " + Segment.SegmentChoices.Count);
            if(Segment.SegmentChoices.Count != 0)
            {
                foreach(DialogueChoice Choice in Segment.SegmentChoices)
                Debug.Log(Choice.ChoiceID + " " + Choice.ChoiceNextID + " " + Choice.ResponseText);
            }
        }
    }

    private void DisableButtons()
    {
        Choice_0_Button.SetActive(false);
        Choice_1_Button.SetActive(false);
        Choice_2_Button.SetActive(false);
        Choice_3_Button.SetActive(false);
        ContinueButton.SetActive(false);
    }
}
