using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// This script animates a material's offset over time
	/// </summary>
	public class SRGAnimateUVW:MonoBehaviour 
	{
		// How fast the material will animate
		public Vector3 animationSpeed = Vector3.one;

		// The current offset of the material UVW
		internal Vector3 currentUVW = Vector3.zero;

		// Update is called once per frame
		void Update () 
		{
			// Move the offset over time
			currentUVW += animationSpeed * Time.deltaTime;

			// Update the material
			GetComponent<Renderer>().material.mainTextureOffset = currentUVW;

		}
	}
}
