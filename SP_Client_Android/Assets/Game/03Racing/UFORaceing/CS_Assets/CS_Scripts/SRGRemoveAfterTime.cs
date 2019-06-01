using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// This script removes the object after some time
	/// </summary>
	public class SRGRemoveAfterTime:MonoBehaviour 
	{
		[Tooltip("How many seconds to wait before removing this object")]
		public float removeAfterTime = 10;

		[Tooltip("Keep the object alive even when loading a new scene/level. This is used to allow a transition effect between scenes")]
		public bool keepBetweenScenes = false;

		public AnimationClip removeAnimation;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			// Remove this object after a delay
			Destroy( gameObject, removeAfterTime);

			if ( removeAnimation )
			{
				// Add the needed animation clip if it's missing from the animation component.
				if ( GetComponent<Animation>().GetClip(removeAnimation.name) == null )    GetComponent<Animation>().AddClip( removeAnimation, removeAnimation.name);

				// Reduce the length of the remove animation from the total remove time
				removeAfterTime -= removeAnimation.length;

				// Play the remove animation with a delay
				StartCoroutine(RemoveAfterTime(removeAfterTime));
			}

			if ( keepBetweenScenes == true )    DontDestroyOnLoad(gameObject);
		}

		IEnumerator RemoveAfterTime( float delay )
		{
			yield return new WaitForSeconds(delay);

			// Play the remove animation a little before the object gets destroyed
			GetComponent<Animation>().Play(removeAnimation.name);
		}
	}
}
