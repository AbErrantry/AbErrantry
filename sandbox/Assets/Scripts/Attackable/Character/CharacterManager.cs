using System.Collections;
using System.Collections.Generic;
using Character2D;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	public static CharacterManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void SpawnCharacter(CharacterInfoTuple tuple)
	{
		var character = Resources.Load<GameObject>("Characters/" + tuple.name);

		//instantiate a prefab for the character
		var newCharacter = Instantiate(character) as GameObject;
		var npc = newCharacter.GetComponent<NPC>();

		//set the properties for the character
		npc.name = tuple.name;
		npc.gold = tuple.gold;
		npc.currentSceneName = tuple.level;
		npc.currentDialogueState = tuple.conversation;

		newCharacter.transform.position = new Vector3(tuple.xLoc, tuple.yLoc, 0.0f);

		//for some reason Unity does not use full scale for the instantiated object by default
		newCharacter.transform.localScale = Vector3.one;
	}
}
