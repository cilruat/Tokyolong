using UnityEngine;
using System;

namespace WhackAMole.Types
{
	/// <summary>
	/// This script defines a level in the game. When the player reaches a certain score, the level is increased and the difficulty is changed accordingly
	/// This class is used in the Game Controller script
	/// </summary>
	[Serializable]
	public class Level
	{
        [Tooltip("The score needed to win this level")]
        public int scoreToNextLevel = 200;

        [Tooltip("The time bonus for finishing this level")]
        public float timeBonus = 5;

        [Tooltip("The maximum number of targets allowed on screen at once")]
        public int maximumTargets = 2;
	}
}