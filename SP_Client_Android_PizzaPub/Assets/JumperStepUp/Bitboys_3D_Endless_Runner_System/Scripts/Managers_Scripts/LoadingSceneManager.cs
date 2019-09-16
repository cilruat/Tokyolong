using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 3D ENDLESS RUNNER SYSTEM By BITBOYS STUDIO.
public class LoadingSceneManager : MonoBehaviour
{
	public string sceneToLoad; // the name of the scene that will charge after the loading scene.
	public Image loadingBar; // Here we put the loading bar image (under the editor)
	private AsyncOperation async; // call the async operation function to sync the loading bar image fill whit the scene loading progress.

	 IEnumerator Start() // This function is called on the scene start.
	{
		yield return new WaitForSeconds (0.1f);

		async = SceneManager.LoadSceneAsync (sceneToLoad); // We indicated that the variable means that we're calling the scene manager to load our scene.

	}

	void Update(){

		if (async != null) { // until the scene it's not completely loaded we fill the loading bar image, from 0 to 1.
			loadingBar.fillAmount = async.progress;	
		}
	}



}