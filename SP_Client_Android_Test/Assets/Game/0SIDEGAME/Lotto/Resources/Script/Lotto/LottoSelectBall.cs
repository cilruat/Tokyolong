using UnityEngine;
using System.Collections;

/// <summary>
// Player Picking Lotto Ball in Select Window
/// </summary>
public class LottoSelectBall : MonoBehaviour {
	public int type=0;
	public bool _toggle=false;

	// how many pick balls
	int getToggleNum(){
		int num = 0;
		LottoSelectBall[] toggles = FindObjectsOfType(typeof(LottoSelectBall)) as LottoSelectBall[];
		foreach(LottoSelectBall toggle in toggles){
			if(toggle._toggle){
				num++;
			}
		}
		return num;
	}

	// if 3 more pick, do not working.
	void OnMouseClicked(){
		_toggle = !_toggle;
		if(_toggle){
			if(getToggleNum() <= 3){
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 1f);
			}else{
				_toggle = false;
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			}
		}else{
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
	}

	void OnMouseRelease(){
		transform.localScale = new Vector3(1f,1f,1);
	}

	void OnMouseOver(){
		transform.localScale = new Vector3(1.1f,1.1f,1);
	}

	void OnMouseOut(){
		transform.localScale = new Vector3(1f,1f,1);
	}
}
