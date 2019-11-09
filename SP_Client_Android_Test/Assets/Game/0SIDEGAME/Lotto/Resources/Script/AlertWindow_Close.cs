using UnityEngine;
using System.Collections;

/// <summary>
// Alert Window Close Button 
// if clicked this button, just run close animation alert window. 
/// </summary>
public class AlertWindow_Close : MonoBehaviour {
	void OnMouseClicked(){
		GameObject window = transform.parent.gameObject;
		Animator anim = window.GetComponent<Animator>();
		anim.Play("AlertDisappear");
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
