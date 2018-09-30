using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class EnemyOrbit : MonoBehaviour {

	[Header("Ghost Enemy Orbit Movement")]


	public GameObject orbitObject; // Set the orbit object to make the ghost orbit around it.
	public float speed;
	private LevelManager manager;
	private PauseMenu pause;

	public float movementAmplitude = 0.02f;
	public float movementPeriod = 0.1f;

	public float activatorRange ;// Set here the distance between the player and the enemy to be activated.
	public LayerMask activatorLayer;// Set here the Player layer.
	public bool inActivatorRange = false;// The bool that makes the enemy to know if the player is in range.
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource ghostAudioSource;
	public AudioClip ghostSfx;


	void Start () {

		manager = FindObjectOfType<LevelManager> ();
		pause = FindObjectOfType<PauseMenu> ();

	}
	
	void Update () {

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive || LevelManager.sfxActive && pause.isPaused) {

			ghostAudioSource.volume = 0;

		} else {

			ghostAudioSource.volume = 1;


		}
		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////

		if (manager.gamePlaying && !pause.isPaused) {
			


		inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;


			OrbitAround ();
			floating ();

		if(inActivatorRange){


			PlaySound ();
	  }
	 }
	}

	void OrbitAround (){

		transform.RotateAround (orbitObject.transform.localPosition, Vector3.up, speed * Time.deltaTime);

	
	}

	void floating (){

		float theta = Time.timeSinceLevelLoad / movementPeriod;
		float distance = movementAmplitude * Mathf.Sin(theta);
		transform.localPosition = transform.localPosition + Vector3.up * distance;
	}

	public void PlaySound (){

		if (!ghostAudioSource.isPlaying) {

			ghostAudioSource.PlayOneShot (ghostSfx, 1f);
		}

	}

}
