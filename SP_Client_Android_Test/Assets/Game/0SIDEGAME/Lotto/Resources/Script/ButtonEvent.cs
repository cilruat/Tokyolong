using UnityEngine;
using System.Collections;

/// <summary>
// if you want to make button with sprite gameobject.
// just add this component script.
/// </summary>
public class ButtonEvent : MonoBehaviour {

	private bool _isTouch = false;
	private int status = 0;

	public bool enable = true;

	// check collid mouse point to sprite.
	bool RayCast(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (GetComponent<Collider>().Raycast(ray, out hit, 100.0F)){
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		if (enable == false) return ;
		
		if(Input.GetMouseButton(0)){
			if(_isTouch == false){
				if(RayCast()){
					if(status != 1){ // clicked!
						gameObject.SendMessage("OnMouseClicked");
						status = 1;
					}
				}
			}
			_isTouch = true;
        }else{
        	if(_isTouch){
				if(status != 2){ //mouse release!
					gameObject.SendMessage("OnMouseRelease");
					status = 2;
				}
        	}else{
        		if(RayCast()){
					if(status != 3){ // mouse over!
        				gameObject.SendMessage("OnMouseOver");
						status = 3;
					}
        		}else{
					if(status != 4){//mouse out!
        				gameObject.SendMessage("OnMouseOut");
						status = 4;
					}
        		}
        	}
        	_isTouch = false;
        }
	}
}
