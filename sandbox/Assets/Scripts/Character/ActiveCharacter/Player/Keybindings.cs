using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HardShellStudios.CompleteControl;

[AddComponentMenu("Hard Shell Studios/Complete Control/UI Rebind Button")]
[RequireComponent(typeof(Button))]
public class Keybindings : MonoBehaviour
{

    public string uniqueName;
    public KeyTarget keyTarget = KeyTarget.PositivePrimary;
    public TextMeshProUGUI text;
    public bool constantUpdate = false;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
	public TextMeshProUGUI up, left, down, right, jump, run, attack, interact, pause, backpack;
	private GameObject currentKey;

    private string originalString;
    private bool isBinding = false;
    private Button button;
    private bool textSettled = false;

    private void Start()
    {
		keys.Add("MoveUp",(KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveUp", "W")));
		keys.Add("MoveLeft", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft", "A")));
		keys.Add("MoveDown", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveDown", "S")));
		keys.Add("MoveRight", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight", "D")));
		keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
		keys.Add("Run", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run", "LeftShift")));
		keys.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack", "Mouse0")));
		keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "Q")));
		keys.Add("Pause", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", "Escape")));
		keys.Add ("Backpack", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Backpack", "B")));

		up.text = keys["MoveUp"].ToString();
		left.text = keys["MoveLeft"].ToString();
		down.text = keys["MoveDown"].ToString();
		right.text = keys["MoveRight"].ToString();
		jump.text = keys["Jump"].ToString();
		run.text = keys["Run"].ToString();
		attack.text = keys["Attack"].ToString();
		interact.text = keys["Interact"].ToString();
		pause.text = keys ["Pause"].ToString ();
		backpack.text = keys ["Backpack"].ToString ();
	
        originalString = text.text;
        button = GetComponent<Button>();
        button.onClick.AddListener(RebindKey);
    }

    private void Update()
    {
        if (isBinding)
        {
            KeyCode key = hInput.CurrentKeyDown();
            if (key != KeyCode.None)
            {
                if (key == hInput.RebindRemovalKey)
                    hInput.SetKey(uniqueName, KeyCode.None, keyTarget);
                else
                    hInput.SetKey(uniqueName, key, keyTarget);

                isBinding = false;
                button.interactable = true;
            }
        }
        else
        {
            if (!textSettled || constantUpdate)
            {
                if (originalString.Contains("{key}") || originalString.Contains("{name}"))
                {
                    text.text = originalString.Replace("{key}", hInput.DetailsFromKey(uniqueName, keyTarget).ToString());
                }
                else
                    text.text = originalString;
            }
        }
    }

	void OnGUI()
	{
		if (currentKey != null)
		{
			Event e = Event.current;
			if (e.isKey)
			{
				keys[currentKey.name] = e.keyCode;
				currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
				currentKey = null;
			}
		}
	}

    public void RebindKey()
    {
        text.text = "PRESS ANY KEY";
        textSettled = false;
        isBinding = true;
        button.interactable = false;
    }
}
