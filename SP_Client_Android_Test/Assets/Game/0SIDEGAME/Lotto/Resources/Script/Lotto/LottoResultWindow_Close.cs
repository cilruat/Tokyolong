using UnityEngine;
using System.Collections;

/// <summary>
// LottoResult Close Button.
// Repeat Lotto!
/// </summary>
public class LottoResultWindow_Close : MonoBehaviour {
	void OnMouseClicked(){
		Application.LoadLevel(2);
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
