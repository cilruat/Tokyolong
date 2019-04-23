using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This scripts is for all texts that appears for a short time. Used for score pop-ups and bonus alerts.
public class TextAnimate : MonoBehaviour {

	public Transform pivot;
	public bool rotate;
	public float speed;
	public float fadeSpeed;
	private Color color;
	private Vector3 startPos;
	private float timer;
	private float fadeDelay = 0.4f;
	
	void Awake () {
		startPos = transform.position;
	}
	
	
	 void OnEnable(){
		color = GetComponent<Text>().color;
		if(pivot != null) {
			transform.position = Camera.main.WorldToScreenPoint(pivot.position);
		} else {
			transform.position = startPos;
		}
		
		if(rotate)
			transform.eulerAngles = new Vector3(0,0,-179);
	}
	
	void OnDisable (){
		color.a = 1;
		GetComponent<Text>().color = color;
		timer = 0;
	}
	
	void Update () {
		timer += Time.deltaTime;
		if(timer > fadeDelay) {
			if(color.a > 0.01f) {
				color.a -= 0.01f*fadeSpeed;
				GetComponent<Text>().color = color;
			} else {
				gameObject.SetActive(false);
			}
		}
		transform.position += new Vector3(0,Time.deltaTime*100*speed,0);
		if(rotate)
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler( 0, 0, 0 ), Time.deltaTime*10);
	}
}
