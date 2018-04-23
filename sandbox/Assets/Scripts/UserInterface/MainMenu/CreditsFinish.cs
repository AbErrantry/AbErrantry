using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsFinish : MonoBehaviour
{
	private FMOD.Studio.EventInstance creditsSong;

	private void Start()
	{
		creditsSong = FMODUnity.RuntimeManager.CreateInstance("event:/Music/area/village");
		creditsSong.setVolume(PlayerPrefs.GetFloat("DialogueVolume") * PlayerPrefs.GetFloat("MasterVolume"));
		creditsSong.start();
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	private void OnDestroy()
	{
		creditsSong.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}
