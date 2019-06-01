using UnityEngine;
using System.Collections;

namespace Bowling
{
public class ball : MonoBehaviour {
	public float axisforce;
	public GameObject forcebar;
	bool start = false;
	public float Force;
	public float weight;
	public Transform doorbin;
	public AudioClip bowling1;
	public AudioClip pinhit;
	public GameObject score;
	bool force=false;
	bool activetimer = false;
	public float timer = 7f;
	bool Q;
	AudioSource sound;
	Fbar bar;
	void Start () {
		bar = forcebar.GetComponent<Fbar> ();
		sound = GetComponent<AudioSource> ();
	}
	
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			if(start == false){
			forcebar.SetActive(true);
			}
		}
		if (start == true) {
			GetComponent<Rigidbody> ().AddForce (Vector3.left * (Input.GetAxis ("Mouse X") * axisforce));
		}
		if (Input.GetButtonUp ("Fire1")) {
			if(start == false){
				if(bar.force > 5){
			sound.clip = bowling1;
			sound.Play();
			activetimer = true;
			force = true;
			GetComponent<Animation>().Stop();
			start = true;
			GetComponent<Rigidbody> ().AddForce (Vector3.back*bar.force*Force);
				}
			forcebar.SetActive(false);
			}
		}
		if (Q == false) {
			if (Vector3.Distance (transform.position, doorbin.transform.position) > 30) {
				doorbin.GetComponent<Animation> ().Play ();
				Q = true;
			}
		}
		if (activetimer == true) {
			timer -= Time.deltaTime;
		}
		if(timer <= 0){
			score.SetActive(true);
			activetimer = false;
		}
		if (force == true) {
			GetComponent<Rigidbody> ().AddForce (Vector3.back*8);
		}
	}
}
}