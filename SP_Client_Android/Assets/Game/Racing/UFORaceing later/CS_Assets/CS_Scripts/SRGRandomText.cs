using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SpeederRunGame.Types
{
	/// <summary>
	/// This script changes the text based on the platform type we are using.
	/// </summary>
	public class SRGRandomText:MonoBehaviour
	{
		// The text that will be displayed
		public string[] textList;
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			if ( textList.Length > 0 )    GetComponent<Text>().text = textList[Mathf.FloorToInt(Random.Range(0, textList.Length))];
			
		}
	}
}