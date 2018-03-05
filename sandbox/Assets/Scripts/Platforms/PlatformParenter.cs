using System.Collections.Generic;
using UnityEngine;

public class PlatformParenter : MonoBehaviour
{
	private Platform platform;
	private void Start()
	{
		platform = gameObject.transform.root.GetComponent<Platform>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character2D.Attackable>() != null)
		{
			platform.children.Add(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Character2D.Attackable>() != null)
		{
			platform.children.Remove(other.gameObject);
		}
	}
}
