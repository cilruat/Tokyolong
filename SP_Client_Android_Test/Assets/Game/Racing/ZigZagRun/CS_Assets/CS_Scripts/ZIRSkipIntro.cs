using UnityEngine;
using Zigzag.Types;

namespace Zigzag
{
	/// <summary>
	/// This script lets the player skip the intro animation, and displays the main menu animation afterwards.
	/// </summary>
	public class ZIRSkipIntro : MonoBehaviour
	{
		//The intro animation object which can be skipped
		public Animation introAnimation;
		
		//The button we have to press in order to skip the intro
		public string skipButton = "Fire1";

        //The menu animation object that plays after the intro animation ends
        public Animation menuAnimation;

        /// <summary>
        /// Start is only called once in the lifetime of the behaviour.
        /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
        /// This allows you to delay any initialization code, until it is really needed.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts
        /// </summary>
        void  Update ()
		{

            if ( introAnimation )
            {
                if (Input.GetButton(skipButton) || introAnimation.isPlaying == false )
                {
                    SkipIntro();
                }
            }
        }

        public void SkipIntro()
        {
            // Skip to the end of the intro animation
            if (introAnimation) introAnimation[introAnimation.clip.name].time = introAnimation.clip.length;

            // Play the menu animation if it exists
            if (menuAnimation) menuAnimation[menuAnimation.clip.name].time = menuAnimation.clip.length;
        }

    }
}
