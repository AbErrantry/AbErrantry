using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusToElement : MonoBehaviour
{
	public Dialogue2D.DialogueManager dialogueManager;

	public void FocusOnChoice()
	{
		dialogueManager.FocusOnChoice();
	}
}
