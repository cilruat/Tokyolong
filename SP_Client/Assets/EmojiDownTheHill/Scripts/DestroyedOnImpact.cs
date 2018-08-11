using UnityEngine;
using System.Collections;

public class DestroyedOnImpact : MonoBehaviour {
	public SpawnCubes spawner;

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "wave") {
            spawner.myWaves.Remove (coll.gameObject);
            Destroy(coll.gameObject);
            spawner.InstantiateNewWave();
		}
	}
}
