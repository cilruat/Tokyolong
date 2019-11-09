using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// This script defines an item, which spawns within the game area and can be picked up by the player. 
	/// </summary>
	public class SRGSection:MonoBehaviour
	{
		internal Transform thisTransform;

		[Tooltip("A list of all the walls in this section. The walls are")]
		public Transform[] walls;

		[Tooltip("The number of hidden walls")]
		public int hiddenWalls = 7;

		[Tooltip("A random rotation for the section")]
		public Vector2 randomRotation = new Vector2(0,360);

		// A general use index
		internal int index = 0;
		
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			thisTransform = transform;

			// Make sure the number of hidden walls is never bigger than the number of actual walls this section has
			if ( hiddenWalls > walls.Length )    hiddenWalls = walls.Length;

			// Shuffle the list of walls, so we can hide them randomly
			walls = Shuffle(walls);

			// Hide a number of walls from the section
			while ( hiddenWalls > 0 )
			{
				hiddenWalls--;

				// Hide a wall
				walls[hiddenWalls].gameObject.SetActive(false);
			}

			// Give a random rotation for the section
			thisTransform.eulerAngles = Vector3.forward * Mathf.RoundToInt(Random.Range(randomRotation.x,randomRotation.y));
		}

		/// <summary>
		/// Shuffles the specified Transform list, and returns it
		/// </summary>
		/// <param name="texts">A list of texts</param>
		Transform[] Shuffle( Transform[] objects )
		{
			// Go through all the objects and shuffle them
			for ( index = 0 ; index < objects.Length ; index++ )
			{
				// Hold the text in a temporary variable
				Transform tempObject = objects[index];
				
				// Choose a random index from the text list
				int randomIndex = UnityEngine.Random.Range( index, objects.Length);
				
				// Assign a random text from the list
				objects[index] = objects[randomIndex];
				
				// Assign the temporary text to the random question we chose
				objects[randomIndex] = tempObject;
			}
			
			return objects;
		}


		

	}
}
