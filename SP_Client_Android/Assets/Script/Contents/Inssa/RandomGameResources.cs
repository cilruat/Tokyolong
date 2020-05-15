using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGameResources : MonoBehaviour {

	public Image Game1, Game2, Game3, Game4, Game5, Game6, Game7, Game8;


	private void Awake()
	{
		Game1 = Resources.Load<Image> ("Prefabs/Game1");
		Game2 = Resources.Load<Image> ("Prefabs/Game2");
		Game3 = Resources.Load<Image> ("Prefabs/Game3");
		Game4 = Resources.Load<Image> ("Prefabs/Game4");
		Game5 = Resources.Load<Image> ("Prefabs/Game5");
		Game6 = Resources.Load<Image> ("Prefabs/Game6");
		Game7 = Resources.Load<Image> ("Prefabs/Game7");
		Game8 = Resources.Load<Image> ("Prefabs/Game8");
	}
}
