using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class CoinToUi : MonoBehaviour {

	[Header("Flying Coins")]


	private float speed = 12f ; // The coin movement speed
	public float SpinSpeed = 360.0f; // Constant rotation speed
	public Vector3 endPos; // stores the last position of the object life before get back to its start position.
	public Vector3 startPos; // store the coin object inital position.
	[Header("Sound Stuff *(Volume Controlled from the Mixer)")]
	public AudioSource chestAudioSource;
	public AudioClip chestSfx; 


	void Update () {
		
		// Use this to can set the sfx on and off with the setting buttons
		if (!LevelManager.sfxActive) {

			chestAudioSource.volume = 0;

		} else {

			chestAudioSource.volume = 1;

		}

		////////////////////////////////////////////////////////////////////////////////////////////////

		if (isActiveAndEnabled) { // If this object is active in the scene we activate its movement.

			this.transform.rotation *= Quaternion.AngleAxis (SpinSpeed * Time.deltaTime, transform.up); // make the coins spin while go to the end position.

			float step = speed * Time.deltaTime;

			transform.position = Vector3.MoveTowards (this.transform.position, endPos, step); // smooth move the coin from its initial position to the end position.	

		}

		if (this.transform.position == endPos) { // if the coin reaches the end position it goes back to its initial position.

			PlaySound (); // Call the function that plays the chest sound effect.

			this.gameObject.SetActive (false); // disable the object

			this.transform.position = startPos; // after disable the object changes the position to the initial position.

		}
	}
	public void PlaySound (){


	chestAudioSource.PlayOneShot (chestSfx, 1f);

	}
	}
}
