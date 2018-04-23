using System.Collections;
using System.Collections.Generic;
using System.IO;
using HardShellStudios.CompleteControl;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigSettings : MonoBehaviour
{
	public Slider masterVolume;
	public Slider musicVolume;
	public Slider sfxVolume;
	public Slider dialogueVolume;

	public TMP_Dropdown screenMode;
	public TMP_Dropdown resolution;
	public TMP_Dropdown dialogueTextSpeed;
	public TMP_Dropdown colorblindMode;

	public TMP_Dropdown controllerType;
	public TMP_Dropdown gamesToLoad;

	public Button loadGameButton;

	public CanvasGroup axisGroup;

	private List<string> saveFileNames;

	public TMP_InputField newSaveFile;
	public TMP_Text errorMessage;

	private bool isSavingPrince;

	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.HasKey("MasterVolume"))
		{
			masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
		}
		else
		{
			PlayerPrefs.SetFloat("MasterVolume", 0.5f);
			masterVolume.value = 0.5f;
		}

		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
		}
		else
		{
			PlayerPrefs.SetFloat("MusicVolume", 0.5f);
			musicVolume.value = 0.5f;
		}

		if (PlayerPrefs.HasKey("SfxVolume"))
		{
			sfxVolume.value = PlayerPrefs.GetFloat("SfxVolume");
		}
		else
		{
			PlayerPrefs.SetFloat("SfxVolume", 0.5f);
			sfxVolume.value = 0.5f;
		}

		if (PlayerPrefs.HasKey("DialogueVolume"))
		{
			dialogueVolume.value = PlayerPrefs.GetFloat("DialogueVolume");
		}
		else
		{
			PlayerPrefs.SetFloat("DialogueVolume", 0.5f);
			dialogueVolume.value = 0.5f;
		}

		if (PlayerPrefs.HasKey("FullScreen"))
		{
			Screen.fullScreen = IntToBool(PlayerPrefs.GetInt("FullScreen"));
			if (Screen.fullScreen)
			{
				screenMode.captionText.text = "Fullscreen";
			}
			else
			{
				screenMode.captionText.text = "Windowed";
			}
		}
		else
		{
			PlayerPrefs.SetInt("FullScreen", BoolToInt(Screen.fullScreen));
		}

		if (PlayerPrefs.HasKey("Resolution"))
		{
			SetResolutionFromPrefs(PlayerPrefs.GetString("Resolution"));
		}
		else
		{
			SetResolutionFromPrefs(Screen.currentResolution.ToString());
		}
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
			data.text = Screen.resolutions[i].ToString();
			resolution.options.Add(data);
		}
		resolution.captionText.text = Screen.currentResolution.ToString();

		if (PlayerPrefs.HasKey("TextSpeed"))
		{
			dialogueTextSpeed.captionText.text = PlayerPrefs.GetString("TextSpeed");
		}
		else
		{
			PlayerPrefs.SetString("TextSpeed", "Normal");
			dialogueTextSpeed.captionText.text = "Normal";
		}

		if (PlayerPrefs.HasKey("ColorblindMode"))
		{
			colorblindMode.captionText.text = colorblindMode.options[PlayerPrefs.GetInt("ColorblindMode")].text;
		}
		else
		{
			PlayerPrefs.SetInt("ColorblindMode", 0);
			colorblindMode.captionText.text = colorblindMode.options[0].text;
		}

		if (PlayerPrefs.HasKey("ControllerType"))
		{
			controllerType.captionText.text = PlayerPrefs.GetString("ControllerType");
			if (controllerType.captionText.text == "Keyboard")
			{
				axisGroup.interactable = true;
			}
			else
			{
				axisGroup.interactable = false;
			}
		}
		else
		{
			controllerType.captionText.text = "Keyboard";
			SetControllerType();
		}

		List<string> files = new List<string>(Directory.GetFiles(Application.streamingAssetsPath + "/Saves/"));
		saveFileNames = new List<string>();
		foreach (string file in files)
		{
			if (file.EndsWith(".db"))
			{
				TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
				data.text = Path.GetFileNameWithoutExtension(file);
				gamesToLoad.options.Add(data);
				saveFileNames.Add(data.text);
			}
		}
		if (gamesToLoad.options.Count > 0)
		{
			gamesToLoad.captionText.text = gamesToLoad.options[0].text;
		}
		else
		{
			loadGameButton.interactable = false;
		}

		CharacterSaveChange(true);
	}

	public void SetMasterVolume()
	{
		PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
	}

	public void SetMusicVolume()
	{
		PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
	}

	public void SetSfxVolume()
	{
		PlayerPrefs.SetFloat("SfxVolume", sfxVolume.value);
	}

	public void SetDialogueVolume()
	{
		PlayerPrefs.SetFloat("DialogueVolume", dialogueVolume.value);
	}

	public void SetScreenMode()
	{
		if (screenMode.captionText.text == "Fullscreen")
		{
			PlayerPrefs.SetInt("FullScreen", 1);
			Screen.fullScreen = true;
		}
		else
		{
			PlayerPrefs.SetInt("FullScreen", 0);
			Screen.fullScreen = false;
		}
	}

	public void SetResolution()
	{
		foreach (var res in Screen.resolutions)
		{
			if (res.ToString() == resolution.captionText.text)
			{
				Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
				PlayerPrefs.SetString("Resolution", res.ToString());
			}
		}
	}

	public void SetResolutionFromPrefs(string resolution)
	{
		foreach (var res in Screen.resolutions)
		{
			if (res.ToString() == resolution)
			{
				Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
				PlayerPrefs.SetString("Resolution", res.ToString());
			}
		}
	}

	public void SetTextSpeed()
	{
		PlayerPrefs.SetString("TextSpeed", dialogueTextSpeed.captionText.text);
	}

	public void SetColorblindMode()
	{
		PlayerPrefs.SetInt("ColorblindMode", colorblindMode.value);
	}

	public void SetControllerType()
	{
		PlayerPrefs.SetString("ControllerType", controllerType.captionText.text);

		if (controllerType.captionText.text == "Keyboard")
		{
			hInput.SetKey("move_Horizontal", KeyCode.D, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Horizontal", KeyCode.A, KeyTarget.NegativePrimary);
			hInput.SetKey("move_Vertical", KeyCode.W, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Vertical", KeyCode.S, KeyTarget.NegativePrimary);
			hInput.SetKey("move_Run", KeyCode.LeftShift, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Crouch", KeyCode.LeftControl, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Jump", KeyCode.Space, KeyTarget.PositivePrimary);
			hInput.SetKey("Attack", KeyCode.E, KeyTarget.PositivePrimary);
			hInput.SetKey("Interact", KeyCode.Q, KeyTarget.PositivePrimary);
			hInput.SetKey("Backpack", KeyCode.R, KeyTarget.PositivePrimary);
			hInput.SetKey("Pause", KeyCode.Tab, KeyTarget.PositivePrimary);

			axisGroup.interactable = true;
		}
		else if (controllerType.captionText.text == "Dualshock 4")
		{
			hInput.SetKey("move_Horizontal_controller", AxisCode.Axis7);
			hInput.SetKey("move_Vertical_controller", AxisCode.Axis8);
			hInput.SetKey("move_Run", KeyCode.JoystickButton4, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Crouch", KeyCode.JoystickButton2, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Jump", KeyCode.JoystickButton1, KeyTarget.PositivePrimary);
			hInput.SetKey("Attack", KeyCode.JoystickButton5, KeyTarget.PositivePrimary);
			hInput.SetKey("Interact", KeyCode.JoystickButton0, KeyTarget.PositivePrimary);
			hInput.SetKey("Backpack", KeyCode.JoystickButton3, KeyTarget.PositivePrimary);
			hInput.SetKey("Pause", KeyCode.JoystickButton9, KeyTarget.PositivePrimary);

			axisGroup.interactable = false;
		}
		else if (controllerType.captionText.text == "Xbox Controller")
		{
			hInput.SetKey("move_Horizontal_controller", AxisCode.Axis6);
			hInput.SetKey("move_Vertical_controller", AxisCode.Axis7);
			hInput.SetKey("move_Run", KeyCode.JoystickButton4, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Crouch", KeyCode.JoystickButton1, KeyTarget.PositivePrimary);
			hInput.SetKey("move_Jump", KeyCode.JoystickButton0, KeyTarget.PositivePrimary);
			hInput.SetKey("Attack", KeyCode.JoystickButton5, KeyTarget.PositivePrimary);
			hInput.SetKey("Interact", KeyCode.JoystickButton2, KeyTarget.PositivePrimary);
			hInput.SetKey("Backpack", KeyCode.JoystickButton3, KeyTarget.PositivePrimary);
			hInput.SetKey("Pause", KeyCode.JoystickButton7, KeyTarget.PositivePrimary);

			axisGroup.interactable = false;
		}
	}

	public void ValidateSaveFileName()
	{
		var isValid = !string.IsNullOrEmpty(newSaveFile.text) &&
			newSaveFile.text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
			!File.Exists(Path.Combine(Application.streamingAssetsPath + "/Saves/", newSaveFile.text));
		if (!isValid)
		{
			errorMessage.text = "Error: save file is invalid";
			newSaveFile.text = "SaveData";
			while (saveFileNames.Contains(newSaveFile.text))
			{
				newSaveFile.text = newSaveFile.text + "1";
			}
		}
		else
		{
			errorMessage.text = "";
		}
	}

	public void CharacterSaveChange(bool isPrince)
	{
		isSavingPrince = isPrince;
		PlayerPrefs.SetInt("IsSavingPrince", BoolToInt(isSavingPrince));
	}

	public void StartNewGame()
	{
		if (newSaveFile.text == "")
		{
			newSaveFile.text = "SaveData1";
			ValidateSaveFileName();
		}

		//create new save file
		PlayerPrefs.SetString("CurrentSave", newSaveFile.text + ".db");
		PlayerPrefs.SetInt("IsNewFile", 1);

		if (File.Exists(Application.streamingAssetsPath + "/" + newSaveFile.text + "-Log.txt"))
		{
			File.Delete(Application.streamingAssetsPath + "/" + newSaveFile.text + "-Log.txt");
		}

		string source = Application.streamingAssetsPath + "/Default/TestDatabase.db";
		string dest = Application.streamingAssetsPath + "/Saves/" + newSaveFile.text + ".db";

		File.Copy(source, dest, true);

		SceneManager.LoadScene("Persistent-SC");
	}

	public void LoadGame()
	{
		PlayerPrefs.SetString("CurrentSave", gamesToLoad.captionText.text + ".db");
		PlayerPrefs.SetInt("IsNewFile", 0);
		SceneManager.LoadScene("Persistent-SC");
	}

	public void Reset()
	{
		BackgroundSwitchMenu.instance.ResetSong();
		SceneManager.LoadScene("MainMenu");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Credits()
	{
		BackgroundSwitchMenu.instance.ResetSong();
		SceneManager.LoadScene("Credits");
	}

	private int BoolToInt(bool val)
	{
		if (val)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}

	private bool IntToBool(int val)
	{
		if (val == 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
