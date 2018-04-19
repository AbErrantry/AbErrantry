using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoicesLoad : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Character2D.Player>() != null)
		{
			BackgroundSwitch.instance.ResetSongs();
			SceneManager.LoadScene("Choices");
		}
	}
}
