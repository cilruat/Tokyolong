using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Zigzag
{
	/// <summary>
	/// This script switches between several objects in a sequence. The current shown object number is recorded
    /// in a PlayerPrefs record so it can be kept across several playthroughs
	/// </summary>
	public class ZIRSwitchObjects : MonoBehaviour
	{
        // A list of object we switch through at the start of the scene
        public Transform[] switchObjects;
        internal int switchIndex = 0;

        // The PlayerPrefs record that is used to keep track of the switched objects across multiple playthroughs
        public string playerPrefsName = "TimesOfDay";

        internal int index;
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
            // Get the index of the current object from the PlayerPrefs record
            switchIndex = PlayerPrefs.GetInt(playerPrefsName, switchIndex);

            // Go through all the objects in the list, hide them, and show only the needed object
            for ( index = 0; index < switchObjects.Length; index++)
            {
                if (index == switchIndex) switchObjects[index].gameObject.SetActive(true);
                else switchObjects[index].gameObject.SetActive(false);
            }

            // Go to the next object index in the list
            if (switchIndex < switchObjects.Length - 1) switchIndex++;
            else switchIndex = 0;

            // Record the number so we can use it to show a different object the next time we play
            PlayerPrefs.SetInt(playerPrefsName, switchIndex);
        }
	}
}