using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

namespace StackGameTemplate
{
    /// <summary>
    /// Includes functions for loading levels and URLs. It's intended for use with UI Buttons
    /// </summary>
    public class STKLoadLevel:MonoBehaviour
	{
		[Tooltip("How many seconds to wait before loading a level or URL")]
		public float loadDelay = 1;
		internal float delayTime;

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
			delayTime = Time.unscaledTime;

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
			while ( delayTime + loadDelay > Time.realtimeSinceStartup )    yield return new WaitForFixedUpdate();

			Time.timeScale = 1;

			Application.OpenURL(urlName);
		}
	
		/// <summary>
		/// Loads the level.
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public void LoadLevel()
		{
			delayTime = Time.unscaledTime;

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
			while ( delayTime + loadDelay > Time.unscaledTime )    yield return new WaitForEndOfFrame();

			Time.timeScale = 1;

			//SceneManager.LoadScene(levelName);
			SceneManager.LoadScene("ArcadeGame");

        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void RestartLevel()
		{
			delayTime = Time.unscaledTime;

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
			while ( delayTime + loadDelay > Time.unscaledTime )    yield return new WaitForEndOfFrame();

			Time.timeScale = 1;

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}