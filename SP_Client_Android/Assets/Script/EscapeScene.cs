using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EscapeScene : MonoBehaviour {

	public GameObject mainCanvas;

	public void OnSceneChanger(string sceneName)
	{
		SceneChanger.LoadScene (sceneName, mainCanvas);
	}
}
