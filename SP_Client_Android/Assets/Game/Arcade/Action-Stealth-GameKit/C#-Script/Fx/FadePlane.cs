using UnityEngine;
using System.Collections;
namespace Stealth
{
public class FadePlane : MonoBehaviour {
		
	///*************************************************************************///
	/// Fader class
	/// Fade the scene smoothly at the begining and at the end (game-over)
	///*************************************************************************///

	//make the plane fully visible
	void Awake (){
		GetComponent<Renderer>().material.color = new Color(1,1,1,1);
	}

	void Start (){
		StartCoroutine(fadeOut());
	}

	//At the start of the scene, gently fade from white to total transparent
	IEnumerator fadeOut (){
		transform.position = new Vector3(transform.position.x, 3, transform.position.z);
		GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
		                                    GetComponent<Renderer>().material.color.g,
		                                    GetComponent<Renderer>().material.color.b,
		                                    1);
		GetComponent<MeshCollider>().enabled = true;		
		//fade to total transparency
		float t = 0.0f;
		while(t < 1.0f) {
			t += Time.deltaTime * 1.0f;
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
			                                    GetComponent<Renderer>().material.color.g,
			                                    GetComponent<Renderer>().material.color.b,
			                                    1 - t);
			yield return 0;
		}
		if(t >= 1.0f) {
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
			                                    GetComponent<Renderer>().material.color.g,
			                                    GetComponent<Renderer>().material.color.b,
			                                    0);
			GetComponent<MeshCollider>().enabled = false;
			transform.position = new Vector3(transform.position.x,
			                                 -3,
			                                 transform.position.z);
		}
	}
	}
}