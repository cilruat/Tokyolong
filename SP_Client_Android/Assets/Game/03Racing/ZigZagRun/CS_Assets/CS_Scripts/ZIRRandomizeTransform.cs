using UnityEngine;

namespace Zigzag
{
	/// <summary>
	/// Randomizes the scale, rotation, and color of an object
	/// </summary>
	public class ZIRRandomizeTransform : MonoBehaviour
	{
		// The range of the rotation for each axis
		public Vector2 rotationRangeX = new Vector2(0, 360);
		public Vector2 rotationRangeY = new Vector2(0, 360);
		public Vector2 rotationRangeZ = new Vector2(0, 360);
	
		// The scale of the rotation for each axis
		public Vector2 scaleRangeX = new Vector2(1, 1.3f);
		public Vector2 scaleRangeY = new Vector2(1, 1.3f);
		public Vector2 scaleRangeZ = new Vector2(1, 1.3f);
	
		// Should scaling be uniform for all axes?
		public bool  uniformScale = true;
	
		// An array of possible colors for the object
		public Color[] colorList;
	
		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{
			// Set a random rotation for the object
			transform.localEulerAngles = new Vector3(Random.Range(rotationRangeX.x, rotationRangeX.y), Random.Range(rotationRangeY.x, rotationRangeY.y), Random.Range(rotationRangeZ.x, rotationRangeZ.y));

			// If uniform, set the scale of every axis based on the X axis
			if( uniformScale == true )
				scaleRangeY = scaleRangeZ = scaleRangeX;

			// Set a random scale for the object
			transform.localScale = new Vector3(Random.Range(scaleRangeX.x, scaleRangeX.y), Random.Range(scaleRangeY.x, scaleRangeY.y), Random.Range(scaleRangeZ.x, scaleRangeZ.y));

			// Choose a random color from the list
			int randomColor = Mathf.FloorToInt(Random.Range(0, colorList.Length));

			Component[] comps = GetComponentsInChildren<Renderer>();

			// Set the color to all parts of the object
			foreach( Renderer part in comps )
				part.GetComponent<Renderer>().material.color = colorList[randomColor];
		}
	}
}