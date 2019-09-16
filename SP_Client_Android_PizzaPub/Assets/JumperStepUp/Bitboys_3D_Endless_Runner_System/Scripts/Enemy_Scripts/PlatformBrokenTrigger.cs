using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class PlatformBrokenTrigger : MonoBehaviour {

	public float lifetime = 1f;
	public GameObject brokenPlatform;
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource platformAudioSource;
	public AudioClip platformSfx;

	void Start () {

	}

	// Update is called once per frame
	void Update () {

		// Use this to can set the music on and off with the setting buttons
		if (!LevelManager.sfxActive) {

			platformAudioSource.volume = 0;


		} else {

			platformAudioSource.volume = 1;

		}



		///////////////////////////////////////
	}

	void OnCollisionEnter (Collision coll){

		if (coll.gameObject.tag == "Player") {

			Fall ();

		}
	}

	public void Fall(){

		this.gameObject.GetComponent<MeshCollider> ().isTrigger = true;
		this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
		platformAudioSource.PlayOneShot (platformSfx, 1f);


		brokenPlatform.SetActive (true);

		//Instantiate(brokenPlatform , this.transform.position,  this.transform.rotation);

		Destroy (this.gameObject, lifetime);

	}
}
