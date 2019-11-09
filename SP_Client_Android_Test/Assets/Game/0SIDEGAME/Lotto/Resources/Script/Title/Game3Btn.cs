using UnityEngine;
using System.Collections;

/// <summary>
// go to lotto game button .
/// </summary>
public class Game3Btn : MonoBehaviour {
	void OnMouseClicked(){
		Application.LoadLevel(2);
	}

	void OnMouseRelease(){
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(0.95f,0.95f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(0.9f,0.9f,1);
	}
}
