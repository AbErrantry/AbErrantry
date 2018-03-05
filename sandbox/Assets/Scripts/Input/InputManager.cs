    using System.Collections.Generic;
    using System.Collections;
    using System;
    using UnityEngine;

    public class InputManager
    {
    	public Dictionary<string, KeyCode> keyMapping;

    	private List<string> keyMaps = new List<string>
    	{
    		"Attack",
    		"Block",
    		"Forward",
    		"Backward",
    		"Left",
    		"Right"
    	};

    	private List<KeyCode> defaults = new List<KeyCode>
    	{
    		KeyCode.Q,
    		KeyCode.E,
    		KeyCode.W,
    		KeyCode.S,
    		KeyCode.A,
    		KeyCode.D
    	};

    	public InputManager()
    	{
    		InitializeDictionary();
    	}

    	private void InitializeDictionary()
    	{
    		keyMapping = new Dictionary<string, KeyCode>();
    		for (int index = 0; index < keyMaps.Count; index++)
    		{
    			keyMapping.Add(keyMaps[index], defaults[index]);
    		}
    	}

    	public void SetKeyMap(string keyMap, KeyCode key)
    	{
    		if (!keyMapping.ContainsKey(keyMap))
    		{
    			throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
    		}
    		keyMapping[keyMap] = key;
    	}

    	public bool GetKeyDown(string keyMap)
    	{
    		return Input.GetKeyDown(keyMapping[keyMap]);
    	}

    	public bool GetButtonDown(string keyMap)
    	{
    		//if controller,
    		return Input.GetKeyDown(keyMapping[keyMap]);
    	}

    	public bool GetButton(string keyMap)
    	{
    		return Input.GetKeyDown(keyMapping[keyMap]);
    	}

    	public bool GetButtonUp(string keyMap)
    	{
    		return Input.GetKeyDown(keyMapping[keyMap]);
    	}

    }
