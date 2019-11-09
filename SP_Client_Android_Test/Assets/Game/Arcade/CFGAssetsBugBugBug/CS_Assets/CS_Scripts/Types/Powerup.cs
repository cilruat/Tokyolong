using UnityEngine;
using System;

namespace CoinFrenzyGame.Types
{
	[Serializable]
	public class Powerup
	{
		//[Tooltip("The name of this powerup. Has no functionality, it's just used so we know what the powerup is supposed to be.")]
		//public string powerupName = "Double Coins";

		[Tooltip("The name of the function we call when this powerup is activated from the gamecontroller. You can read about available powerup functions in the documentation")]
		public string startFunction = "SetScoreMultiplier";

		[Tooltip("The parameter that is passed along with the function. The value is a float.")]
		public float startParamater = 2;
		
		[Tooltip("The duration of this powerup. After the powerup timer reaches 0, the end functions are called")]
		public float duration = 10;
		internal float durationMax;
		
		[Tooltip("The name of the function we call when this powerup ends. Usually we call the same function as we did at the start, but we pass a parameter that resets its value")]
		public string endFunction = "SetScoreMultiplier";
		public float endParamater = 1;
		
		[Tooltip("The icon from the list of icons in the powerups canvas.")]
		public Transform icon;
	}
}
