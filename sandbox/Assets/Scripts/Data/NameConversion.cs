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
		text = text.Replace("{jump}", GetKeyName.Input("move_Jump", KeyTarget.PositivePrimary));
		text = text.Replace("{horizontal}", GetKeyName.Input("move_Horizontal", KeyTarget.PositivePrimary));
		text = text.Replace("{vertical}", GetKeyName.Input("move_Vertical", KeyTarget.PositivePrimary));
		text = text.Replace("{crouch}", GetKeyName.Input("move_Crouch", KeyTarget.PositivePrimary));
		text = text.Replace("{backpack}", GetKeyName.Input("Backpack", KeyTarget.PositivePrimary));
		text = text.Replace("{run}", GetKeyName.Input("move_Run", KeyTarget.PositivePrimary));
		text = text.Replace("{attack}", GetKeyName.Input("Attack", KeyTarget.PositivePrimary));
		return text;
	}
}
