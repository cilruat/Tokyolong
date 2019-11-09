using System.Collections;
using UnityEngine;
using CoinFrenzyGame.Types;
using CoinFrenzyGame;

namespace CoinFrenzyGame
{
	/// <summary>
	/// This script defines an enemy, which spawns and moves within the game area. If the enemy touches the player the player dies. 
	/// </summary>
	public class CFGEnemy:MonoBehaviour
	{
		internal Transform thisTransform;
		static GameObject GameController;
		internal Rect gameAreaLimits;

		[Tooltip("The animated model that contains an Animator component. The Animator has all the animations of the enemy (Spawn,Idle,Move)")]
		public Animator animatorObject;

		[Tooltip("How long to wait before chaning a target. The target is chosen within the game area limits")]
		public float changeTargetTime = 5;
		internal float changeTargetTimeCount;
		internal Vector3 targetPosition;

		[Tooltip("The movement speed of the enemy.")]
		public float moveSpeed = 1;

		[Tooltip("The tag of the object that this enemy can touch")]
		public string touchTargetTag = "Player";

		[Tooltip("A list of functions that run when this enemy touches the target")]
		public TouchFunction[] touchFunctions;

		[Tooltip("Various sounds that play when the enemy touches the target, or when it gets hurt")]
		public AudioClip soundHitTarget;
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
			if (GameController == null)    GameController = GameObject.FindGameObjectWithTag ("GameController");

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Set the time to the next target change
			changeTargetTimeCount = changeTargetTime;

			// Choose a random target position within the game area, which this enemy will move towards. 
			targetPosition = new Vector3( Random.Range( gameAreaLimits.x, gameAreaLimits.width), 0, Random.Range( gameAreaLimits.y, gameAreaLimits.height));
		}
		
		void Update()
		{
			// While the enemy is spawning, it can't move
			if ( isSpawning == true && !animatorObject.GetCurrentAnimatorStateInfo(0).IsName("Spawn") )
			{
				isSpawning = false;
			}

			// Move the enemy based on its speed
			if ( isSpawning == false )    
			{
				// Set the speed paramater for the animator. If the speed is larger than 0, the Move animation will play, and if it's 0 the Idle animation will play
				if ( animatorObject )    animatorObject.SetFloat("Speed", Vector3.Distance( thisTransform.position, targetPosition));

				// While the enemy is far from the target position, move towards it
				if ( Vector3.Distance( thisTransform.position, targetPosition) > 0.1 )    thisTransform.Translate( Vector3.forward * moveSpeed * Time.deltaTime, Space.Self );

				// Calculate the movement direction of the enemy
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, 0.5f * Time.deltaTime, 0.0F);

				// Rotate towards the target position
				transform.rotation = Quaternion.LookRotation(newDir);

				// Count down to the next target change
				if ( changeTargetTimeCount > 0 )    changeTargetTimeCount -= Time.deltaTime;
				else
				{
					changeTargetTimeCount = changeTargetTime;

					// Choose a random target position within the game area, which this enemy will move towards. 
					targetPosition = new Vector3( Random.Range( gameAreaLimits.x, gameAreaLimits.width), 0, Random.Range( gameAreaLimits.y, gameAreaLimits.height));
				}
			}
		}
	
		/// <summary>
		/// Is executed when this obstacle touches another object with a trigger collider
		/// </summary>
		/// <param name="other"><see cref="Collider"/></param>
		void OnTriggerEnter(Collider other)
		{	
			// Check if the object that was touched has the correct tag
			if( isSpawning == false && other.tag == touchTargetTag )
			{
				// Go through the list of functions and runs them on the correct targets
				foreach( var touchFunction in touchFunctions )
				{
					// Check that we have a target tag and function name before running
					if( touchFunction.functionName != string.Empty )
					{
						// If the targetTag is "TouchTarget", it means that we apply the function on the object that ouched this lock
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

				// If there is a sound source and audio to play, play the sound from the audio source
				if ( soundSource && soundHitTarget )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundHitTarget);
			}
		}

		/// <summary>
		/// Sets the game area limits, taken from the game controller
		/// </summary>
		/// <param name="setValue">Set value.</param>
		public void SetGameAreaLimits( Rect setValue )
		{
			gameAreaLimits = setValue;
		}

		void OnDrawGizmos()
		{
			// Draw a line from this enemy to its target position
			Gizmos.DrawLine( transform.position, targetPosition);
		}

	}
}