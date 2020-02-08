using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour {

	public Image Fire;
	public float speed = 2.0f;
	public float waitTime = 5.0f;
	public bool repeat;



	IEnumerator Start(){
		while (repeat) {
			yield return ChangeFill (0.0f, 1.0f, waitTime);
			yield return ChangeFill (1.0f, 0.0f, waitTime);
		}
	
	}

	public IEnumerator ChangeFill(float start, float end, float time){

		float i = 0.0f;
		float rate = (1.0f / time) * speed;

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			Fire.fillAmount = Mathf.Lerp (start, end, i);
			yield return null;
		}

	}
}
