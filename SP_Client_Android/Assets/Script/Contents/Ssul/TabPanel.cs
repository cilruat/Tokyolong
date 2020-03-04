using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class TabPanel : MonoBehaviour {

	public List<TabButton> tabButtons;
	public List<GameObject> contentsPanels;

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
