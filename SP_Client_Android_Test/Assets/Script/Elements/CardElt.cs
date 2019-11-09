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
		temp.gameObject.SetActive (false);
	}

	public void SetImg(Texture tex)
	{
		img.texture = tex;
		objImg.SetActive (false);
	}

	public void OnShow()
	{
		if (isFind || parent.start == false || parent.end)
			return;

		parent.CheckPair (idx, pairNum);
		Rolling (true);
	}

	public void Hide()
	{
		Rolling (false);
	}

	public void Find()
	{
		isFind = true;
		btn.scalingOnTouch = false;
		ShiningGraphic.Start (img);
	}

	public void Rolling(bool show)
	{
		StartCoroutine (_Rolling (show));
	}

	IEnumerator _Rolling(bool show)
	{
		RectTransform rt = (RectTransform)transform;
		float y = rt.localEulerAngles.y;
		float end_y = show ? 180f : 0f;

		Vector3 start = Vector3.up * y;
		Vector3 end = Vector3.up * end_y;

		while (true) {
			Vector3 rot = Vector3.MoveTowards (start, end, 15f);
			rt.Rotate (rot, Space.Self);

			if (rt.localEulerAngles.y >= end_y * .5f) {
				if (objImg.activeSelf != show)
					objImg.SetActive (show);
			}

			if (rt.localEulerAngles.y >= end_y)
				break;

			yield return null;
		}

		rt.localEulerAngles = new Vector3 (0f, end_y, 0f);
	}
}
