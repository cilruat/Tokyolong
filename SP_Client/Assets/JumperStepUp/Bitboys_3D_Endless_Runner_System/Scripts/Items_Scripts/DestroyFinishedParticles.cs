using UnityEngine;
using System.Collections;
// Sets that the object must be destroyed when the particle system stops to play.
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class DestroyFinishedParticles : MonoBehaviour {

	private ParticleSystem thisParticleSystem;

	
	void Start () {
		thisParticleSystem = GetComponent<ParticleSystem> ();
	}
	// Update is called once per frame
	void Update () {
	if (thisParticleSystem.isPlaying)
		return;
		Destroy (gameObject);
	}

	void OnBecameInvisible()
	{

		Destroy (gameObject);
	}
}

