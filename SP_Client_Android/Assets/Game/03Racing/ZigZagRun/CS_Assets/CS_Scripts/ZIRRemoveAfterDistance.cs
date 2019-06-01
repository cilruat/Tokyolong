using UnityEngine;
using System.Collections;

namespace Zigzag
{
	public class ZIRRemoveAfterDistance : MonoBehaviour 
	{
		internal Transform thisTransform;
		internal Transform cameraObject;

		public float distance = 10;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start () 
		{
			thisTransform = transform;
			cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
		}
		
		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void Update () 
		{
			if ( thisTransform.position.x < cameraObject.position.x - distance )    Destroy(gameObject);
		}
	}
}
