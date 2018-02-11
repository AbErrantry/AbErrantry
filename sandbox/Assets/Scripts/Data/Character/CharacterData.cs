using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class CharacterData : ScriptableObject
{
    public Dictionary<string, CharacterFields> characterDictionary;

    //default constructor
    private void OnEnable()
    {
        characterDictionary = new Dictionary<string, CharacterFields>();
        GetCharacters();
    }

    //gets each character type into memory from the database
    private void GetCharacters()
    {
        string characterDatabase = Application.dataPath + "/Data/CharacterDatabase.xml";
        List<CharacterFields> characterList = new List<CharacterFields>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(characterDatabase);
        characterList = (from character in XDoc.Root.Elements("character")select new CharacterFields
        {
            type = character.AttributeValueNull_String("type"),
                vitality = character.Element("vitality").ElementValueNull_Float(),
                strength = character.Element("strength").ElementValueNull_Float(),
                agility = character.Element("agility").ElementValueNull_Float(),
                weight = character.Element("weight").ElementValueNull_Float(),
        }).OrderBy(x => x.type).ToList();

        foreach (CharacterFields character in characterList)
        {
            characterDictionary.Add(character.type, character);
        }
    }

    //debug function
    private void PrintCharacters()
    {
        foreach (CharacterFields val in characterDictionary.Values)
        {
            Debug.Log(val.type + " " + val.vitality + " " + val.strength + " " + val.agility + " " + val.weight);
        }
    }
}
