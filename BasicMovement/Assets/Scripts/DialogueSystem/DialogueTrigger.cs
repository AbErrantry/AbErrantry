using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public void TriggerDialogueStart()
    {
        FindObjectOfType<DialogueManager>().StartDialogue();
    }

    public void TriggerNextSegment()
    {
        FindObjectOfType<DialogueManager>().GetNextSegment();
    }

    public void TriggerChoice(int Choice)
    {
        FindObjectOfType<DialogueManager>().SubmitChoice(Choice);
    }
}
