using UnityEngine;
using System.Collections;

namespace CoinFrenzyGame
{
	/// <summary>
	/// Makes this object look at a target position or object
	/// </summary>
	public class CFGLookAt:MonoBehaviour 
	{
		internal Transform thisTransform;

		[Tooltip("The tag of the object we should look at. This is assigned into lookAtObject on Start ")]
		public string lookAtTag = "MainCamera";

		[Tooltip("The object we should look at. If no object is assigned, we try to get the object from lookAtTag")]
		public Transform lookAtObject;

		[Tooltip("The position we should look at, if there is no object")]
		public Vector3 lookAtPosition;

		[Tooltip("Always look at the target object/position")]
		public bool alwaysLookAtTarget = false;

		[Tooltip("Flip the look direction, so the objects looks away from the target instead of at it")]
		public bool lookAwayFromTarget = false;

		// Use this for initialization
		void Awake() 
		{
			thisTransform = transform;

			// Assign the target object from the tag, if it's not already assigned
			if ( lookAtObject == null && lookAtTag != string.Empty )    lookAtObject = GameObject.FindGameObjectWithTag(lookAtTag).transform;

			// Look at the target object. If there is no object, look at the target position
			if ( lookAtObject )    thisTransform.LookAt(lookAtObject.position);
			else    thisTransform.LookAt(lookAtPosition);

			// Flip the object to make it look away from the target
			if ( lookAwayFromTarget == true )    thisTransform.Rotate( Vector3.up * 180, Space.Self );
		}
		
		// Update is called once per frame
		void Update() 
		{
			if ( alwaysLookAtTarget == true )
			{
				// Look at the target object. If there is no object, look at the target position
				if ( lookAtObject )    thisTransform.LookAt(lookAtObject.position);
				else    thisTransform.LookAt(lookAtPosition);

				// Flip the object to make it look away from the target
				if ( lookAwayFromTarget == true )    thisTransform.Rotate( Vector3.up * 180, Space.Self );
			}
		}
	}
}
