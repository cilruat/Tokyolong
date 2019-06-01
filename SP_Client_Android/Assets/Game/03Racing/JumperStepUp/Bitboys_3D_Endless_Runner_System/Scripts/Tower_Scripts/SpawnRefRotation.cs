using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumperStepUp
{
// YOU BETTER RUN By BITBOYS STUDIO.
public class SpawnRefRotation : MonoBehaviour { // This script is attached to the Level Manager object. It manages the towers rotation.

	[Header("Tower Rotation Reference Object")]

	public float rotationSpeed = 61f; // Set the tower rotation speed.
	private LevelManager manager; // Call the level manager script.
	private Vector3 startPos; // Used to store the initial position of the reference object.
	private Quaternion startRot; // Used to store the initial rotation of the reference object.

	void Awake(){

		manager = FindObjectOfType<LevelManager> ();
		startPos = this.gameObject.transform.localPosition;
		startRot = this.gameObject.transform.localRotation;
	}
		
	void Update () {

		if (manager.gamePlaying) { // connect with the level manager script to know if the game play has started.

			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.Self); // Start rotating the object and the towers will rotate at the same time.
		}

		if (manager.resettingAll || manager.resetTowerRotation) { // connect with the level manager script to know if the script is calling the reset position and rest rotation bools when the player resets the game in the game over scene.

			this.gameObject.transform.localPosition = startPos; // Set the tower position to its initial position.
			this.gameObject.transform.localRotation = startRot; // Set the tower rotation to its initial rotation.

		}

	}
	}
}