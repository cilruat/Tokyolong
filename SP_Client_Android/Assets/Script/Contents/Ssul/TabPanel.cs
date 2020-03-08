using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class TabPanel : MonoBehaviour {

	public List<TabButton> tabButtons;
	public List<GameObject> contentsPanels;

	int selected = 0;

	private void Start()
	{
		ClickTab (selected);
	}
		
	public void ClickTab(int id)
	{
		for (int i = 0; i < contentsPanels.Count; i++) 
		{
			if (i == id) {
				contentsPanels [i].SetActive (true);
				tabButtons [i].Selected ();
			} 
			else 
			{
				contentsPanels [i].SetActive (false);
				tabButtons [i].DeSelected ();

			}
		}

	}
}
