using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : Interactable
{
    public int currentDialogueState;
    public string currentSceneName;
    public int gold;
    public bool changesDirection = true;

    public bool isFacingRight;

    public static event Action<CharacterInfoTuple> OnCharacterInfoChanged;

    private FMOD.Studio.EventInstance progressionNoise;
    private FMOD.Studio.EventInstance hostileNoise;

    //used for initialization
    private new void Start()
    {
        typeOfInteractable = Types.NPC;
        base.Start();

        isFacingRight = true;
        FaceRight(true);

        progressionNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/progression_jingle");
        progressionNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

        hostileNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/become_hostile");
        hostileNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
    }

    public void FaceRight(bool faceRight)
    {
        if (changesDirection)
        {
            if (faceRight)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                isFacingRight = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = false;
            }
        }
    }

    public void SetDialogueState(int state)
    {
        currentDialogueState = state;
        CharacterInfoChanged(currentSceneName, transform.position.x, transform.position.y);
        progressionNoise.start();
    }

    public void SetGold(int delta)
    {
        gold += delta;
        CharacterInfoChanged(currentSceneName, transform.position.x, transform.position.y);
    }

    public void CharacterInfoChanged(string levelName, float xLoc, float yLoc)
    {
        var tuple = new CharacterInfoTuple();
        tuple.name = name;
        tuple.gold = gold;
        tuple.xLoc = xLoc;
        tuple.yLoc = yLoc;
        tuple.conversation = currentDialogueState;
        tuple.level = levelName;
        OnCharacterInfoChanged(tuple);
    }

    public void MakeHostile()
    {
        var dormantCharacter = gameObject.GetComponent<Character2D.DormantCharacter>();
        if (dormantCharacter != null)
        {
            hostileNoise.start();
            dormantCharacter.BecomeHostile();
            RemoveReference();
            EventDisplay.instance.AddEvent(name + " became hostile!");
            Destroy(this);
        }
        else
        {
            Debug.LogError("NPC cannot become hostile because it does not have an enemy component.");
        }
    }

    private void RemoveReference()
    {
        GameData.data.saveData.DeleteCharacter(name);
    }

    public void Disappear()
    {
        RemoveReference();
    }
}
