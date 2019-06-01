using UnityEngine;
using Zigzag.Types;

namespace Zigzag
{
	/// <summary>
	/// This script defines a block which can interact with the player in various ways. A block may be a rock or a wall that
	/// kills the player, or it can be an item that can be collected like cash.
	/// </summary>
	public class ZIRBlock : MonoBehaviour
	{
		//The tag of the object that can touch this block
		public string touchTargetTag = "Player";
		
		//A list of functions that run when this block is touched by the target
		public TouchFunction[] touchFunctions;

		//Remove this object after a certain amount of touches. If set to 0, this object will never be removed (  a ex   death wall )
		public int removeAfterTouches = 0;
		internal bool  isRemovable = false;
		
		//The animation that plays when this object is touched
		public AnimationClip hitAnimation;
		
		//The sound that plays when this object is touched
		public AudioClip soundHit;
		public string soundSourceTag = "GameController";
		
		//The effect that is created at the location of this object when it is destroyed
		public Transform deathEffect;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void  Start ()
		{
			//If removeAfterTouches is higher than 0, make this object removable after one or more touches
			if ( removeAfterTouches > 0 )    isRemovable = true;
		}
		
		/// <summary>
		/// Is executed when this obstacle touches another object with a trigger collider
		/// </summary>
		/// <param name="other"><see cref="Collider"/></param>
		void  OnTriggerEnter ( Collider other  )
		{	
			//Check if the object that was touched has the correct tag
			if ( other.tag == touchTargetTag )
			{
				//Go through the list of functions and runs them on the correct targets
				foreach( var touchFunction in touchFunctions )
				{
					//Check that we have a target tag and function name before running
					if ( touchFunction.targetTag != string.Empty && touchFunction.functionName != string.Empty )
					{
						//Run the function
						GameObject.FindGameObjectWithTag(touchFunction.targetTag).SendMessage(touchFunction.functionName, touchFunction.functionParameter);
					}
				}
				
				//If there is an animation, play it
				if ( GetComponent<Animation>() && hitAnimation )    
				{
					//Stop the animation
					GetComponent<Animation>().Stop();
					
					//Play the animation
					GetComponent<Animation>().Play(hitAnimation.name);
				}
				
				//If this object is removable, count down the touches and then remove it
				if ( isRemovable == true )
				{
					//Reduce the number of times this object was touched by the target
					removeAfterTouches--;
					
					if ( removeAfterTouches <= 0 )    
					{
						if ( deathEffect )    Instantiate( deathEffect, transform.position, Quaternion.identity);
						
						Destroy(gameObject);
					}
				}
				
				//If there is a sound source and a sound assigned, play it
				if ( soundSourceTag != "" && soundHit )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundHit);
			}
		}
	}
}
