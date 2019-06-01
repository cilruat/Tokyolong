using UnityEngine;
using System;

namespace ZombieDriveGame.Types
{
	/// <summary>
	/// This script defines a spawnable object, with a spawn chance.
	/// </summary>
	[Serializable]
	public class Spawn
	{
        [Tooltip("The object that will be spawned")]
        public Transform spawnObject;

        [Tooltip("The chance for it to be spawned")]
        public int spawnChance = 1;

        [Tooltip("The minimum gap that should be created between this object and the next")]
        public int spawnGap = 0;
    }
}