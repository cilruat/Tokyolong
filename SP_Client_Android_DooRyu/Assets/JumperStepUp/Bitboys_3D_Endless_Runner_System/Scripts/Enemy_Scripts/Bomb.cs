using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
[RequireComponent (typeof (Animator))]
public class Bomb : MonoBehaviour {


	[Header("Bomb Enemy")]
	public float activatorRange ; // Set here the distance between the player and the enemy to be activated.
	public LayerMask activatorLayer; // Set here the Player layer.
	public bool inActivatorRange = false; // The bool that makes the enemy to know if the player is in range.
	private Animator anim; // the animator component.
	[Header("Enemy Die - Particles Prefab")]
	public GameObject dieEffects; 
	private LevelManager manager; // call the level manager script
	private PauseMenu pause; // Call the Pause menu script
	private int instantiatedParticles = 0; // We use this to know if the die effects are alredy instantiated and avoid to instantiate repeated prefabs because the function is called in void update.
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource bombAudioSource; // Set here the bomb enemy first sound audio source 
	public AudioClip bombSfx; // The first effect audio clip
	public AudioSource explodeAudioSource; // the second sound effect audio source
	public AudioClip explodeSfx; // Set the explosion audio effect clip



	void Start(){

		instantiatedParticles = 0; // Set the instantiated particles amount to 0 in the start.
		anim = this.GetComponent<Animator> ();
		manager = FindObjectOfType<LevelManager> ();
		pause = FindObjectOfType<PauseMenu> ();

	}
	void Update () {

		//AUDIO STUFF//
		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive || LevelManager.sfxActive && pause.isPaused) {

			bombAudioSource.volume = 0;
			explodeAudioSource.volume = 0;

		} else {

			bombAudioSource.volume = 1;
			explodeAudioSource.volume = 1;

		}

		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////		///////////////////////////////////////
		

		if (manager.gamePlaying && !pause.isPaused) {


			inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;


			if (inActivatorRange) { // If the player is near the enemy the bomb starts the bomb explode coroutine.

				StartCoroutine (BombExplode ()); 

			}

		}

	}

	public void PlaySound (){

		if (!bombAudioSource.isPlaying) { // this method is good to avoid repeated sounds. We only play the sound one time if the audio source is stopped.

			bombAudioSource.PlayOneShot (bombSfx, 1f);
		}

	}



	public IEnumerator BombExplode()
	{

		PlaySound (); // Call the first sound effect

		anim.SetBool ("Explode", true);  // Call the inflate animation


		yield return new WaitForSeconds(0.5f); // Wait before instantiate the particle prefab

	
		BombEffects (); // Instantiate the particles prefab


		this.gameObject.GetComponent<Renderer> ().enabled = false; // Disable the object renderer instead of destroy them to avoid fps drops.

		anim.SetBool ("Explode", false); // Set the inflate animation to false.


		yield return new WaitForSeconds(1f); // Wait


		this.gameObject.SetActive (false); // Disable the object completely instead of destroy them to avoid fps drops.
	
	}

	public void BombEffects(){

		if (instantiatedParticles == 0) {//We use this to know if the die effects are alredy instantiated and avoid to instantiate repeated prefabs because the function is called in void update.

			Instantiate (dieEffects, this.transform.position, Quaternion.identity); // Instantiates the particles prefab on the enemy position
			instantiatedParticles = 1; // Set the int to 1 to avoid spawn repetition.
		}
		if (!explodeAudioSource.isPlaying) { // Play the explode sound

			explodeAudioSource.PlayOneShot (explodeSfx, 1f);
		}
	}
	}



}
