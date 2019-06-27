using System.Collections;
using UnityEngine;
using CoinFrenzyGame.Types;
using CoinFrenzyGame;

namespace CoinFrenzyGame
{
	/// <summary>
	/// This script defines an item, which spawns within the game area and can be picked up by the player. 
	/// </summary>
	public class CFGItem:MonoBehaviour
	{
		internal Transform thisTransform;
		internal GameObject GameController;

		[Tooltip("The tag of the object that this enemy can touch")]
		public string touchTargetTag = "Player";

		[Tooltip("A list of functions that run when this enemy touches the target")]
		public TouchFunction[] touchFunctions;

		[Tooltip("The effect that is created at the location of this item when it is picked up")]
		public Transform pickupEffect;

		[Tooltip("Should the object be removed when picked up?")]
		public bool removeOnPickup = true;
		internal bool isPickedUp = false;

		[Tooltip("Various animation clips")]
		public AnimationClip spawnAnimation;
		public AnimationClip idleAnimation;

		[Tooltip("The sound that plays when you pick up an item")]
		public AudioClip soundPickup;

		[Tooltip("The source from which sounds play, by tag")]
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;

		// The enemy is still spawning, it won't move until it finises spawning
		internal bool isSpawning = true;
	
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

			// Add all the needed animation clips if they are missing from the animation component.
			if ( spawnAnimation && GetComponent<Animation>().GetClip(spawnAnimation.name) == null )    GetComponent<Animation>().AddClip( spawnAnimation, spawnAnimation.name);
			if ( idleAnimation && GetComponent<Animation>().GetClip(idleAnimation.name) == null )    GetComponent<Animation>().AddClip( idleAnimation, idleAnimation.name);

			// Play the spawn animation, and then retrun to the move animation
			StartCoroutine( PlayAnimation( spawnAnimation, idleAnimation));
		}
	
		/// <summary>
		/// Is executed when this obstacle touches another object with a trigger collider
		/// </summary>
		/// <param name="other"><see cref="Collider"/></param>
		void OnTriggerEnter(Collider other)
		{	
			// Check if the object that was touched has the correct tag
			if( isPickedUp == false && other.tag == touchTargetTag )
			{
				isPickedUp = true;

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
				if ( removeOnPickup )    Destroy(gameObject);

				// If there is a sound source and audio to play, play the sound from the audio source
				if ( soundSource && soundPickup )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundPickup);
			}
		}

		/// <summary>
		/// Plays an animation and when it finishes it reutrns to a default animation
		/// </summary>
		/// <returns>The animation.</returns>
		/// <param name="firstAnimation">First animation.</param>
		/// <param name="defaultAnimation">Default animation to be played after first animation is done</param>
		IEnumerator PlayAnimation( AnimationClip firstAnimation, AnimationClip defaultAnimation )
		{
			if( GetComponent<Animation>() )
			{
				// If there is a spawn animation, play it
				if( firstAnimation )
				{
					// Stop the animation
					GetComponent<Animation>().Stop();
					
					// Play the animation
					GetComponent<Animation>().Play(firstAnimation.name);
				
			
					// Wait for some time
					yield return new WaitForSeconds(firstAnimation.length);

					// If the spawning animation finished, we are no longer spawning and can start moving
					if ( isSpawning == true && firstAnimation == spawnAnimation )    isSpawning = false;
				}

				// If there is a walk animation, play it
				if( defaultAnimation )
				{
					// Stop the animation
					GetComponent<Animation>().Stop();
					
					// Play the animation
					GetComponent<Animation>().CrossFade(defaultAnimation.name);
				}
			}
		}
	}
}