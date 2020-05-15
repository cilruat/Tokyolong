using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGameGetList : MonoBehaviour {

	public List<Image> ImageList = new List<Image>();

	private void Awake()
	{
		try
		{
			ImageList.Clear();

			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game1);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game2);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game3);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game4);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game5);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game6);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game7);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game8);

		}
		catch(System.Exception e) 
		{
			throw;
		}
		for (int i = 0; i < ImageList.Count; i++) 
		{
			Instantiator (i);
		}

	}

	public void Instantiator(int TargetFromImageList)
	{
		Image RandomImage = Instantiate (ImageList [TargetFromImageList], gameObject.transform);

		switch(TargetFromImageList)
		{
		case 0:
			RandomImage.name = "Game1";
			break;

		case 1:
			RandomImage.name = "Game2";
			break;

		case 2:
			RandomImage.name = "Game3";
			break;

		case 3:
			RandomImage.name = "Game4";
			break;

		case 4:
			RandomImage.name = "Game5";
			break;

		case 5:
			RandomImage.name = "Game6";
			break;

		case 6:
			RandomImage.name = "Game7";
			break;

		case 7:
			RandomImage.name = "Game8";
			break;

		default:
			RandomImage.name = "(none)";
			break;

	}
		RandomImage.gameObject.SetActive (false);



}
}
