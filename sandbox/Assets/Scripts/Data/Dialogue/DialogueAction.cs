namespace Dialogue2D
{
	public enum ActionTypes
	{
		None, OpenShopMenu, BecomeHostile, AdvanceQuest, Disappear, RequestGold, RequestItem, GiveItem, GiveGold
	};

	public class DialogueAction
	{
		public ActionTypes type;
		public string name;
		public int amount;
	}
}