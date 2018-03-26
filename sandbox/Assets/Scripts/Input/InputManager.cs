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

    	public bool GetButtonDown(string keyMap)
    	{
    		return Input.GetKeyDown(keyMapping[keyMap]);
    	}

		public bool GetButtonUp(string keyMap)
		{
			return Input.GetKeyUp(keyMapping[keyMap]);
		}

		public bool GetButton(string keyMap)
		{
			return Input.GetKey(keyMapping[keyMap]);
		}

		public float GetAxisRaw(string keyMap)
		{
			//Up and down, W and S, A and D, etc.
			//goes from -1 -> 1 depending on button pressed.
			//keyMap-> get tuple
			if(Input.GetKey(keyMapping[keyMap])) // positive value
			{
				return 1.0f;
			}
			else if(Input.GetKey(keyMapping[keyMap])) // negative value
			{
				return -1.0f;
			}
			else
			{
				return 0.0f;
			}
		}
    }
