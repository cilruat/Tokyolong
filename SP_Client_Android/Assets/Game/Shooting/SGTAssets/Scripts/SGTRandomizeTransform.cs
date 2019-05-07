using UnityEngine;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	/// This script randomizes an object's rotation and scale within a range.
	/// </summary>
	public class SGTRandomizeTransform:MonoBehaviour 
	{
		// A random rotation is chosen within this range
		public Vector2 rotationRange = new Vector2(-90, 90);

		// A random scale is chosen within this range
		public Vector2 scaleRange = new Vector2(-0.9f, 1.1f);

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			// Set a random rotation within the range
			transform.eulerAngles = Vector3.forward * Random.Range( rotationRange.x, rotationRange.y);

			// Set a random scale within the range
			transform.localScale = Vector3.one * Random.Range( scaleRange.x, scaleRange.y);
		}
	}
}
