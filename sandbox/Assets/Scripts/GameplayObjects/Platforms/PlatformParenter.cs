using System.Collections.Generic;
using UnityEngine;

public class PlatformParenter : MonoBehaviour
{
	private Platform platform;
	private void Start()
	{
		platform = gameObject.transform.parent.GetComponent<Platform>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character2D.Attackable>() != null && other.gameObject.GetComponent<Character2D.AttackableTile>() == null)
		{
			platform.children.Add(other.gameObject);
			if (other.gameObject.GetComponent<Character2D.CharacterMovement>())
			{
				other.gameObject.GetComponent<Character2D.CharacterMovement>().isOnPlatform = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character2D.Attackable>() != null && other.gameObject.GetComponent<Character2D.AttackableTile>() == null)
		{
			platform.children.Remove(other.gameObject);
			if (other.gameObject.GetComponent<Character2D.CharacterMovement>())
			{
				other.gameObject.GetComponent<Character2D.CharacterMovement>().isOnPlatform = false;
			}
		}
	}
}
