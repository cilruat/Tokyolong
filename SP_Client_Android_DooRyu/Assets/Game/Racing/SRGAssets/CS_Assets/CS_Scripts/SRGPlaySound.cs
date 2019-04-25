using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// Plays a sound from an audio source.
	/// </summary>
	public class SRGPlaySound : MonoBehaviour
	{
		[Tooltip("The sound to play")]
		public AudioClip sound;

		[Tooltip("Should we play the sound when the game starts")]
		public bool playOnStart = true;

		[Tooltip("How long to wait before playing the sound. Make sure you set playOnStart to true if you want the sound to play")]
		public float playDelay = 0;
	
		[Tooltip("The tag of the sound source")]
		public string soundSourceTag = "GameController";

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			if( playOnStart == true )    StartCoroutine("PlaySound");
		}
	
		/// <summary>
		/// Plays the sound
		/// </summary>
		IEnumerator PlaySound()
		{
			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if ( soundSourceTag != string.Empty && sound ) 
			{
				if ( playDelay > 0 )    yield return new WaitForSeconds(playDelay);

				// Play the sound
				GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
			}	
		}
	}
}