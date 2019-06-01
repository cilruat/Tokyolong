using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace ZombieDriveGame
{
	/// <summary>
	/// Selects a certain button when this canvas/panel/object is enabled
	/// </summary>
	public class ZDGSelectButton : MonoBehaviour 
	{
		// The button to select
		public GameObject selectedButton;

		/// <summary>
		/// Runs when this object is enabled
		/// </summary>
		void OnEnable() 
		{
            if ( selectedButton )    
			{
				// Select the button
				if ( EventSystem.current )    EventSystem.current.SetSelectedGameObject(selectedButton);
			}	
		}
	}
}
