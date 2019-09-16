using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class CameraFollow : MonoBehaviour {

	[Header("Camera - Player Smooth follow movement")]
	public Transform target; // The transform that camera will follow
	public float smoothFollow = 0.5F; // The smooth speed of the following movement.
	private Vector3 velocity = Vector3.zero;
	[Header("Camera Bounds")]
	public float camXPos; // The max X camera position movement
	public float camYPos; // The max Y camera position movement
	public float camZPos; // The max Z camera position movement

	void LateUpdate() {

			Vector3 targetPosition = target.TransformPoint (new Vector3 (camXPos, camYPos, camZPos));
			transform.position = Vector3.SmoothDamp (this.transform.position, targetPosition, ref velocity, smoothFollow);

	}

}