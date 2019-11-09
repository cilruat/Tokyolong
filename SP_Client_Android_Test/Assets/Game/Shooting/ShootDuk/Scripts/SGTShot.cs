using UnityEngine;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	/// This script defines a shot that can hit a target
	/// </summary>
	public class SGTShot:MonoBehaviour 
	{
		// The vertical and horizontal power of this shot when it hits a target. X is the sideways power, and Y is the bouncing up power 
		public Vector2 shotPower = new Vector2(8,6);

		// How much damage this shot does when it hits a target
		public float shotDamage = 1;

		/// <summary>
		/// Raises the trigger enter2d event. Works only with 2D physics.
		/// </summary>
		/// <param name="other"> The other collider that this object touches</param>
		void OnTriggerEnter2D(Collider2D other) 
		{
			// Check if we hit the correct target
			if ( other.GetComponent<SGTTarget>() ) 
			{
				// Run the hit target function on the target ( runs on GSGTarget.cs )
				other.SendMessage("HitTarget", transform);
			}
		}
    }
}
