using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keybindings : MonoBehaviour {

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI up, left, down, right, jump, run, attack, interact, pause;

    private GameObject currentKey;

	// Use this for initialization
	void Start ()
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

        up.text = keys["MoveUp"].ToString();
        left.text = keys["MoveLeft"].ToString();
        down.text = keys["MoveDown"].ToString();
        right.text = keys["MoveRight"].ToString();
        jump.text = keys["Jump"].ToString();
        run.text = keys["Run"].ToString();
        attack.text = keys["Attack"].ToString();
        interact.text = keys["Interact"].ToString();
        pause.text = keys["Pause"].ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(keys["MoveUp"]))
        {

        }
        if (Input.GetKeyDown(keys["MoveLeft"]))
        {

        }
        if (Input.GetKeyDown(keys["MoveDown"]))
        {

        }
        if (Input.GetKeyDown(keys["MoveRight"]))
        {

        }
        if (Input.GetKeyDown(keys["Jump"]))
        {

        }
        if (Input.GetKeyDown(keys["Run"]))
        {

        }
        if (Input.GetKeyDown(keys["Attack"]))
        {

        }
        if (Input.GetKeyDown(keys["Interact"]))
        {

        }
        if (Input.GetKeyDown(keys["Pause"]))
        {

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

    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }

    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }
}
