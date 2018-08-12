using UnityEngine;
using System.Collections;

public class TheTerrian : MonoBehaviour {
	public bool isOwningASpring=true;
	public Sprite sprite2;
	public void ChangeColor(){
		GetComponent<SpriteRenderer> ().sprite = sprite2;
	}

}
