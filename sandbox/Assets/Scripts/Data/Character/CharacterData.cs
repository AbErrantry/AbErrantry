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
        //PrintCharacters();
    }

    //gets each character type into memory from the database
    private void GetCharacters()
    {
        string characterDatabase = Application.streamingAssetsPath + "/CharacterDatabase.xml";
        List<CharacterFields> characterList = new List<CharacterFields>();

        //parse XML with LINQ
        XDocument XDoc = XDocument.Load(characterDatabase);
        characterList = (from character in XDoc.Root.Elements("character") select new CharacterFields
        {
            type = character.AttributeValueNull_String("type"),
                vitality = character.Element("vitality").ElementValueNull_Integer(),
                strength = character.Element("strength").ElementValueNull_Integer(),
                agility = character.Element("agility").ElementValueNull_Float(),
                weight = character.Element("weight").ElementValueNull_Float(),
                attacks = character.Elements("attack")
                .Select(attack => new CharacterAttackInfo
                {
                    id = attack.AttributeValueNull_Integer("id"),
                        damage = attack.Element("damage").ElementValueNull_Integer(),
                        oddsThreshold = attack.Element("oddsThreshold").ElementValueNull_Float(),
                        attackTime = attack.Element("attackTime").ElementValueNull_Float(),
                        windupTime = attack.Element("windupTime").ElementValueNull_Float(),
                }).OrderBy(x => x.oddsThreshold).ToList(),
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
            foreach (CharacterAttackInfo atk in val.attacks)
            {
                Debug.Log("     -> " + atk.id + " " + atk.damage + " " + atk.oddsThreshold + " " + atk.attackTime + " " + atk.windupTime);
            }
        }
    }
}
