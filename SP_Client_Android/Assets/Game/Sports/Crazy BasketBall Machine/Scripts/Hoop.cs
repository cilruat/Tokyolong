using UnityEngine;
using System.Collections;

public class Hoop : MonoBehaviour {


	public AudioClip bouncedSound;

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="BasketBall")
			//if(collision.relativeVelocity.magnitude>5)
			AudioSource.PlayClipAtPoint(bouncedSound, gameObject.transform.position);
	}
}
