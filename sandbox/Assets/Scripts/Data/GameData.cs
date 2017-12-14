using UnityEngine;

//encapsulates all data objects into a single object
public class GameData : MonoBehaviour
{
    public DialogueData dialogueData;
    public CharacterData characterData;
    public ItemData itemData;
    public SaveData saveData;
    public ConfigData configData;

    private void Start()
    {
        dialogueData = ScriptableObject.CreateInstance<DialogueData>();
        characterData = ScriptableObject.CreateInstance<CharacterData>();
        itemData = ScriptableObject.CreateInstance<ItemData>();
        saveData = ScriptableObject.CreateInstance<SaveData>();
        configData = ScriptableObject.CreateInstance<ConfigData>();
    }
}
