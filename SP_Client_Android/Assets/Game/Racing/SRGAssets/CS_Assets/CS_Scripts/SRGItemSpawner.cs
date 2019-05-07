using UnityEngine;
using SpeederRunGame.Types;

namespace SpeederRunGame
{
	/// <summary>
	/// This script spawns an item at the position of this object. It's used to allow us to edit one prefab instead of having to edit each object in each sections individually
	/// </summary>
	public class SRGItemSpawner:MonoBehaviour
	{
		// The current item that was spawned
		internal Transform currentItem;

		[Tooltip("The item list that can be spawned here. We use this method because now we can edit one item in the project and it will replace all the items in all sections without having to edit each one")]
		public Transform[] itemsToSpawn;

		void Start()
		{
			// If we have an item assigned, spawn it at the position of this object
			if ( itemsToSpawn.Length > 0 )    
			{
				// Spawn one of the items randomly
				currentItem = Instantiate( itemsToSpawn[Mathf.FloorToInt(Random.Range(0,itemsToSpawn.Length))], transform.position, Quaternion.identity) as Transform;

				// Set the parent of the item to be this spawner
				currentItem.SetParent(transform);
			}
		}
	}
}