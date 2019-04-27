//When the button with a box collider is pressed, then it will load a level.

using UnityEngine;
using System.Collections;

public class NewGameScript : MonoBehaviour {

	void Start () {
	}

	void Update () {

	}
		
	void OnMouseDown() {
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
