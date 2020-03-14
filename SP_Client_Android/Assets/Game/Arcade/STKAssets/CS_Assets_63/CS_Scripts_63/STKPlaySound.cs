using UnityEngine;
using System.Collections;

namespace StackGameTemplate
{
	/// <summary>
	/// Plays a sound from an audio source.
	/// </summary>
	public class STKPlaySound : MonoBehaviour
	{
		[Tooltip("The sound to play")]
		public AudioClip sound;

		[Tooltip("Should we play the sound when the game starts")]
		public bool playOnStart = true;

        [Tooltip("The tag of the sound source")]
		public string soundSourceTag = "Sound";

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			if( playOnStart == true )    PlaySound();
		}

		/// <summary>
		/// Plays the sound
		/// </summary>
		public void PlaySound()
		{
			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if ( soundSourceTag != string.Empty && sound ) 
			{
				// Play the sound
				GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
			}	
		}
    }
}