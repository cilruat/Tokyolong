using UnityEngine;
using System.Collections;

namespace CoinFrenzyGame
{
	[RequireComponent (typeof (Animation))]

	/// <summary>
	/// This script makes an intro animation, and then runs a default animation that can be interupted by random animations.
	/// This is used in the main menu screen to give a bit livelihood to the player character ( make bounce around, look around, etc )
	/// </summary>
	public class CFGIdleAnimation:MonoBehaviour 
	{
		[Tooltip("The first animation that plays. Basically the intro to the idle animation")]
		public AnimationClip firstAnimation;

		[Tooltip("The default/idle animation that plays when no other animations are playing")]
		public AnimationClip idleAnimation;

		[Tooltip("A list of random animations that play every once in a while")]
		public AnimationClip[] randomAnimations;

		[Tooltip("How long to wait before playing one of the random animations")]
		public Vector2 randomAnimationDelay = new Vector2(3, 10);
		internal float animationDelay = 0;

		// Use this for initialization
		void Start() 
		{
			// Play the intro animation
			StartCoroutine(IntroAnimation());

			// Add all the needed animation clips if they are missing from the animation component.
			if ( firstAnimation && GetComponent<Animation>().GetClip(firstAnimation.name) == null )    GetComponent<Animation>().AddClip( firstAnimation, firstAnimation.name);
			if ( idleAnimation && GetComponent<Animation>().GetClip(idleAnimation.name) == null )    GetComponent<Animation>().AddClip( idleAnimation, idleAnimation.name);

		}

		/// <summary>
		/// Play the intro animation
		/// </summary>
		/// <returns>The animation.</returns>
		IEnumerator IntroAnimation()
		{
			if ( GetComponent<Animation>() )
			{
				// Play the intro animation
				if ( firstAnimation )    GetComponent<Animation>().Play(firstAnimation.name);

				// Wait until the end of the animation
				yield return new WaitForSeconds(firstAnimation.length);

				// Play the idle animation
				if ( idleAnimation )    GetComponent<Animation>().Play(idleAnimation.name);

				// Play a random animation
				if ( randomAnimations.Length > 0 )    StartCoroutine(RandomAnimation());

				//print ("should choose a random animation");
			}
		}

		/// <summary>
		/// Plays a random animation.
		/// </summary>
		/// <returns>The animation.</returns>
		IEnumerator RandomAnimation()
		{
			if ( GetComponent<Animation>() )
			{
				// Calculate a delay for the next random animation
				animationDelay = Random.Range( randomAnimationDelay.x, randomAnimationDelay.y);

				// Wait until the end of the animation
				yield return new WaitForSeconds(animationDelay);

				// Play one of the random animations
				if ( randomAnimations.Length > 0 )
				{
					// Choose a random animation
					int randomIndex = Mathf.FloorToInt(Random.value * randomAnimations.Length);

					// Play the random animation, if it exists
					if ( randomAnimations[randomIndex] )    
					{
						// Play the random animation
						GetComponent<Animation>().Play(randomAnimations[randomIndex].name);

						// Wait until the end of the animation
						yield return new WaitForSeconds(randomAnimations[randomIndex].length);
					}
				}

				// Play a random animation
				StartCoroutine(RandomAnimation());
			}
		}
	}
}
