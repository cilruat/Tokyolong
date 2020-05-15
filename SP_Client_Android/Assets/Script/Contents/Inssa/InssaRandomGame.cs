using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InssaRandomGame : MonoBehaviour {

	System.Random InssaRandomizer = new System.Random(); //여기 클래스에서 랜덤 선언?

	Transform RandomPanel;

	public void Randomize()
	{
		RandomPanel = GameObject.Find ("Canvas").gameObject.transform.GetChild (0).gameObject.transform;


		for (int i = 0; i < RandomPanel.childCount; i++) 
		{
			RandomPanel.GetChild (i).gameObject.SetActive (false);
		}

		switch (InssaRandomizer.Next (0, GameObject.Find ("Canvas").gameObject.transform.GetChild (0).gameObject.GetComponent<RandomGameGetList> ().ImageList.Count)) 
		{
		case 0:
			RandomPanel.GetChild (0).gameObject.SetActive (true);
			break;

		case 1:
			RandomPanel.GetChild (1).gameObject.SetActive (true);
			break;

		case 2:
			RandomPanel.GetChild (2).gameObject.SetActive (true);
			break;

		case 3:
			RandomPanel.GetChild (3).gameObject.SetActive (true);
			break;

		case 4:
			RandomPanel.GetChild (4).gameObject.SetActive (true);
			break;

		case 5:
			RandomPanel.GetChild (5).gameObject.SetActive (true);
			break;

		case 6:
			RandomPanel.GetChild (6).gameObject.SetActive (true);
			break;

		case 7:
			RandomPanel.GetChild (7).gameObject.SetActive (true);
			break;

		default:
			break;

		}

	}
		
}
