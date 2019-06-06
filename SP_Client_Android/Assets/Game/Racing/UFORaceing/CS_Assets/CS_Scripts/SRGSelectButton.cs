using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace SpeederRunGame
{
	/// <summary>
	/// Selects a certain button when this canvas/panel/object is enabled
	/// </summary>
	public class SRGSelectButton:MonoBehaviour 
	{
		[Tooltip("The button that will be selected when this object is activated")]
		public GameObject selectedButton;

		/// <summary>
		/// Runs when this object is enabled
		/// </summary>
		void OnEnable() 
		{
			if ( EventSystem.current && selectedButton )    
			{
				// Select the button
				EventSystem.current.SetSelectedGameObject(selectedButton);
			}		
		}
	}
}
