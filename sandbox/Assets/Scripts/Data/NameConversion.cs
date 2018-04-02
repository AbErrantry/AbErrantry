using Character2D;
using UnityEngine;

public class NameConversion
{
	public static string ConvertSymbol(string text)
	{
		if (Player.instance.isSavingPrincess)
		{
			text = text.Replace("@", "Princess");
			text = text.Replace("#", "she");
			text = text.Replace("%", "daughter");
			text = text.Replace("$", "her");
		}
		else
		{
			text = text.Replace("@", "Prince");
			text = text.Replace("#", "he");
			text = text.Replace("%", "son");
			text = text.Replace("$", "him");
		}
		return text;
	}
}
