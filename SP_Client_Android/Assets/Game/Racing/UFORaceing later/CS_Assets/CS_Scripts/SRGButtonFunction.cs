using UnityEngine;
using System.Collections;

namespace SpeederRunGame
{
	/// <summary>
	/// This script executes a function on a target object. It needs to be attached to a UI Button.
	/// </summary>
	public class SRGButtonFunction:MonoBehaviour 
	{
		[Tooltip("The tag of the object in which the function will run")]
		public string targetTag = "GameController";

		//The object in which the function will run
		internal GameObject targetObject;

		[Tooltip("The name of the function that will be executed OnMouseDown")]
		public string mouseDownFunction = "ChangeLane";

		//[Tooltip("The name of the function that will be executed OnMouseUp")]
		//public string mouseUpFunction = "EndMove";

		[Tooltip("The value of the parameter passed along to the function")]
		public int parameter = 0;

		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start()
		{
			// If the target object is not assigned, assign it by the tag
			if ( targetObject == null )    targetObject = GameObject.FindGameObjectWithTag(targetTag);
		}

		/// <summary>
		/// Executes the function at the target object, OnMouseDown
		/// </summary>
		public void ExecuteMouseDown()
		{
			// Check if we have a function name
			if ( mouseDownFunction != string.Empty )
			{  
				// Check if there is a target object
				if ( targetObject )    
				{
					//Send the message to the target object, with a parameter
					targetObject.SendMessage( mouseDownFunction, parameter);
				}
			}
		}
		
		/// <summary>
		/// Executes the function at the target object, OnMouseUp
		/// </summary>
//		public void ExecuteMouseUp()
//		{
//			// Check if we have a function name
//			if ( mouseUpFunction != string.Empty )
//			{  
//				// Check if there is a target object
//				if ( targetObject )    
//				{
//					//Send the message to the target object, with a parameter
//					targetObject.SendMessage( mouseUpFunction, parameter);
//				}
//			}
//		}
	}
}