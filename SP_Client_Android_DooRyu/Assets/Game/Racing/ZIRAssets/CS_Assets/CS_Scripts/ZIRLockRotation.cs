using UnityEngine;

namespace Zigzag
{
	/// <summary>
	/// Locks the current rotation of the object. Used to keep the camera looking in the same direction
	/// </summary>
	public class ZIRLockRotation : MonoBehaviour
	{
		internal Transform thisTransform;
		internal Quaternion initialRotation;
		
		void Start()
		{
			thisTransform = transform;
			
			// Set the initial rotation of the object
			initialRotation = thisTransform.rotation;
		}
		
		void LateUpdate()
		{
			// Keep the rotation of the object to the initial rotation
			thisTransform.rotation = initialRotation;
		}
	}
}
