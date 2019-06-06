using UnityEngine;
using System.Collections;
namespace Turnbase
{
public class ScaleAnimator : MonoBehaviour {

	/// <summary>
	/// This class animates the selectionCircle object around the layer units
	/// by modifying it's X and Z scales.
	/// </summary>
		
	private float intensity = 1.2f;		//scale intensity
	private float animSpeed = 1.0f;		//scale speed
	
	private bool animationFlag;			//can cycle the animation?
	private float startScaleX;			//starting scale for X axis
	private float startScaleZ;			//starting scale for Z axis
	private float endScaleX;			//destination scale for X axis
	private float endScaleZ;			//destination scale for Z axis


	/// <summary>
	/// Init
	/// </summary>
	void Start (){
		animationFlag = true;
		startScaleX = transform.localScale.x;
		startScaleZ = transform.localScale.z;
		endScaleX = startScaleX * intensity;
		endScaleZ = startScaleZ * intensity;
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update() {
		//run the scale animation
		if(animationFlag) {
			animationFlag = false;
			StartCoroutine(animate(this.gameObject));
		}
	}


	/// <summary>
	/// Animate the scales of given game object.
	/// </summary>
	/// <returns>Void</returns>
	/// <param name="_btn">GameObject</param>
	IEnumerator animate(GameObject _obj) {
		yield return new WaitForSeconds(0.01f);
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * 1.5f * animSpeed;
			_obj.transform.localScale = new Vector3(Mathf.SmoothStep(startScaleX, endScaleX, t),
			                                        _obj.transform.localScale.y,
			                                        Mathf.SmoothStep(startScaleZ, endScaleZ, t));
			yield return 0;
		}
		
		float r = 0.0f; 
		if(_obj.transform.localScale.x >= endScaleX) {
			while (r <= 1.0f) {
				r += Time.deltaTime * 1.5f * animSpeed;
				_obj.transform.localScale = new Vector3(Mathf.SmoothStep(endScaleX, startScaleX, r),
				                                        _obj.transform.localScale.y,
				                                        Mathf.SmoothStep(endScaleZ, startScaleZ, r));
				yield return 0;
			}
		}
		
		if(_obj.transform.localScale.x <= startScaleX) {
			yield return new WaitForSeconds(0.01f);
			animationFlag = true;
		}
	}
	}
}