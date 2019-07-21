using UnityEngine;
using System;

namespace SpeederRunGame.Types
{
	/// <summary>
	/// This class defines an object that can be spawned, and its chance of apperance.
	/// </summary>
	[Serializable]
	public class Spawn
	{
		[Tooltip("The object to be spawned")]
		public Transform spawnObject;

		[Tooltip("How often the object appears. The minimum value is 1")]
		public int spawnChance = 1;

		//[Tooltip("At which level in the game will these objects start spawning? The level is set in the game controller")]
		//public int spawnAtLevel = 1;
	}
}