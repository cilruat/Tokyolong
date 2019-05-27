using UnityEngine;
using System.Collections;

/// <summary>
// Back button. go to title.
/// </summary>
public class BackButton : MonoBehaviour {
	void OnMouseClicked(){
		Application.LoadLevel(0);
	}

	void OnMouseRelease(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(1.6f,1.6f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(1.5f,1.5f,1);
	}
}
