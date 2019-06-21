using UnityEngine;
using System.Collections;

namespace OnetapSoccer
{
public class ScaleAnimator : MonoBehaviour {

	///*********************************************************
	/// Simple animator class for Goal gfx object
	///*********************************************************

	private Vector3 startingPosition;
	private Vector3 startingScale;
	private Vector3 targetScale;


	void Start (){
		init();
		StartCoroutine(increaseScale());
		StartCoroutine(move());
	}


	///*********************************************************
	/// Init
	///*********************************************************
	void init (){
		startingPosition = transform.position;
		startingScale = transform.localScale;
		targetScale = new Vector3 (5.0f, 2.5f, 0.001f);
	}


	///*********************************************************
	/// Animate the text by ncreasing it's scale
	///*********************************************************
	IEnumerator increaseScale (){
		float t = 0.0f;
		while(t < 1.0f) {
			t += Time.deltaTime * 1.0f;
			transform.localScale = new Vector3 (Mathf.SmoothStep (startingScale.x, targetScale.x, t), 
				Mathf.SmoothStep (startingScale.y, targetScale.y, t), 
				startingScale.z);
			yield return 0;
		}
	}


	///*********************************************************
	/// Move the text upwards.
	///*********************************************************
	IEnumerator move (){
		float t = 0.0f;
		while(t < 1.5f) { 
			t += Time.deltaTime * 1.0f;
			transform.position = new Vector3(transform.position.x, startingPosition.y + (t * 2), transform.position.z);

			if(t >= 1.5f)
				Destroy(gameObject);
			
			yield return 0;
		}
	}


}
}