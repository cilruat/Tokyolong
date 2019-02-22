using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[System.NonSerialized]
	public bool isUse = false;
	public Rigidbody2D rigid;
	public PageAvoidBullets parent;

	bool isShow = false;
	Vector2 dir = Vector2.zero;

	void Update()
	{
		if (isShow == false && parent.CheckInnerBox (((RectTransform)transform).anchoredPosition))
			isShow = true;
		else if (isShow && parent.CheckInnerBox (((RectTransform)transform).anchoredPosition) == false) {
			isShow = false;
			isUse = false;
			gameObject.SetActive (false);
		}

		if (isShow)
			rigid.velocity = dir * parent.SPEED_BULLET;
	}

	public void SetPos(Vector2 pos)
	{
		isShow = false;
		((RectTransform)transform).anchoredPosition = pos;

		if (gameObject.activeSelf == false)
			gameObject.SetActive (true);
	}

	public void SetDir(Vector2 dir)	{ this.dir = dir; }
}
