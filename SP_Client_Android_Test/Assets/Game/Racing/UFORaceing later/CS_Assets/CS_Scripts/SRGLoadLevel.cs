using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// Includes functions for loading levels and URLs. It's intended for use with UI Buttons
	/// </summary>
	public class SRGLoadLevel:MonoBehaviour
	{
		[Tooltip("How many seconds to wait before loading a level or URL")]
		public float loadDelay = 1;

		[Tooltip("The name of the URL to be loaded")]
		public string urlName = "";

		[Tooltip("The name of the level to be loaded")]
		public string levelName = "";

		[Tooltip("The object that appears between scenes")]
		public Transform transitionEffect;

		[Tooltip("The sound that plays when loading a level")]
		public AudioClip soundLoad;

		[Tooltip("The tag of the sound source")]
		public string soundSourceTag = "GameController";

		[Tooltip("The object that plays the sound")]
		public GameObject soundSource;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
		    // If there is no sound source assigned, try to assign it from the tag name
			if ( !soundSource && GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);
		}

		/// <summary>
		/// Loads the URL.
		/// </summary>
		/// <param name="urlName">URL/URI</param>
		public void LoadURL()
		{
			// Execute the function after a delay
			StartCoroutine("ExecuteLoadURL");

			if ( transitionEffect )    Instantiate(transitionEffect);

			// If there is a sound, play it from the source
			if ( soundSource && soundLoad )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundLoad);
		}

		/// <summary>
		/// Executes the load URL function
		/// </summary>
		IEnumerator ExecuteLoadURL()
		{
			Time.timeScale = 1;

			yield return new WaitForSeconds(loadDelay);

			Application.OpenURL(urlName);
		}
	
		/// <summary>
		/// Loads the level.
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public void LoadLevel()
		{
			// Execute the function after a delay
			StartCoroutine("ExecuteLoadLevel");

			if ( transitionEffect )    Instantiate(transitionEffect);
			
			// If there is a sound, play it from the source
			if ( soundSource && soundLoad )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundLoad);
		}

		/// <summary>
		/// Executes the Load Level function
		/// </summary>
		IEnumerator ExecuteLoadLevel()
		{
			Time.timeScale = 1;

			yield return new WaitForSeconds(loadDelay);

			SceneManager.LoadScene(levelName);
		}

		/// <summary>
		/// Restarts the current level.
		/// </summary>
		public void RestartLevel()
		{
			// Execute the function after a delay
			StartCoroutine("ExecuteRestartLevel");

			if ( transitionEffect )    Instantiate(transitionEffect);

			// If there is a sound, play it from the source
			if ( soundSource && soundLoad )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundLoad);
		}
		
		/// <summary>
		/// Executes the Load Level function
		/// </summary>
		IEnumerator ExecuteRestartLevel()
		{
			Time.timeScale = 1;

			yield return new WaitForSeconds(loadDelay);

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}