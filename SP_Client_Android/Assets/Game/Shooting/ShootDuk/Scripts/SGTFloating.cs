using UnityEngine;
using System.Collections;

namespace ShootingGallery
{
	/// <summary>
	/// This script makes an object float based on height and speed. The floating phase is realative to the X position of the object.
	/// Used to make the moving targets float across the screen
	/// </summary>
	public class SGTFloating : MonoBehaviour 
	{
		internal Transform thisTransform;
		internal Vector3 initialPosition;

		// The height range that the object will float in
		public float height = 0.3f;

		// The speed at which the object floats
		public float speed = 3;

		// Use this for initialization
		void Start() 
		{
			thisTransform = transform;

			// Remember the initial position of the object
			initialPosition = thisTransform.position;
		}
		
		// Update is called once per frame
		void Update() 
		{
			// Make the object float relative to its initial position
			thisTransform.position = new Vector3( thisTransform.position.x, initialPosition.y + Mathf.Sin(thisTransform.position.x * speed) * height, thisTransform.position.z );
		}
	}
}
