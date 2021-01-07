namespace GameBench
{
	using UnityEngine;

	public class Config : MonoBehaviour
	{
		[Range (1, 30)]
		public int speed;
		public int spinTurnCost = 300;
		public float spinDurationMin = 10.0f, spinDurationMax = 15.0f;
		public bool freeTurn, paidTurn;
		public RewardItem[] rewarItem = new RewardItem[8];
		public UITheme[] themes;
		public int selectedTheme;
		public Sprite[] rewardSprites, pointSprite;
		private static Config instance;

		public static Config Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<Config> ();
				}

				return instance;
			}
		}

		public UITheme CurrentTheme {
			get { return Instance.themes [selectedTheme]; }
		}
	}

	public enum WheelTheme
	{
		Theme1,
		Theme2,
		Theme3,
		Theme4,
		Theme5
	}

	[System.Serializable]
	public class RewardItem
	{
		public Color itemBgColor = new Color (1, 1, 1, 1);
		//public Sprite rewardSprite;
		public int amountOrID;
		public RewardType rewardType;

		public Sprite rewardSp {
			get { return Config.Instance.rewardSprites [(int)rewardType]; }
		}
	}


	[System.Serializable]
	public class UITheme
	{
		public Sprite borderSprite, backButonBg, addMoreButtonBg, arrowSprite, spinButtonBg;
	}

	public enum RewardType
	{
		Coin = 0,
		//		Diamond,
		Ticket,
		Car,
		Tire
	}
}