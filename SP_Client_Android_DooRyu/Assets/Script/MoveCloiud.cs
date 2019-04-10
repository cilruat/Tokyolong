using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloiud : MonoBehaviour {
	private float TimeLeft = 10.0f;
	private float nextTime = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3 (-1, 0, 0) * Time.deltaTime);
	}
}
