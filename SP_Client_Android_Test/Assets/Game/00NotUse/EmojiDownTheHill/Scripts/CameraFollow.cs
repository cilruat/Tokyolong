using UnityEngine;
using System.Collections;

namespace Emoji2
{
	public class CameraFollow : MonoBehaviour {

		public GameObject target;
		float initialDeltaY;

	    void Start()
	    {
	        initialDeltaY = target.transform.position.y - transform.position.y;
	    }
		
		// Update is called once per frame
		void Update () {
	        
	        float deltaY = target.transform.position.y - transform.position.y;

	        if (deltaY > initialDeltaY + 0.5f) {
	            transform.position = transform.position + new Vector3(0, 0.02f, 0); // camera moves up to catch up with target 
			}
		}
	}
}