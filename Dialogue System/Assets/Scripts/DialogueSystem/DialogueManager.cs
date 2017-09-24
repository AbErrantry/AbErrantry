using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;

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
    private List<DialogueSegment> DialogueSegments;
    private DialogueSegment CurrentSegment;
    private string CharacterName;
    private string DialogueFileName;
    private int NextSegmentID;

	// Use this for initialization
	void Start ()
    {
        DialogueSegments = new List<DialogueSegment>();
        CurrentSegment = new DialogueSegment();
        CharacterName = "";
        DialogueFileName = "";
        NextSegmentID = -1;
    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation.");
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

    public void DisplaySegment()
    {
        switch (CurrentSegment.SegmentType)
        {
            case 0: //end of dialogue

                break;
            case 1: //continue

                break;
            case 2: //2 choice response

                break;
            case 3: //4 choice response

                break;
            default:
                Debug.Log("Error. Reached default in switch statement that should not have been reached.");
                break;
        }
        Debug.Log(CurrentSegment.SegmentText);
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


    public void StartDialogue()
    {
        Debug.Log("Starting conversation with NPC.");
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
}
