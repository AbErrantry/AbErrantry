using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dialogue2D;
using UnityEngine;
using UnityEngine.UI;
public class QuestData : ScriptableObject
{
    public Dictionary<string, Quest> questDictionary;
    private string root;

    //default constructor
    private void OnEnable()
    {
        questDictionary = new Dictionary<string, Quest>();
        root = Application.streamingAssetsPath + "/Quests/";
        GetAllQuests();
        PrintQuests(); //uncomment for debug
    }

    private void GetAllQuests()
    {
        List<string> files = new List<string>(Directory.GetFiles(root));
        foreach (string file in files)
        {
            if (file.EndsWith(".xml"))
            {
                Quest quest = GetQuest(file);
                questDictionary.Add(quest.name, quest);
            }
        }
    }

    private Quest GetQuest(string path)
    {
        Quest quest = new Quest();
        quest.segments = new Dictionary<int, QuestSegment>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(path);

        quest.name = XDoc.Root.Element("name").ElementValueNull_String();

        quest.text = XDoc.Root.Element("text").ElementValueNull_String();

        quest.segments = (from step in XDoc.Root.Elements("step") select new QuestSegment
        {
            id = step.AttributeValueNull_Integer("id"),
                text = step.Element("text").ElementValueNull_String(),
                hint = step.Element("hint").ElementValueNull_String(),
        }).ToDictionary(x => x.id, x => x);

        return quest;
    }

    private void PrintQuests()
    {
        foreach (var quest in questDictionary.Values)
        {
            Debug.Log("Name: " + quest.name + ": " + quest.text);
            foreach (var step in quest.segments.Values)
            {
                Debug.Log("     ID: " + step.id + ": Text: " + step.text + " | Hint: " + step.hint);
            }
        }
    }
}
