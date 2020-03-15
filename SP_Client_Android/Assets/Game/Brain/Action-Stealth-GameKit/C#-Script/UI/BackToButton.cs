using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace Stealth
{
public class BackToButton : MonoBehaviour {
		
	///*************************************************************************///
	/// This class changes the current scene with a predefined scene (via inspector)
	///*************************************************************************///

	//public string backTo; 		//name of the level we want to load
	public AudioClip tapSfx;
	private bool canTap = true;

	void Update (){
		
		if(canTap)
			StartCoroutine(tapManager());
		
		//also enable the procedure by pressing "Escape" button on keyboard or handheld devices.
		if(Input.GetKeyDown(KeyCode.Escape)) 
				SceneManager.LoadScene ("ArcadeGame");
	}

	//Respnd to click/touch inputs
	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator tapManager (){

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
			
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			if(objectHit.name == "BackButton") {
				canTap = false;
				playSFX(tapSfx);
				yield return new WaitForSeconds(1);
				SceneManager.LoadScene ("ArcadeGame");
			}	
		}
	}

	void playSFX ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
	}
}