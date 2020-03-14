using UnityEngine;
using System.Collections;

namespace StackGameTemplate
{
	/// <summary>
	/// This script removes the object after some time
	/// </summary>
	public class STKRemoveAfterTime:MonoBehaviour 
	{
		[Tooltip("How many seconds to wait before removing this object")]
		public float removeAfterTime = 10;

		[Tooltip("The animation that is played before the object is removed")]
		public AnimationClip removeAnimation;

		[Tooltip("The effect that is created at the location of this object when it is removed")]
		public Transform removeEffect;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			if ( removeAnimation )
			{
				// Add the needed animation clip if it's missing from the animation component.
				if ( GetComponent<Animation>().GetClip(removeAnimation.name) == null )    GetComponent<Animation>().AddClip( removeAnimation, removeAnimation.name);

				// Reduce the length of the remove animation from the total remove time
				if ( removeAnimation )    removeAfterTime -= removeAnimation.length;
			}

			// Play the remove animation with a delay
			StartCoroutine(RemoveAfterTime(removeAfterTime));
		}

		IEnumerator RemoveAfterTime( float delay )
		{
			yield return new WaitForSeconds(delay);

			// Play the remove animation a little before the object gets destroyed
			if ( removeAnimation )    
			{
				GetComponent<Animation>().Play(removeAnimation.name);

				yield return new WaitForSeconds(removeAnimation.length);
			}

			if ( removeEffect )    
			{
				Instantiate( removeEffect, transform.position, Quaternion.identity);
			}

			// Remove this object after a delay
			Destroy( gameObject);
		}
	}
}
