using UnityEngine;
using System.Collections;

/// <summary>
// Pachinko Betting button
/// </summary>
public class Pinball_BetBtn : MonoBehaviour {
	public int betCoins = 0;
	public bool toggle;
	
	//toggled buttons! 
	//if you toggle on some button, other button will be toggle off.
	void OnMouseClicked(){
		GameObject[] btns = GameObject.FindGameObjectsWithTag("BetBtn");
		for(int i =0;i<btns.Length;i++){
			btns[i].SendMessage("toggleOff");
		}
		toggleOn();
		transform.localScale = new Vector3(1.4f,1.4f,1);
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

	void toggleOff(){
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Texture/button_oval");

		toggle = false;
	}

	void toggleOn(){
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Texture/button_oval2");

		toggle = true;
	}
}
