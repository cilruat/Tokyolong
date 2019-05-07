#pragma warning disable 0618

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SpeederRunGame.Types;

namespace SpeederRunGame
{
	/// <summary>
	/// This script defines a player, which has and animation and can die. The movement and rotation speed are not defined here, but in the game controller.
	/// </summary>
	public class SRGPlayer:MonoBehaviour 
	{
		internal GameObject gameController;
		internal Vector3 targetPosition;

		[Tooltip("The animated model that contains an Animator component. The Animator has all the animations of the player (Spawn,Move,etc)")]
		internal Animator animatorObject;

		// The player is dead now. When dead, the player can't move.
		internal bool isDead = false;

		[Tooltip("The effect that is created at the location of this object when it is destroyed")]
		public Transform deathEffect;

        [Tooltip("The effect that is created at the location of this object when it has a shield activated")]
        public ParticleSystem shieldEffect;

        // The time for the shield to be active
        internal float shieldDuration = 0;

        // The time for the slowmotion to be active
        internal float slowmotionDuration = 0;

        void Update()
        {
            // If the shield duration is higher than 0, keep it activated and count down to turn it off
            if (shieldDuration > 0)
            {
                // Count down to turn it off
                shieldDuration -= Time.deltaTime;

                // Keep emitting the effect particles
                if (shieldEffect && shieldEffect.emissionRate != 2) shieldEffect.emissionRate = 2;
            }
            else if ( shieldEffect && shieldEffect.emissionRate != 0) shieldEffect.emissionRate = 0; // Stop the particles

            // If the slowmotion duration is higher than 0, keep it activated and count down to turn it off
            if ( slowmotionDuration > 0) slowmotionDuration -= Time.deltaTime;
            else if ( Time.timeScale != 1) Time.timeScale = 1;
        }

		/// <summary>
		/// Kills the object, and creates a death effect
		/// </summary>
		public void Die()
		{
			if ( isDead == false && shieldDuration <= 0 )
			{
				isDead = true;

				GameObject.FindGameObjectWithTag("GameController").SendMessage("GameOver", 1.5f);

				// If there is a death effect, create it at the position of the player
				if( deathEffect )    Instantiate(deathEffect, transform.position, transform.rotation);

				// Remove the object from the game
				//Destroy(gameObject);

				// Disable the object
				gameObject.SetActive(false);
			}
		}

		public void Live()
		{
			isDead = false;
		}

        /// <summary>
        /// Sets the state of the shield to On or Off, which decides if we get harmed by obstacles or not
        /// </summary>
        public void Shield( float duration )
        {
            shieldDuration = duration;
        }

        /// <summary>
        /// Sets the state of the shield to On or Off, which decides if we get harmed by obstacles or not
        /// </summary>
        public void Slowmotion(float duration)
        {
            slowmotionDuration = duration;

            // Set the slow motion time scale to 0.5
            Time.timeScale = 0.5f;
        }

        /// <summary>
        /// Sets the target position for the player to move towards.
        /// </summary>
        /// <param name="targetValue">Target position</param>
        public void SetTargetPosition( Vector3 targetValue )
		{
			targetPosition = targetValue;
		}
	}
}
