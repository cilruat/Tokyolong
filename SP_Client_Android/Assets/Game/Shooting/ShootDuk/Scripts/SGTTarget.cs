using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	/// This script defines a target that can be shot and bounced and destroyed.
	/// </summary>
	public class SGTTarget:MonoBehaviour 
	{	
		internal GameObject GameController;

		// How long to wait before showing the target
		internal float showTime = 0;

		// How long to wait before hiding the target, after it has been revealed
		internal float hideDelay = 0;

		// The bonus effect that shows how much score we got when we destroyed a target
		public Transform scoreEffect;

		// Various animations for showing/hiding, and hitting targets
		public string hitAnimation = "TargetHit";
		public string showAnimation = "TargetShow";
		public string hideAnimation = "TargetHide";

		// Has the target been hit?
		public bool isHit = false;

		// Is the target hidden?
		public bool isHidden = true;

		// The sound that plays when this object is hit
		public AudioClip soundHit;

		// The source from which sound for this object play
		public string soundSourceTag = "Sound";

		// The audiosource from which sounds play
		internal GameObject soundSource;

		// A random range for the pitch of the audio source, to make the sound more varied
		public Vector2 pitchRange = new Vector2( 0.9f, 1.1f);

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			// Hold the gamcontroller object in a variable for quicker access
			GameController = GameObject.FindGameObjectWithTag("GameController");

			//Assign the sound source for easier access
			if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{
			if ( isHit == false && isHidden == false && hideDelay > 0 )
			{
				hideDelay -= Time.deltaTime;

				if ( hideDelay <= 0 )    HideTarget();
			}
        }

        /// <summary>
        /// Hits the target, giving hit bonus and playing a sound.
        /// </summary>
        /// <param name="hitSource">Hit source.</param>
        void HitTarget( Transform hitSource )
		{
			if ( isHit == false && isHidden == false )
			{
				// The target has been hit. It can't be hit again until it resets
				isHit = true;

				// Play the hit animation, which also hides the target
				GetComponent<Animation>().Play(hitAnimation);

				// Reset the target after waiting to the hit animation duration
				StartCoroutine(ResetTarget(GetComponent<Animation>().GetClip(hitAnimation).length));

				// Give hit bonus for this target
				GameController.SendMessage("HitBonus", hitSource);

				// If there is a sound source and a sound assigned, play it
				if ( soundSourceTag != "" && soundHit )    
				{
					//Reset the pitch back to normal
					GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().pitch = Random.Range( pitchRange.x, pitchRange.y);;
					
					//Play the sound
					GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundHit);
				}

				// If there is a source and a sound, play it from the source
				if ( soundSource && soundHit )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundHit);
			}
		}

		/// <summary>
		/// Hides the target, animating it and then sets it to hidden
		/// </summary>
		void HideTarget()
		{
			// Play the hiding animation
			GetComponent<Animation>().Play(hideAnimation);

			// Reset the target after waiting to the hiding animation duration
			StartCoroutine(ResetTarget(0));
		}

		/// <summary>
		/// Shows the target, animating it and then sets it to unhidden
		/// </summary>
		/// <returns>The target.</returns>
		IEnumerator ShowTarget( float showDuration )
		{
			// Show the target only if it was hidden before
			if ( isHidden == true )
			{
				// Play the show animation
				GetComponent<Animation>().Play(showAnimation);

				// Wait for the show animation duration
				yield return new WaitForSeconds(GetComponent<Animation>().GetClip(showAnimation).length);

				// The target is not hidden anymore
				isHidden = false;

				// Set how long to wait before hiding the target again
				hideDelay = showDuration;
			}	
		}

		/// <summary>
		/// Resets the target to its hidden and unhit status
		/// </summary>
		/// <returns>The target.</returns>
		/// <param name="delay">How many seconds to wait before resetting the target</param>
		IEnumerator ResetTarget( float delay )
		{
			// Wait some time
			yield return new WaitForSeconds(delay);

			// The target is hidden
			isHidden = true;

			// The target is not hit
			isHit = false;
		}
	}
}
