using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// YOU BETTER RUN By BITBOYS STUDIO.
public class TowerMovement : MonoBehaviour {

	[Header("Tower Movement")]

	public float moveSpeed = 1.3f; // The tower move down velocity
	public float thisPosition; // The actual position of the tower
	private LevelManager manager; // call the leve manager script.
	private GameObject rotationReference; // Set here the rotation reference object.
	private Vector3 startPos; // used to store the tower initial position
	private Quaternion startRot; // used to store tower initial rotation.
	private Vector3 lastPos; // used to store the last position of the tower when game play stops.

	void OnEnable(){

		manager = FindObjectOfType<LevelManager> ();
		rotationReference = GameObject.FindGameObjectWithTag ("Rotator"); // When the tower is spawned search an object with the "Rotator" Tag and follow its rotation. (In this case, the level manager object)
		startPos = this.gameObject.transform.localPosition;
		startRot = this.gameObject.transform.localRotation;
	}


	void FixedUpdate(){

		// if the tower object position is less than -30 it activates the spawn and destroy function.
		if (thisPosition <= -30f ) { 

			SpawnAndDestroy();
		}

		/////////////////////////////////////////////////////////////////////////

		// RESET GAME TOWER RESETING
		if (manager.resettingAll){ // This function is called from the level manager script, used to reset the towers position and rotation.

			this.gameObject.transform.localPosition = startPos;
			this.gameObject.transform.localRotation = startRot;
			manager.towersAmount = manager.towersAmount - 1;

			Destroy (this.gameObject);

		}
		//////////////////////////////////////////////////////
	
		// used to store the last position of the tower when game play stops.
		this.lastPos = this.gameObject.transform.localPosition;


		if (manager.resetTowerRotation){ // This function is called from the level manager script, used to reset the tower rotation when player continue the game.

			this.gameObject.transform.localPosition = lastPos;
			this.gameObject.transform.localRotation = rotationReference.transform.localRotation;

		}

	}

	void Update(){


		if (manager.gamePlaying) { // while the game is playing the tower will be moving down and rotating.


			thisPosition = this.gameObject.transform.localPosition.y; // Store the Y axis position every frame.
			this.transform.localRotation = rotationReference.transform.localRotation; // copy the object reference rotation.
			transform.Translate (Vector3.down * moveSpeed * Time.deltaTime, Space.Self);// Move down the tower object.

		}
	}
		
	public void SpawnAndDestroy()
	{

		if (manager.towersAmount >= 12) { // To ensure if we can spawn a new tower and destroy this, we answer the level manager if the actual amount of towers in scene are bigger or equal to 12.
			manager.towersAmount = manager.towersAmount - 1; // Substract the destroyed tower from the total towers amount in scene.
			manager.totalTowersSpawned = manager.totalTowersSpawned + 1; // Add 1 tower to the total amount of towers spawned.
			manager.SpawnTower (); // Call the level manager to spawn a new tower.
			Destroy (this.gameObject); // finally destroy this object.

		}

	}

}