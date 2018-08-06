using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardElt : MonoBehaviour {

	public int idx = -1;
	public int pairNum = -1;
	public Text temp;
	public RawImage img;
	public GameObject objImg;
	public PagePairCards parent;

	public bool isFind = false;

	public void SetIdx(int idx, int pairNum)
	{
		this.idx = idx;
		this.pairNum = pairNum;

		temp.text = idx.ToString () + ", " + pairNum.ToString ();
		temp.gameObject.SetActive (true);
	}

	public void SetImg(Texture tex)
	{
		img.texture = tex;
		objImg.SetActive (false);
	}

	public void OnShow()
	{
		if (isFind)
			return;

		parent.CheckPair (idx, pairNum);
		objImg.SetActive (true);
	}

	public void Hide()
	{
		objImg.SetActive (false);
	}		
}
