using UnityEngine;

namespace Zigzag
{
	/// <summary>
	/// Plays a random sound from a list of sounds, through an audio source.
	/// </summary>
	public class ZIRPlaySound : MonoBehaviour
	{
		// An array of possible sounds
		public AudioClip[] audioList;
	
		// The tag of the sound source
		public string audioSourceTag = "Sound";
		public bool playOnStart = true;
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			if( playOnStart == true )
				PlaySound();
		}
	
		/// <summary>
		/// Plays a random sound from the audioList array based off upper and lower bounds.
		/// </summary>
		void PlaySound()
		{
			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if( audioSourceTag != string.Empty && audioList.Length > 0 )
				GameObject.FindGameObjectWithTag(audioSourceTag).GetComponent<AudioSource>().PlayOneShot(audioList[Mathf.FloorToInt(Random.value * audioList.Length)]);
		}
	
		/// <summary>
		/// Plays a song by index from the audio array
		/// </summary>
		/// <param name="soundIndex"><see cref="AudioClip"/> index in the array</param>
		void PlaySound(int soundIndex)
		{
			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if( audioSourceTag != string.Empty && audioList.Length > 0 ) 
				GameObject.FindGameObjectWithTag(audioSourceTag).GetComponent<AudioSource>().PlayOneShot(audioList[soundIndex]);
		}

        /// <summary>
		/// Plays the sound
		/// </summary>
		public void PlaySound(AudioClip sound)
        {
            // If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
            if (audioSourceTag != string.Empty && sound)
            {
                // Play the sound
                GameObject.FindGameObjectWithTag(audioSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
            }
        }
    }
}