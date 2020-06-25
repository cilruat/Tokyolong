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
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game9);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game10);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game11);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game12);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game13);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game14);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game15);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game16);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game17);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game18);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game19);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game20);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game21);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game22);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game23);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game24);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game25);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game26);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game27);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game28);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game29);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game30);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game31);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game32);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game33);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game34);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game35);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game36);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game37);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game38);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game39);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game40);
			ImageList.Add(this.gameObject.GetComponent<RandomGameResources>().Game41);
		}
		catch (System.Exception e) 
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

			case 8:
				RandomImage.name = "Game9";
				break;

			case 9:
				RandomImage.name = "Game10";
				break;

			case 10:
				RandomImage.name = "Game11";
				break;

			case 11:
				RandomImage.name = "Game12";
				break;

			case 12:
				RandomImage.name = "Game13";
				break;

			case 13:
				RandomImage.name = "Game14";
				break;

			case 14:
				RandomImage.name = "Game15";
				break;

			case 15:
				RandomImage.name = "Game16";
				break;

			case 16:
				RandomImage.name = "Game17";
				break;

			case 17:
				RandomImage.name = "Game18";
				break;

			case 18:
				RandomImage.name = "Game19";
				break;

			case 19:
				RandomImage.name = "Game20";
				break;

			case 20:
				RandomImage.name = "Game21";
				break;

			case 21:
				RandomImage.name = "Game22";
				break;

			case 22:
				RandomImage.name = "Game23";
				break;

			case 23:
				RandomImage.name = "Game24";
				break;

			case 24:
				RandomImage.name = "Game25";
				break;

			case 25:
				RandomImage.name = "Game26";
				break;

			case 26:
				RandomImage.name = "Game27";
				break;

			case 27:
				RandomImage.name = "Game28";
				break;

			case 28:
				RandomImage.name = "Game29";
				break;

			case 29:
				RandomImage.name = "Game30";
				break;
			case 30:
				RandomImage.name = "Game31";
				break;
			case 31:
				RandomImage.name = "Game32";
				break;
			case 32:
				RandomImage.name = "Game33";
				break;
			case 33:
				RandomImage.name = "Game34";
				break;
			case 34:
				RandomImage.name = "Game35";
				break;
			case 35:
				RandomImage.name = "Game36";
				break;
			case 36:
				RandomImage.name = "Game37";
				break;
			case 37:
				RandomImage.name = "Game38";
				break;
			case 38:
				RandomImage.name = "Game39";
				break;
			case 39:
				RandomImage.name = "Game40";
				break;

			case 40:
				RandomImage.name = "Game41";
				break;


			default:
			RandomImage.name = "(none)";
			break;

	}
		RandomImage.gameObject.SetActive (false);



}
}
