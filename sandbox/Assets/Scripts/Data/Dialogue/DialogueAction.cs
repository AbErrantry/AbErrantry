namespace Dialogue2D
{
	public enum ActionTypes
	{
		None, OpenShopMenu, BecomeHostile, Disappear, RequestGold, RequestItem, GiveItem, 
		GiveGold, ProgressDialogue, ProgressQuest, TransportLocation, TransportLevel, TakeGold
	};

	public class DialogueAction
	{
		public ActionTypes type;
		public string name;
		public int number;
		public float xloc;
		public float yloc; 
	}
}