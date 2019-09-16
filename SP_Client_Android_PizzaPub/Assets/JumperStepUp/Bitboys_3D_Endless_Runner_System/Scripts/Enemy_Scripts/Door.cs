using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class Door : MonoBehaviour {


	[Header("Door Enemy")]
	public GameObject door; // The door object (Not the frame part)
	public float doorOpenDegrees; /// The door open amount degrees
	public float doorCloseDegrees; // The door close amount degrees
	public float openDoorSpeed = 20f;
	public float closeDoorSpeed = 20f;


	public float activatorRange ;
	public LayerMask activatorLayer;
	public bool inActivatorRange = false;

	public bool doorOpened = false;

	private Quaternion startRot;

	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource doorAudioSource;
	public AudioClip doorSfx;
	public AudioSource closeDoorAudioSource;
	public AudioClip closeDoorSfx;

	void Start(){

		startRot= door.gameObject.transform.localRotation;

	}
	void Update () {

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive) {

			doorAudioSource.volume = 0;
			closeDoorAudioSource.volume = 0;


		} else {

			doorAudioSource.volume = 1;
			closeDoorAudioSource.volume = 1;

		}



		///////////////////////////////////////

		inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;


		if (inActivatorRange) {

			Open ();

		}

		if (!inActivatorRange && doorOpened) {


		
			Close ();

		}

	}

	public void PlaySound (){

		if (!doorAudioSource.isPlaying) {

			doorAudioSource.PlayOneShot (doorSfx, 1f);
		}

	}
	public void PlayCloseSound (){

		if (!closeDoorAudioSource.isPlaying) {

			closeDoorAudioSource.PlayOneShot (closeDoorSfx, 1f);
		}

	}


	public void Open(){

		StartCoroutine(OpenDoor()); 

		PlaySound ();

	}

	public void Close(){

		StartCoroutine(CloseDoor()); 


	}

	public IEnumerator OpenDoor()
	{


		doorOpened = true;


		Quaternion DoorOpenRotation = Quaternion.AngleAxis(doorOpenDegrees, Vector3.up);
		door.gameObject.transform.localRotation= Quaternion.Slerp(door.gameObject.transform.localRotation, DoorOpenRotation, openDoorSpeed * Time.deltaTime);



		yield return null;

	}

	public IEnumerator CloseDoor()
	{

	

		yield return new WaitForSeconds(1f);

		PlayCloseSound ();


		door.gameObject.transform.localRotation= Quaternion.Slerp(door.gameObject.transform.localRotation, startRot, closeDoorSpeed * Time.deltaTime);


		doorOpened = false;

		yield return null;


}


}
