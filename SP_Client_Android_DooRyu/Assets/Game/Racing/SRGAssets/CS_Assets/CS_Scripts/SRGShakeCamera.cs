using System.Collections;
using UnityEngine;

namespace SpeederRunGame
{
	/// <summary>
	/// Shakes an object when it runs, with values for strength and time. You can set which object to shake, and if you keep the object value empty it 
	/// will shake the object it's attached to.
	/// </summary>
	public class SRGShakeCamera:MonoBehaviour
	{

		public Transform cameraObject;

		// The original position of the camera
		public Vector3 cameraOrigin;
		
		// How violently to shake the camera
		public Vector3 strength;
		private Vector3 strengthDefault;
		
		// How quickly to settle down from shaking
		public float decay = 0.8f;
		
		// How many seconds to shake
		public float shakeTime = 1f;
		private float shakeTimeDefault;
		public bool playOnAwake = true;
		public float delay = 0;
		
		// Is this effect playing now?
		public bool isShaking = false;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		public IEnumerator Start()
		{
			if ( cameraObject == null )    cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;

			cameraOrigin = cameraObject.position;

			strengthDefault = strength;
			
			shakeTimeDefault = shakeTime;
			
			if( playOnAwake )
			{
				yield return StartCoroutine(WaitASec(delay));
				
				StartShake();
			}
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		public void Update()
		{
			if( isShaking == true )
			{
				if( shakeTime > 0 )
				{		
					shakeTime -= Time.deltaTime;
					
					Vector3 tempPosition = Camera.main.transform.position;

					// Move the camera in all directions based on strength
					tempPosition.x = cameraOrigin.x + Random.Range(-strength.x, strength.x);
					tempPosition.y = cameraOrigin.y + Random.Range(-strength.y, strength.y);
					//tempPosition.z = cameraOrigin.z + Random.Range(-strength.z, strength.z);

					Camera.main.transform.position = tempPosition;
					
					// Gradually reduce the strength value
					strength *= decay;
				}
				else
				if( Camera.main.transform.position != cameraOrigin )
				{
					shakeTime = 0;
					
					// Reset the camera position
					Camera.main.transform.position = new Vector3( cameraOrigin.x, cameraOrigin.y, Camera.main.transform.position.z);
					
					isShaking = false;
				}
			}
		}

		/// <summary>
		/// Starts the shake of the camera.
		/// </summary>
		public void StartShake()
		{
			isShaking = true;
			
			strength = strengthDefault;
			
			shakeTime = shakeTimeDefault;
		}

		
		/// <summary>
		/// Waits an amount of time, used with startcoroutine.
		/// </summary>
		/// <returns>The A sec.</returns>
		/// <param name="time">Time.</param>
		public IEnumerator WaitASec(float time)
		{
			yield return new WaitForSeconds(time);
		}
	}
}