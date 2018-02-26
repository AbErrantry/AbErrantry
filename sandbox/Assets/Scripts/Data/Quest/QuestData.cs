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
        List<string> directories = new List<string>(Directory.GetDirectories(root));
        foreach (string directory in directories)
        {
            if (directory.EndsWith(".xml"))
            {
                Quest quest = GetQuest(directory + "/");
                questDictionary.Add(quest.name, quest);
            }
        }
    }
    private Quest GetQuest(string path)
    {
        Quest quest = new Quest();
        quest.segments = new Dictionary<int, QuestSegment>();
        return quest;
    }
    private string RemovePostfix(string name)
    {
        string result = string.Empty;
        int index = name.IndexOf("_");
        if (index > 0)
        {
            result = name.Substring(0, index);
        }
        else
        { }
        return result;
    }
    private void PrintQuests()
    { }
}
