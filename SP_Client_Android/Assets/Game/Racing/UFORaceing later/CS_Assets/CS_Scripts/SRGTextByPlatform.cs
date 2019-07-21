using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SpeederRunGame.Types
{
	/// <summary>
	/// This script changes the text based on the platform type we are using.
	/// </summary>
	public class SRGTextByPlatform:MonoBehaviour
	{
		// The text that will be displayed on PC/Mac/Webplayer
		public string computerText = "CLICK TO START";
	
		// The text that will be displayed on Android/iOS/WinPhone
		public string mobileText = "TAP TO START";
	
		// The text that will be displayed on Playstation, Xbox, Wii
		public string consoleText = "PRESS 'A' TO START";

		// The UI graphic that will be displayed on PC/Mac/Webplayer
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			if ( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer )
			{
				GetComponent<Text>().text = computerText;
			}
			else if ( Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android )
			{
				GetComponent<Text>().text = mobileText;
			}
			else if ( Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne )
			{
				GetComponent<Text>().text = consoleText;
			}
		}
	}
}