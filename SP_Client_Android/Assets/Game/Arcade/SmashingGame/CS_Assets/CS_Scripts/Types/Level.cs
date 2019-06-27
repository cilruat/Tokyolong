using UnityEngine;
using System;

namespace ObjectSmashingGame.Types
{
	/// <summary>
	/// This script defines a level in the game. When the player reaches a certain score, the level is increased and the difficulty is changed accordingly
	/// This class is used in the Game Controller script
	/// </summary>
	[Serializable]
	public class Level
	{
        [Tooltip("The number of rows in this level")]
        public int rowsInLevel = 10;

        [Tooltip("The speed of the game in this level")]
        public float moveSpeed = 1;

        [Tooltip("The maximum number of objects in a row")]
        public int maxObjectsInRow = 1;

        [Tooltip("The chance for an object in a row to be bad")]
        public float badObjectChance = 0.1f;

        //[Tooltip("The final object that spawns, which should be bigger and slower than the rest, but give more score. You can leave this empty if you don't want a big object in this level")]
        //public Transform bigObject;
    }
}