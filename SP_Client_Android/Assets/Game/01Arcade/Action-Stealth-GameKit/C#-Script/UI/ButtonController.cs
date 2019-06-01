using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace Stealth
{
public class ButtonController : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Menu Buttons Controller.
	///*************************************************************************///

	void Update (){	
		StartCoroutine(tapManager());
	}

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
			objectHit.GetComponent<Renderer>().material.color = Color.red;
			yield return new WaitForSeconds(0.5f);
			switch(objectHit.name) {
				case "Example-01":
				SceneManager.LoadScene ("ExampleScene-01");
					break;
				case "Example-02":
				SceneManager.LoadScene ("ExampleScene-02");
					break;
				case "Example-03":
				SceneManager.LoadScene ("ExampleScene-03");
					break;
				case "Example-04":
				SceneManager.LoadScene ("ExampleScene-04");
					break;
				case "Example-05":
				SceneManager.LoadScene ("ExampleScene-05");
					break;
				case "Example-06":
				SceneManager.LoadScene ("ExampleScene-06");
					break;
				case "Example-07":
				SceneManager.LoadScene ("ExampleScene-07");
					break;
				case "Example-08":
				SceneManager.LoadScene ("ExampleScene-08");
					break;
				case "Example-09":
				SceneManager.LoadScene ("ExampleScene-09");
					break;
				case "Example-10":
				SceneManager.LoadScene ("ExampleScene-10");
					break;
				case "Example-11":
				SceneManager.LoadScene ("ExampleScene-11");
					break;
				case "Example-12":
				SceneManager.LoadScene ("ExampleScene-12");
					break;
				case "Example-13":
				SceneManager.LoadScene ("LiveExample-01");
					break;
				case "Example-14":
				SceneManager.LoadScene ("LiveExample-02");
					break;
				case "Example-15":
				SceneManager.LoadScene ("LiveExample-03");
					break;
			}	
		}
	}
	}
}