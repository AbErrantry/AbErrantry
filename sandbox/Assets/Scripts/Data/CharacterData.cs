using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;

public class CharacterData : ScriptableObject
{
    public Dictionary<string, Character> characterDictionary;

    //default constructor
    public CharacterData()
    {
        characterDictionary = new Dictionary<string, Character>();
        GetCharacters();
    }

    //gets each character type into memory from the database
    private void GetCharacters()
    {
        string characterDatabase = "CharacterTypeDatabase.xml";
        List<Character> characterList = new List<Character>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(characterDatabase);
        characterList = (from character in XDoc.Root.Elements("character")
                    select new Character
                    {
                        type = character.AttributeValueNull_String("type"),
                        vitality = character.Element("vitality").ElementValueNull_Float(),
                        strength = character.Element("strength").ElementValueNull_Float(),
                        agility = character.Element("agility").ElementValueNull_Float(),
                        weight = character.Element("weight").ElementValueNull_Float(),
                    }).OrderBy(x => x.type).ToList();

        foreach (Character character in characterList)
        {
            characterDictionary.Add(character.type, character);
        }
    }

    //debug function
    private void PrintCharacters()
    {
        foreach (Character val in characterDictionary.Values)
        {
            Debug.Log(val.type + " " + val.vitality + " " + val.strength + " " + val.agility + " " + val.weight);
        }
    }
}
