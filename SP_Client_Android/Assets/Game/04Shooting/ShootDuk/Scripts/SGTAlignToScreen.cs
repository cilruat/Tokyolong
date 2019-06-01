using UnityEngine;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	/// This script aligns an object to either the left or right edge of the screen
	/// </summary>
	public class SGTAlignToScreen : MonoBehaviour 
	{
		// The side to which this object is aligned
		public string alignTo = "Left";

		// Use this for initialization
		void Start() 
		{
			// Align the object to the left edge of the screen, or to the right
			if ( alignTo == "Left" )    transform.position = new Vector3( Camera.main.ScreenToWorldPoint(Vector3.zero).x, transform.position.y, transform.position.z);
			else if ( alignTo == "Right" )    transform.position = new Vector3( Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x, transform.position.y, transform.position.z);
		}
	}
}
