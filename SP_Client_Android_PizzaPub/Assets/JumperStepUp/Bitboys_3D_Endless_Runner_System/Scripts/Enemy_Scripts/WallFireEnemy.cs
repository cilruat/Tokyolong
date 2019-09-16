using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class WallFireEnemy : MonoBehaviour {

	[Header("Wall Fire Enemy")]

	public float activatorRange = 6f ;// Set here the distance between the player and the enemy to be activated.
	public LayerMask activatorLayer;// Set here the Player layer.
	public bool inActivatorRange = false;// The bool that makes the enemy to know if the player is in range.

	public GameObject fireParticles;

	private PauseMenu pause;
	private LevelManager manager;

	void Start () {
		pause = FindObjectOfType<PauseMenu> ();
		manager = FindObjectOfType<LevelManager> ();
	}

	// Update is called once per frame
	void Update () {

		if (manager.gamePlaying && !pause.isPaused) {

			inActivatorRange = Physics.OverlapSphere (transform.position, activatorRange, activatorLayer).Length > 0f;

			if (inActivatorRange) { // Only if the player is near activate the fire partices to avoid bad performance.

				fireParticles.gameObject.SetActive (true);

			} else {
				fireParticles.gameObject.SetActive (false);


			}

			}
		}
	}

