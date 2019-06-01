using System.Collections;
using UnityEngine;
using SpeederRunGame.Types;
using SpeederRunGame;

namespace SpeederRunGame
{
	/// <summary>
	/// This script defines a block, which interacts with objects that touch it. It can be an item you can pick up, or an obstacle that kills you
	/// </summary>
	public class SRGBlock:MonoBehaviour
	{
		internal Transform thisTransform;
		internal GameObject GameController;

		[Tooltip("The tag of the object that this enemy can touch")]
		public string touchTargetTag = "Player";

		[Tooltip("A list of functions that run when this enemy touches the target")]
		public TouchFunction[] touchFunctions;

		[Tooltip("The effect that is created at the location of this object when it is picked up")]
		public Transform pickupEffect;

		[Tooltip("Should the object be destroyed when touched")]
		public bool destroyOnTouch = false;

		// The object has been touched, so it can't be touched again
		internal bool isTouched = false;

		[Tooltip("The sound that plays when you touch an object")]
		public AudioClip soundPickup;

		[Tooltip("The source from which sounds play, by tag")]
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;

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

			// Register the game controller for easier access
			GameController = GameObject.FindGameObjectWithTag("GameController");

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);
		}
	
		/// <summary>
		/// Is executed when this obstacle touches another object with a trigger collider
		/// </summary>
		/// <param name="other"><see cref="Collider"/></param>
		void OnTriggerEnter(Collider other)
		{	
			// Check if the object that was touched has the correct tag
			if( isTouched == false && other.tag == touchTargetTag )
			{
				isTouched = true;

				// Go through the list of functions and runs them on the correct targets
				foreach( var touchFunction in touchFunctions )
				{
					// Check that we have a target tag and function name before running
					if( touchFunction.functionName != string.Empty )
					{
						// If the targetTag is "TouchTarget", it means that we apply the function on the object that touched this target
						if ( touchFunction.targetTag == "TouchTarget" )
						{
							// Run the function
							other.SendMessage(touchFunction.functionName, transform);
						}
						else if ( touchFunction.targetTag != string.Empty )    // Otherwise, apply the function on the target tag set in this touch function
						{
							// Run the function
							GameObject.FindGameObjectWithTag(touchFunction.targetTag).SendMessage(touchFunction.functionName, touchFunction.functionParameter);
						}
					}
				}

				// Create a pickup effect, if we have one assigned
				if( pickupEffect )    Instantiate(pickupEffect, transform.position, Quaternion.identity);

				// Remove the item
				if ( destroyOnTouch == true )    Destroy(gameObject);

				// If there is a sound source and audio to play, play the sound from the audio source
				if ( soundSource && soundPickup )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundPickup);
			}
		}
	}
}