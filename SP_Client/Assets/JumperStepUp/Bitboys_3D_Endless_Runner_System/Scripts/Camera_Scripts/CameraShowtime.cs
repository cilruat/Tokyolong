using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.

public class CameraShowtime : MonoBehaviour {

	[Header("Game Start Camera movement")]
	public GameObject introCamStartPosition; // The initial position of the camera when starts the show movement.

	[Header("Don change this!")]
	public GameObject orbitObject; // The object chosen to enable the camera orbite around it.
	public float rotateSpeed = 20f; // The rotation speed.
	public float moveSpeed = 2f; // The movement speed.
	private bool orbitDown = false; // activates the orbit down movement if sets to true.
	private bool orbitUp = false; // activates the orbit up movement if sets to true.



	void Awake(){

		this.transform.localPosition = introCamStartPosition.transform.localPosition;
		this.transform.localRotation = introCamStartPosition.transform.localRotation;


	}
	
	void Update(){



		if (this.transform.localPosition.y >= 75f && this.isActiveAndEnabled) {

			orbitDown = true;
			orbitUp = false;

		}


		if (this.transform.localPosition.y <= 7.5f && this.isActiveAndEnabled) {

			orbitDown = false;
			orbitUp = true;

		}

		if (this.isActiveAndEnabled && orbitDown) { // Orbit and go down

			transform.RotateAround (orbitObject.transform.localPosition, Vector3.up, rotateSpeed * Time.deltaTime);
			transform.Translate (Vector3.down * moveSpeed * Time.deltaTime, Space.World);
		}

		if (this.isActiveAndEnabled && orbitUp) { // Orbit and go up

			transform.RotateAround (orbitObject.transform.localPosition, Vector3.up, rotateSpeed * Time.deltaTime);
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime, Space.World);
		}


	}
}
