using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Xml.Linq;
using Dialogue2D;

public class DialogueData : ScriptableObject
{
    public Dictionary<string, CharacterDialogue> dialogueDictionary;
    private string root;
    
    //default constructor
    private void OnEnable()
    {
        dialogueDictionary = new Dictionary<string, CharacterDialogue>();
        root = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/Dialogue/";
        GetAllDialogue();
    }

    private void GetAllDialogue()
    {
        List<string>directories = new List<string>(Directory.GetDirectories(root));
        foreach(string directory in directories)
        {
            CharacterDialogue cd = GetCharacterDialogue(directory + "/");
            dialogueDictionary.Add(cd.name, cd);
        }
    }

    private CharacterDialogue GetCharacterDialogue(string path)
    {
        List<string>conversations = new List<string>(Directory.GetFiles(path));
        CharacterDialogue chr = new CharacterDialogue();
        chr.conversation = new Dictionary<int, ConversationDialogue>();
        chr.name = new DirectoryInfo(path).Name;
        foreach (string conversation in conversations)
        {
            ConversationDialogue cnv = GetConversationDialogue(conversation);
            chr.conversation.Add(cnv.id, cnv);
        }
        return chr;
    }

    private ConversationDialogue GetConversationDialogue(string conv)
    {
        ConversationDialogue cnv = new ConversationDialogue();
        cnv.segments = new Dictionary<int, DialogueSegment>();

        cnv.id = RemovePostfix(Path.GetFileNameWithoutExtension(conv)); //get id of conversation
        //parse XML with LINQ

        List<DialogueSegment> segments = new List<DialogueSegment>();
        XDocument XDoc = XDocument.Load(conv);
        segments = (from segment in XDoc.Root.Elements("segment")
                            select new DialogueSegment
                            {
                                id = segment.AttributeValueNull_Integer("id"),
                                text = segment.Element("content").ElementValueNull_String(),
                                next = segment.Element("next").ElementValueNull_Integer(),
                                //SegmentAction = TODO: implement
                                choices = segment.Elements("choice")
                                    .Select(choice => new DialogueChoice
                                    {
                                        id = choice.AttributeValueNull_Integer("id"),
                                        next = choice.Element("next").ElementValueNull_Integer(),
                                        text = choice.Element("response").ElementValueNull_String(),
                                    }).OrderBy(x => x.id).ToList(),
                                actions = segment.Elements("action")
                                    .Select(choice => new DialogueAction
                                    {
                                        type = choice.AttributeValueNull_ActionType("type"),
                                        name = choice.Element("name").ElementValueNull_String(),
                                        amount = choice.Element("amount").ElementValueNull_Integer(),
                                    }).OrderBy(x => x.type).ToList() 
                            }).OrderBy(x => x.id).ToList();
        foreach(DialogueSegment segment in segments)
        {
            cnv.segments.Add(segment.id, segment);
        }
        return cnv;
    }

    private int RemovePostfix(string id)
    {
        int result;
        string errorLog = id;
        int index = id.IndexOf("_");
        if (index > 0)
        {
            id = id.Substring(0, index);
        }
        if(int.TryParse(id, out result))
        {
            return result;
        }
        else
        {
            Debug.LogError("Conversation name syntax is invalid: " + errorLog 
                + ". Please change the file name for this conversation to be of the form '<integer>_<description>.xml'.");
            return -1;
        }
    }

    private void PrintDialogue()
    {
        foreach(CharacterDialogue chr in dialogueDictionary.Values)
        {
            Debug.Log("--------------------CHARACTER--------------------");
            Debug.Log("Character Name: " + chr.name);
            foreach(ConversationDialogue cnv in chr.conversation.Values)
            {
                Debug.Log("     ---------------CONVERSATION---------------");
                Debug.Log("         " + "Conversation ID: " + cnv.id);
                foreach(DialogueSegment ds in cnv.segments.Values)
                {
                    Debug.Log("         ----------SEGMENT----------");
                    Debug.Log("             " + "ID: " + ds.id);
                    Debug.Log("             " + "Text: " + ds.text);
                    Debug.Log("             " + "Next ID: " + ds.next);
                    foreach(DialogueChoice dc in ds.choices)
                    {
                        Debug.Log("             -----CHOICE-----");
                        Debug.Log("                 " + "ID: " + dc.id);
                        Debug.Log("                 " + "Text: " + dc.text);
                        Debug.Log("                 " + "Next ID: " + dc.next);
                    }
                    foreach(DialogueAction da in ds.actions)
                    {
                        Debug.Log("             -----ACTION-----");
                        Debug.Log("                 " + "Type: " + da.type.ToString());
                        Debug.Log("                 " + "Name: " + da.name);
                        Debug.Log("                 " + "Amount: " + da.amount);
                    }
                }
            }
        }
        Debug.Log("----------------------------------------");
        Debug.Log("----------------------------------------");
        Debug.Log("----------------------------------------");
        //tests
        Debug.Log(dialogueDictionary["Villager"].conversation[1].segments[3].choices[1].text);
        Debug.Log(dialogueDictionary["Villager2"].conversation[3].segments[0].text);
    }
}
