using UnityEngine;
using System;

namespace CoinFrenzyGame.Types
{
	/// <summary>
	/// This script defines a level in the game. When the player earns enough points, the level is increased and the difficulty is changed accordingly
	/// This class is used in the Game Controller script
	/// </summary>
	[Serializable]
	public class Level
	{
		[Tooltip("The score needed to win this level")]
		public int scoreToNextLevel = 200;

		[Tooltip("The maximum number of enemies in this level")]
		public int enemyLimit = 1;
	}
}