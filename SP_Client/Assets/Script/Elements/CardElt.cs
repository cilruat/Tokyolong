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
	public UIButton btn;
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

	public void Find()
	{
		isFind = true;
		btn.scalingOnTouch = false;
		ShiningGraphic.Start (img);
	}

	public void Rolling()
	{
		RectTransform rt = (RectTransform)transform;
		float y = rt.anchoredPosition.y;
		Vector3 start = new Vector3 (0f, y, 0f);
		Vector3 end = new Vector3 (0f, y == 0f ? 180f : 0f, 0f);
		UITweenRotation.Start (gameObject, start, end, TWParam.New (.2f).Curve (TWCurve.CurveLevel2));
	}
}
