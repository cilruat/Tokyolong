using UnityEngine;
using System.Collections;
namespace Stealth
{
public class MatAlphaRandomizer : MonoBehaviour {
		
	///*************************************************************************///
	/// This class will randomize a renderer's alpha value to fake a flick 
	///*************************************************************************///

	public bool isActive = false;
	public float flickPowerBase = 0.16f;
	public float flickDelay = 0.08f;
	private float startTime = 0.0f;

	void Update (){
		if(isActive)
			flick();
	}

	void flick (){
		if(Time.timeSinceLevelLoad  > startTime) {
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
			                                    GetComponent<Renderer>().material.color.g,
			                                    GetComponent<Renderer>().material.color.b,
			                                    flickPowerBase + (Random.value/1.2f));
			startTime += flickDelay;
		}
	}
	}
}