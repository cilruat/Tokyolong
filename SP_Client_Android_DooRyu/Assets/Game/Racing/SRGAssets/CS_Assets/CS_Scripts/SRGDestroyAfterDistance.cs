using UnityEngine;
using System.Collections;

namespace ColorSwitchGame
{
	/// <summary>
	/// This script removes the object after it passes the camera position by a certain distance
	/// </summary>
	public class SRGDestroyAfterDistance:MonoBehaviour 
	{
		[Tooltip("How many seconds to wait before removing this object")]
		public float destroyAfterDistance = 10;

		[Tooltip("How often should we check if we passed the distance?")]
		public float checkTime = 5;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			// Check if we passed the required distance every few seconds
			InvokeRepeating("CheckDistance", checkTime, checkTime);
		}

		void CheckDistance()
		{
			// If we pass the distance, destroy the object
			//if ( Vector3.Distance( Camera.main.transform.position, transform.position) > destroyAfterDistance )    Destroy(gameObject);
		
			if ( Camera.main.transform.position.z > transform.position.z + destroyAfterDistance )    Destroy(gameObject);
		}
	}
}
