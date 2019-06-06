using UnityEngine;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	//This script randomizes an object's animation speed, and time offset
	/// </summary>
	public class SGTRandomizeAnimation:MonoBehaviour 
	{
		//The random speed range
		public Vector2 randomSpeed = new Vector2(0.5f,1);
		
		//Should the time of the animation be randomized?
		public bool  randomOffset = true;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void  Start()
		{
			//Set the speed of the animation to a random number between randomSpeed.x and randomSpeed.y
			GetComponent<Animation>()[GetComponent<Animation>().clip.name].speed = Random.Range(randomSpeed.x, randomSpeed.y);
			
			if ( randomOffset == true )
			{
				//Choose a random time from the animation
				GetComponent<Animation>()[GetComponent<Animation>().clip.name].time = Random.Range(0, GetComponent<Animation>().clip.length);
				
				//Enable the animation
				GetComponent<Animation>()[GetComponent<Animation>().clip.name].enabled = true;
				
				//Sample the animation at the current time
				GetComponent<Animation>().Sample();
				
				//Disable the animation
				GetComponent<Animation>()[GetComponent<Animation>().clip.name].enabled = false;
				
				//Play the animation from the new time we sampled
				GetComponent<Animation>().Play();
			}
		}
	}
}
