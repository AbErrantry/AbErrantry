using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeyName
{
	public static string Input(string uniqueName, KeyTarget keyTarget)
	{
		return hInput.DetailsFromKey(uniqueName, keyTarget).ToString();
	}
}
