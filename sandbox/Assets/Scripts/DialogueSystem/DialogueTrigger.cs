using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; //reference to the DialogueManager

    //triggers the start of a dialogue
    public void TriggerDialogueStart()
    {
        dialogueManager.StartDialogue();
    }

    //triggers the next segment in a dialogue
    public void TriggerNextSegment()
    {
        dialogueManager.GetNextSegment();
    }

    //triggers a choice in a dialogue
    public void TriggerChoice(int Choice)
    {
        dialogueManager.SubmitChoice(Choice);
    }
}
