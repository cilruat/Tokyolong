using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
[RequireComponent (typeof (Animator))]


public class EyeMonster : MonoBehaviour {

	[Header("Eye Monster Enemy")]


	private Animator anim;
	public float activatorRange ;// Set here the distance between the player and the enemy to be activated.
	public LayerMask activatorLayer;// Set here the Player layer.
	public bool inActivatorRange = false;// The bool that makes the enemy to know if the player is in range.
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource monsterAudioSource;
	public AudioClip monsterSfx;
	private PauseMenu pause;
	private LevelManager manager;

	void Start () {

		anim = GetComponent<Animator> ();
		pause = FindObjectOfType<PauseMenu> ();
		manager = FindObjectOfType<LevelManager> ();

	}
	
	void Update () {

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive || LevelManager.sfxActive && pause.isPaused) {

			monsterAudioSource.volume = 0;


		} else {

			monsterAudioSource.volume = 1;

		}
		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////

		if (manager.gamePlaying && !pause.isPaused) {
			


		inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;

		if(inActivatorRange){

			anim.SetTrigger ("Spin");

			PlaySound ();
		}
	}
	}
		
	public void PlaySound (){

		if (!monsterAudioSource.isPlaying) {

			monsterAudioSource.PlayOneShot (monsterSfx, 1f);
		}

	}
}
