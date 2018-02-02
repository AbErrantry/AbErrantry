using UnityEngine;

//encapsulates all data objects into a single object
public class GameData : MonoBehaviour
{
    public static GameData data;

    public DialogueData dialogueData;
    public CharacterData characterData;
    public ItemData itemData;
    public SaveData saveData;
    public QuestData questData;
    public ConfigData configData;

    private void Awake()
    {
        if(data == null)
        {
            data = this;
            dialogueData = ScriptableObject.CreateInstance<DialogueData>();
            characterData = ScriptableObject.CreateInstance<CharacterData>();
            itemData = ScriptableObject.CreateInstance<ItemData>();
            saveData = ScriptableObject.CreateInstance<SaveData>();
            questData = ScriptableObject.CreateInstance<QuestData>();
            configData = ScriptableObject.CreateInstance<ConfigData>();
        }
        else if(data != this)
        {
            Destroy(gameObject);
        }
    }
}
