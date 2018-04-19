using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Choices : MonoBehaviour
{
	private List<ReflectingChoice> choices;
	private int currentIndex;

	public Image characterImage;
	public TMP_Text choiceText;
	public TMP_Text typeText;
	public TMP_Text pageText;

	public Button nextButton;
	public Button finishButton;

	public TMP_Text resultsHeaderText;
	public TMP_Text resultsBodyText;

	public float textSpeed;

	private void Start()
	{
		currentIndex = 0;
		choices = new List<ReflectingChoice>();
		GetTextSpeed();
		GetChoiceFileAsList();
		GetResultsPage();
	}

	private void GetTextSpeed()
	{
		if (PlayerPrefs.GetString("TextSpeed") == "Fast")
		{
			textSpeed = 1.0f;
		}
		else if (PlayerPrefs.GetString("TextSpeed") == "Normal")
		{
			textSpeed = 2.0f;
		}
		else
		{
			textSpeed = 3.0f;
		}
	}

	private void GetChoiceFileAsList()
	{
		string file = PlayerPrefs.GetString("CurrentSave");
		if (file == null)
		{
			Application.Quit();
		}
		string path = "URI=file:" + Application.streamingAssetsPath + "/Saves/" + file;
		string filename = Path.GetFileNameWithoutExtension(path);
		path = Application.streamingAssetsPath + "/" + filename + "-Log.txt";

		List<string> lines = new List<string>(File.ReadAllLines(path));
		foreach (var line in lines)
		{
			var choice = new ReflectingChoice();
			var lineSplit = line.Split('[', ']');
			choice.karma = int.Parse(lineSplit[1]);
			choice.name = lineSplit[3];
			choice.text = lineSplit[5];
			choices.Add(choice);
		}
	}

	public void GetNextChoice()
	{
		pageText.text = "Page " + (currentIndex + 1) + " out of " + choices.Count;
		nextButton.interactable = false;
		StartCoroutine(TypeSentence(currentIndex));
		if (currentIndex >= choices.Count - 1)
		{
			nextButton.gameObject.SetActive(false);
			finishButton.gameObject.SetActive(true);
		}
		if (choices[currentIndex].karma > 0)
		{
			typeText.text = "<#005500>Positive Choice";
		}
		else if (choices[currentIndex].karma == 0)
		{
			typeText.text = "<#333333>Neutral Choice";
		}
		else
		{
			typeText.text = "<#FF0000>Negative Choice";
		}
		characterImage.sprite = Resources.Load<Sprite>("Pictures/" + choices[currentIndex].name);
		currentIndex++;
	}

	private void GetResultsPage()
	{
		int karmaSum = 0;
		foreach (var choice in choices)
		{
			karmaSum += choice.karma;
		}

		if (karmaSum <= -50)
		{
			resultsHeaderText.text = "<#FF0000>You did not perform well at all...";
			resultsBodyText.text = "For a majority of the choices, you took the wrong approach." +
				" This may be a sign that you are too trusting of individuals." +
				" Be careful who you give personal information to as it may put you in danger.";
		}
		else if (karmaSum <= -25)
		{
			resultsHeaderText.text = "<#FF0000>You did not perform very well.";
			resultsBodyText.text = "For a majority of the choices, you took the wrong approach." +
				" This may be a sign that you are too trusting of individuals." +
				" Be careful who you give personal information to as it may put you in danger.";
		}
		else if (karmaSum <= 0)
		{
			resultsHeaderText.text = "<#333333>You performed below average.";
			resultsBodyText.text = "For some of the choices, you took the wrong approach." +
				" You showed that you understand when to keep personal information private at times," +
				" but there is definitely room for improvement.";
		}
		else if (karmaSum <= 25)
		{
			resultsHeaderText.text = "<#333333>You performed above average.";
			resultsBodyText.text = "For a good amount of the choices, you took the right approach." +
				" You showed that you understand when to keep personal information private at times," +
				" but there is definitely room for improvement.";
		}
		else if (karmaSum <= 50)
		{
			resultsHeaderText.text = "<#005500>You performed very well!";
			resultsBodyText.text = "You have a great understanding of personal privacy." +
				" You were able to handle most situations correctly. Still, you" +
				" need to be aware at all times who you are giving personal information to.";
		}
		else
		{
			resultsHeaderText.text = "<#005500>You performed incredibly!";
			resultsBodyText.text = "You handled all situations almost flawlessly!" +
				" Keep this in mind whenever you talk to strangers. You" +
				" need to be aware at all times who you are giving personal information to.";
		}
	}

	public void FinishButtonClick()
	{
		BackgroundSwitchMenu.instance.ResetSong();
		SceneManager.LoadScene("Credits");
	}

	//coroutine that animates the text on screen character by character
	private IEnumerator TypeSentence(int index)
	{
		choiceText.text = "";
		foreach (var letter in choices[index].text.ToCharArray())
		{
			choiceText.text = choiceText.text + letter;
			yield return new WaitForSeconds(0.01f * textSpeed);
		}
		yield return new WaitForSeconds(0.5f);
		nextButton.interactable = true;
	}

}
