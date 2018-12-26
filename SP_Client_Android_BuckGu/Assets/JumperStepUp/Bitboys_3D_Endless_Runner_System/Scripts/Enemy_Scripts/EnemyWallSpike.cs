using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class EnemyWallSpike : MonoBehaviour {

	[Header("Wall Spike Enemy")]

	public float activatorRange ;// Set here the distance between the player and the enemy to be activated.
	public LayerMask activatorLayer;// Set here the Player layer.
	public bool inActivatorRange = false;// The bool that makes the enemy to know if the player is in range.

	public float spikeSpeed = 2f;
	public float minPos;
	public float maxPos;
	public bool goOut = false;
	public bool goIn = false;

	private PauseMenu pause;
	private LevelManager manager;

	void Start () {
		pause = FindObjectOfType<PauseMenu> ();
		manager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (manager.gamePlaying && !pause.isPaused) {  // Only if the player is near activate the movement to avoid bad performance.

			inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;

			if(inActivatorRange){

		
		

		if (this.transform.localPosition.z >= minPos) { // Change the movement direction when the spike position is less than the minimum desired pos.

			goOut = true;
			goIn = false;
		}


				if (this.transform.localPosition.z <= maxPos) {// Change the movement direction when the spike position is less than the maximum desired pos.

			goOut = false;
			goIn = true;

		}


		if (goIn) {

			transform.Translate (Vector3.forward * spikeSpeed * Time.deltaTime, Space.Self);

		}

		if (goOut) {

			transform.Translate (Vector3.back * spikeSpeed * Time.deltaTime, Space.Self);
		}
		
	}
}
	}
}
