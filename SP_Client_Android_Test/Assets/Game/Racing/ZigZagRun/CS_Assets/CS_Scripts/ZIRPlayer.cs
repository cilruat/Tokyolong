using UnityEngine;
using System.Collections;

namespace Zigzag
{
	public class ZIRPlayer : MonoBehaviour 
	{
		/// <summary>
		/// This script defines the player, its movement and turning speed, as well as the effects and sounds associated with it.
		/// </summary>
		internal Transform thisTransform;
		internal GameObject gameController;
		
		//The player's movement speed, and variables that check if the player is moving now, where it came from, and where it's going to
		public float speed = 0.1f;
		
		//How fast the player rotates towards its target direction
		public float turnSpeed = 100;
		internal float turnDirection = 0;
		
		//A list of all the wheels that can turn
		public Transform[] wheels;
		
		//Death effects that show when the player is killed
		public Transform deathEffect;

		// The animation that plays when we win
		public string animationVictory = "PlayerVictory";

		// Did the player win the game?
		internal bool isVictorious = false;
		
		//Various sounds and their source
		public AudioClip[] soundTurn;
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void  Start ()
		{
			thisTransform = transform;
			
			gameController = GameObject.FindGameObjectWithTag("GameController");
			
			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void  Update ()
		{
			//If the game is not paused, the player turns towards its target direction
			if ( Time.timeScale > 0 && isVictorious == false )
			{
				//Turn towards your target direction and then snap to it
				if ( Mathf.Abs(Mathf.DeltaAngle( thisTransform.eulerAngles.y, turnDirection)) > 5 )    thisTransform.eulerAngles = new Vector3( thisTransform.eulerAngles.x, Mathf.LerpAngle( thisTransform.eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), thisTransform.eulerAngles.z);
				else    thisTransform.eulerAngles = new Vector3( thisTransform.eulerAngles.x, turnDirection, thisTransform.eulerAngles.z);
				
				//Always move forward based on speed
				thisTransform.Translate( Vector3.forward * speed * Time.deltaTime, Space.Self);
				
				//Tilt the player object opposite the turning direction, to give a nice effect
				//thisTransform.localEulerAngles = new Vector3( thisTransform.localEulerAngles.x, thisTransform.localEulerAngles.y, Mathf.LerpAngle( thisTransform.localEulerAngles.z, (turnDirection - thisTransform.eulerAngles.y) * 0.5f, Time.deltaTime * turnSpeed));

				//Also turn the wheels in the direction of turning
				wheels[0].eulerAngles = new Vector3( wheels[0].eulerAngles.x, Mathf.LerpAngle( wheels[0].eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), wheels[0].eulerAngles.z);
				wheels[1].eulerAngles = new Vector3( wheels[1].eulerAngles.x, Mathf.LerpAngle( wheels[1].eulerAngles.y, turnDirection, Time.deltaTime * turnSpeed), wheels[1].eulerAngles.z);
			}
		}
		
		/// <summary>
		/// Destroys the player, and triggers the game over event
		/// </summary>
		/// <param name="deathType"> index of the death effect</param>
		void  Die (  int deathType   )
		{
			//Call the game over function from the game controller
			gameController.SendMessage("GameOver", 2);
			
			//Create the death effect in place of the player
			if ( deathEffect )    Instantiate( deathEffect, thisTransform.position, thisTransform.rotation);

			//Remove the player object
			Destroy(gameObject);
		}

		/// <summary>
		/// Runs when the player wins the level
		/// </summary>
		void Victory()
		{
			isVictorious = true;

			// If there is a vicotry animation, play it
			if ( GetComponent<Animation>() && animationVictory != string.Empty )    GetComponent<Animation>().Play(animationVictory);
		}
		
		/// <summary>
		/// Sets the turn direction
		/// </summary>
		/// <param name="direction"> angle to rotate to</param>
		void  SetTurn (  float direction   )
		{
			turnDirection = direction;
			
			//If there is a sound source and more than one sound assigned, play one of them from the source
			if ( soundSource && soundTurn.Length > 0 )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundTurn[Mathf.FloorToInt(Random.value * soundTurn.Length)]);
		}
	}
}