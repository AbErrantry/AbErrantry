using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	public PlayerInfoTuple playerInfo;

	// Use this for initialization
	void Start()
	{
		playerInfo = new PlayerInfoTuple();
	}
}
