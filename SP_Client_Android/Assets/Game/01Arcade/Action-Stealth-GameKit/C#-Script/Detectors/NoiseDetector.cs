using UnityEngine;
using System.Collections;
namespace Stealth
{
public class NoiseDetector : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Noise Detector class.
	/// This class modifies the scale of a sphere to make it bigger by time. 
	/// If player collide with the sphere, it trigger the alarm.
	///*************************************************************************///

	public float detectionRate = 5.0f; 		//every N Seconds to scale Up/Down
	public float growSpeed = 1.0f;			//speed of scale up/down procedure.

	private float minSize;
	private float maxSize;
	private bool  isDetectingFlag; 	//is in the middle of animation?
	private float nextTime;

	///*************************************************************************///
	/// Simple Init
	///*************************************************************************///
	void Start (){
		minSize = 0.9f;
		maxSize = 5.0f;
		isDetectingFlag = false;
		transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		nextTime = detectionRate;
	}

	void Update (){
		if(!isDetectingFlag && Time.timeSinceLevelLoad > nextTime)
			StartCoroutine(scaleManager());
	}

	///*************************************************************************///
	/// This function modifies this detector's scales to make it look like a noise detector.
	///*************************************************************************///
	IEnumerator scaleManager (){
		isDetectingFlag = true;
		float t = 0.0f;
		while(t < 1.0f) {
			t += Time.deltaTime * growSpeed;
			transform.localScale = new Vector3(Mathf.SmoothStep(minSize, maxSize, t),
			                                   transform.localScale.y,
			                                   Mathf.SmoothStep(minSize, maxSize, t));
			yield return 0;
		}
		if(transform.localScale.x >= maxSize) {
			float t2 = 0.0f;
			while(t2 < 1.0f) {
				t2 += Time.deltaTime * growSpeed;
				transform.localScale = new Vector3(Mathf.SmoothStep(maxSize, minSize, t2),
				                                   transform.localScale.y,
				                                   Mathf.SmoothStep(maxSize, minSize, t2));
				yield return 0;
			}
			if(transform.localScale.x <= minSize) {
				nextTime += detectionRate;
				isDetectingFlag = false;
				yield break;
			}
		}	
	}
	}
}