using UnityEngine;
using System.Collections;

namespace BSK
{
//This script made for ball in menu scene that makes it throw automaticly with high chances for goal
public class AutoThrow : MonoBehaviour {
	
	private Rigidbody ballRigidbody;
	private Vector3 startPos;
	private AudioSource audioSource;
	private bool complete;
	
	void Start(){
		ballRigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		startPos = transform.position;
		StartCoroutine(Born());
	}
	
	public IEnumerator Born(){
		yield return new WaitForSeconds(3);
		transform.position = startPos;
		ballRigidbody.isKinematic = true;
		yield return new WaitForSeconds(1);
		ThrowBall();
		complete = false;
	}
	
	public void Complete(){
		if(!complete)
			StartCoroutine(Born());
		complete = true;
	}
	
	public void ThrowBall(){
		float rand = Random.Range(-0.3f,0.3f);
		ballRigidbody.isKinematic = false;
        ballRigidbody.AddForce(new Vector3(-5+rand, 9+rand, 0),ForceMode.Impulse);
        ballRigidbody.AddTorque(0,0,-30);
        ballRigidbody.constraints = RigidbodyConstraints.None;
	}
	
	void OnTriggerEnter(Collider other){
		switch (other.gameObject.name) {
			case "trigger2":
				PlayRandomClip(SoundController.data.ballImpactNet);
				Complete();
			break;
		}
	}
	
	void OnCollisionEnter(Collision other){
		switch (other.gameObject.tag){
			case "ring":
				PlayRandomClip(SoundController.data.ballImpactRing);
			break;
			
			case "floor":
				Complete();
			break;
			case "board":
				PlayRandomClip(SoundController.data.ballImpactSheet);
			break;
			case "pole":
				PlayRandomClip(SoundController.data.ballImpactPole);
			break;
			case "net":
				PlayRandomClip(SoundController.data.ballImpactNet);
			break;
			
		}
	}
	
	void PlayRandomClip(AudioClip[] clips){
		float speed = Mathf.Clamp(ballRigidbody.velocity.magnitude, 0, 15);
		
		audioSource.pitch = 1.15f - speed/50;
		audioSource.PlayOneShot(clips[Random.Range(0,clips.Length)],speed/8);
	}
}
}