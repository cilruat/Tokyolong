using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour {

	Image background;
	public Sprite idleImg;
	public Sprite selectImg;

	private void Awake()
	{
		background = GetComponent<Image> ();
	}

	public void Selected()
	{
		background.sprite = selectImg;
	}

	public void DeSelected()
	{
		background.sprite = idleImg;
	}
}
