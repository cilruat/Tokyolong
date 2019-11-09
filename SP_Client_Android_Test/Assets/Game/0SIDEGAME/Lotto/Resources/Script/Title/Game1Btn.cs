using UnityEngine;
using System.Collections;

/// <summary>
// go to slot game button .
/// </summary>
public class Game1Btn : MonoBehaviour {
	void OnMouseClicked(){
		Application.LoadLevel(3);
	}

	void OnMouseRelease(){
		Debug.Log("Release!!!!!!");
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(0.95f,0.95f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(0.9f,0.9f,1);
	}
}
