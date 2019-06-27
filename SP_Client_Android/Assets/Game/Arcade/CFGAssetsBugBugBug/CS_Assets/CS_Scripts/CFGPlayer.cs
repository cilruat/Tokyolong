using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CoinFrenzyGame.Types;

namespace CoinFrenzyGame
{
	/// <summary>
	/// This script defines a player, which 
	/// </summary>
	public class CFGPlayer:MonoBehaviour 
	{
		internal Transform thisTransform;
		internal Vector3 targetPosition;

		[Tooltip("The animated model that contains an Animator component. The Animator has all the animations of the player (Spawn,Idle,Move)")]
		public Animator animatorObject;

		[Tooltip("The movement speed of the player")]
		public float moveSpeed = 3;
		internal float speedMultiplier = 1;

		// The player is dead now. When dead, the player can't move.
		internal bool isDead = false;

		[Tooltip("This player can't be killed")]
		public bool isImmortal = false;

		[Tooltip("This player can kill enemies when touching them")]
		public bool isKiller = false;

		[Tooltip("The effect that is created at the location of this object when it is destroyed")]
		public Transform deathEffect;

		internal Vector3 currentPosition;
		internal Vector3 previousPosition;

		// Use this for initialization
		void Start() 
		{
			thisTransform = transform;

			currentPosition = previousPosition;
		}
		
		void Update() 
		{
			// Calculate the current position of the player
			currentPosition = thisTransform.position;

			// Set the speed paramater for the animator. If the speed is larger than 0, the Move animation will play, and if it's 0 the Idle animation will play
			if ( animatorObject )    animatorObject.SetFloat("Speed", Vector3.Distance( thisTransform.position, targetPosition));

			// Register the current position of the player as the previous
			previousPosition = currentPosition;

			// Make the player look at the target position
			thisTransform.LookAt(targetPosition);

			// Move the player towards the target position
			thisTransform.position = Vector3.MoveTowards( thisTransform.position, targetPosition, moveSpeed * speedMultiplier * Time.deltaTime);
		}


		/// <summary>
		/// Kills the object, and creates a death effect
		/// </summary>
		public void Die()
		{
			if ( isImmortal == false && isDead == false )
			{
				isDead = true;

				GameObject.FindGameObjectWithTag("GameController").SendMessage("GameOver", 1.5f);

				// If there is a death effect, create it at the position of the player
				if( deathEffect )    Instantiate(deathEffect, transform.position, transform.rotation);

				// Remove the object from the game
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Sets the target position for the player to move towards.
		/// </summary>
		/// <param name="targetValue">Target position</param>
		public void SetTargetPosition( Vector3 targetValue )
		{
			targetPosition = targetValue;
		}

		/// <summary>
		/// Rescale the object to the specified targetScale over time.
		/// </summary>
		/// <param name="targetScale">Target scale.</param>
		IEnumerator Rescale( float targetScale )
		{
			//Perform the scaling action for 1 second
			float scaleTime = 1;
			
			while ( scaleTime > 0 )
			{
				//Count down the scaling time
				scaleTime -= Time.deltaTime;
				
				//Wait for the fixed update so we can animate the scaling
				yield return new WaitForFixedUpdate();
				
				float tempScale = thisTransform.localScale.x;
				
				//Scale the object up or down until we reach the target scale
				tempScale -= ( thisTransform.localScale.x - targetScale ) * 5 * Time.deltaTime;
				
				thisTransform.localScale = Vector3.one * tempScale;
			}
			
			//Rescale the object to the target scale instantly, so we make sure that we got the the target
			thisTransform.localScale = Vector3.one * targetScale;
		}

		/// <summary>
		/// Sets the speed multiplier of the player (ex: if moveSpeed is 3 and speedMultiplier is 2, then the move speed will be 6 )
		/// </summary>
		/// <param name="targetValue"> set value for the multiplier</param>
		public void SetSpeedMultiplier( float setValue )
		{
			speedMultiplier = setValue;

			if ( animatorObject )    
			{
				if ( speedMultiplier > 1 )    animatorObject.SetBool("IsFlying", true);
				else    animatorObject.SetBool("IsFlying", false);
			}
		}

		/// <summary>
		/// Toggles the immortality state of the player ( whether he can die or not )
		/// </summary>
		public void ToggleImmortal()
		{
			isImmortal = !isImmortal;
		}

		/// <summary>
		/// Toggles the killer state of the player ( whether he can kill enemies or not )
		/// </summary>
		public void ToggleKiller()
		{
			isKiller = !isKiller;
		}
	}
}
