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
		text = text.Replace("{crouch}", GetKeyName.Input("move_Crouch", KeyTarget.PositivePrimary));
		text = text.Replace("{backpack}", GetKeyName.Input("Backpack", KeyTarget.PositivePrimary));
		text = text.Replace("{pause}", GetKeyName.Input("Pause", KeyTarget.PositivePrimary));
		text = text.Replace("{run}", GetKeyName.Input("move_Run", KeyTarget.PositivePrimary));
		text = text.Replace("{attack}", GetKeyName.Input("Attack", KeyTarget.PositivePrimary));

		if (PlayerPrefs.GetString("ControllerType") == "Dualshock 4")
		{
			text = text.Replace("JoystickButton0", "[Square]");
			text = text.Replace("JoystickButton1", "[X]");
			text = text.Replace("JoystickButton2", "[Circle]");
			text = text.Replace("JoystickButton3", "[Triangle]");
			text = text.Replace("JoystickButton4", "[L1]");
			text = text.Replace("JoystickButton5", "[R1]");
			text = text.Replace("JoystickButton6", "[L2]");
			text = text.Replace("JoystickButton7", "[R2]");
			text = text.Replace("JoystickButton8", "[Share]");
			text = text.Replace("JoystickButton9", "[Options]");
			text = text.Replace("JoystickButton10", "[L3]");
			text = text.Replace("JoystickButton11", "[R3]");
			text = text.Replace("JoystickButton12", "[PS]");
			text = text.Replace("JoystickButton12", "[PadPress]");
			text = text.Replace("{horizontal}", "left and right DPad");
			text = text.Replace("{vertical}", "up and down DPad");
		}
		else if (PlayerPrefs.GetString("ControllerType") == "Xbox Controller")
		{
			text = text.Replace("JoystickButton0", "[A]");
			text = text.Replace("JoystickButton1", "[B]");
			text = text.Replace("JoystickButton2", "[X]");
			text = text.Replace("JoystickButton3", "[Y]");
			text = text.Replace("JoystickButton4", "[LB]");
			text = text.Replace("JoystickButton5", "[RB]");
			text = text.Replace("JoystickButton6", "[Back]");
			text = text.Replace("JoystickButton7", "[Start]");
			text = text.Replace("JoystickButton8", "[LS]");
			text = text.Replace("JoystickButton9", "[RS]");
			text = text.Replace("{horizontal}", "left and right DPad");
			text = text.Replace("{vertical}", "up and down DPad");
		}
		else if (PlayerPrefs.GetString("ControllerType") == "Keyboard")
		{
			text = text.Replace("{horizontal}", GetKeyName.Input("move_Horizontal", KeyTarget.PositivePrimary) + " and " + GetKeyName.Input("move_Horizontal", KeyTarget.NegativePrimary));
			text = text.Replace("{vertical}", GetKeyName.Input("move_Vertical", KeyTarget.PositivePrimary) + " and " + GetKeyName.Input("move_Vertical", KeyTarget.NegativePrimary));
		}

		return text;
	}
}
