using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageArcadeGame : PageBase {

	public CanvasGroup[] cgBoard;
	public List<TabButton> tabButtons;
	public List<GameObject> contentsPanels;
	public GameObject mainCanvas;

	// Why?
	/*protected override void Awake ()
	{
		//base.boards = cgBoard;
		//base.Awake ();
	}*/
		
	public void OnGameBtn(string sceneName)
	{
		SceneChanger.LoadScene (sceneName, mainCanvas);
	}


	public void ClickTab(int id)
	{
		for (int i = 0; i < contentsPanels.Count; i++) 
		{
			if (i == id) {
				contentsPanels [i].SetActive (true);
			} 
			else 
			{
				contentsPanels [i].SetActive (false);
			}
		}

	}

}