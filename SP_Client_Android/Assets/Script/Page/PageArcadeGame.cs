using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageArcadeGame : PageBase {

	public CanvasGroup[] cgBoard;
	public List<TabButton> tabButtons;
	public List<GameObject> contentsPanels;
	public GameObject mainCanvas;
	public List<GameObject> tabArrow;

	int selected = 0;




	protected override void Awake ()
	{
		base.boards = cgBoard;
		base.Awake ();
	}


	private void Start()
	{
		ClickTab (selected);
	}



	//go to Pop up Panel
	/*public void OnGameBtn(string sceneName)
	{
		SceneChanger.LoadScene (sceneName, mainCanvas);
	}*/



	public void ClickTab(int id)
	{
		for (int i = 0; i < contentsPanels.Count; i++) 
		{
			if (i == id) {
				contentsPanels [i].SetActive (true);
				tabButtons [i].Selected ();
				tabArrow [i].SetActive (true);
			} 
			else 
			{
				contentsPanels [i].SetActive (false);
				tabButtons [i].DeSelected ();
				tabArrow [i].SetActive (false);
			}
		}

	}

}