using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//encapsulates all data objects into a single object
public class GameData : MonoBehaviour
{
    public DialogueData dialogueData = new DialogueData();
    public CharacterData characterData = new CharacterData();
    public ItemData itemData = new ItemData();
    public PlayerState playerState = new PlayerState();
}
