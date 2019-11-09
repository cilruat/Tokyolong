using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
namespace Turnbase
{
public class PauseManager : MonoBehaviour {
		
	/// <summary>
	/// This class manages pause and unpause states.
	/// </summary>
 
	public static bool isPaused;			//is the game in pause mode? (flag)		
	public GameObject pausePlane;			//pausePlane Gameobject which covers the whole scene

	enum Page {PLAY, PAUSE}
	private Page currentPage = Page.PLAY;


	void Awake (){		
		isPaused = false;
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f;
		AudioListener.volume = 1.0f;
		if(pausePlane)
	    	pausePlane.SetActive(false); 
	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update() {
		touchManager();
		//optional pause for debug
		if(Input.GetKeyUp(KeyCode.Escape)) {
			//PAUSE THE GAME
			switch (currentPage) {
	            case Page.PLAY: 
	            	PauseGame(); 
	            	break;
	            case Page.PAUSE: 
	            	UnPauseGame(); 
	            	break;
	            default: 
	            	currentPage = Page.PLAY;
	            	break;
	        }
		}
	}


	/// <summary>
	/// Touch manager on Pause, Reset, Continue, Again, ... buttons
	/// </summary>
	RaycastHit hitInfo;
	Ray ray;
	void touchManager() {
		if(Input.GetMouseButtonDown(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo)) {
				string objectHitName = hitInfo.transform.gameObject.name;
				switch(objectHitName) {
					case "Btn-Pause":
					case "Btn-Resume":
						switch (currentPage) {
				            case Page.PLAY: 
				            	PauseGame();
				            	break;
				            case Page.PAUSE: 
				            	UnPauseGame(); 
				            	break;
				            default: 
				            	currentPage = Page.PLAY;
				            	break;
				        }
						break;
					
					case "Btn-Reset":
					case "Btn-Quit":
					case "Btn-Again":
						PlayerPrefs.DeleteAll ();
						SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
						break;
						
					case "Btn-Next":
						SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
						break;
						
				}
			}
		}
	}


	/// <summary>
	/// Pauses the game.
	/// </summary>
	void PauseGame() {
		print("Game is Paused...");
		isPaused = true;
	    Time.timeScale = 0.0f;
	    AudioListener.volume = 0;
	    if(pausePlane)
	    	pausePlane.SetActive(true);
	    currentPage = Page.PAUSE;
	}


	/// <summary>
	/// Unpause the game.
	/// </summary>
	void UnPauseGame() {
		print("Resume");
	    isPaused = false;
	    Time.timeScale = 1.0f;
	    AudioListener.volume = 1.0f;
		if(pausePlane)
	    	pausePlane.SetActive(false);   
	    currentPage = Page.PLAY;
	}
	}
}