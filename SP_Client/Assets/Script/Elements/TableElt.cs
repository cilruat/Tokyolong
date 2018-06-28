using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableElt : MonoBehaviour {

	public Text tableNum;
	public GameObject objCover;
	public GameObject objUrgency;

	bool isLogin = false;
	bool isUrgency = false;
	int tableNo = 0;
	UITween tweenUrgency;

	public void SetTable(int num)
	{
		this.tableNo = num;
		tableNum.text = num.ToString ();
	}

	public void Login()
	{
		isLogin = true;
		objCover.SetActive (true);
	}

	public void Logout()
	{
		isLogin = false;
		objCover.SetActive (false);
		StopUrgency ();
	}

	public void Urgency()
	{
		if (isLogin == false || isUrgency)
			return;

		isUrgency = true;

		if (tweenUrgency == null)
			tweenUrgency = UITweenAlpha.Start (objUrgency, .25f, 1f, TWParam.New (.8f).Curve (TWCurve.Linear).Loop (TWLoop.PingPong));

		objCover.SetActive (false);
	}

	public void StopUrgency()
	{
		if (isUrgency == false)
			return;

		isUrgency = false;

		if (tweenUrgency != null) {
			tweenUrgency.StopTween ();
			tweenUrgency = null;
		}

		objUrgency.SetActive (false);
		objCover.SetActive (true);
	}
		
	public int GetTableNo() { return this.tableNo; }
	public bool GetUrgency() { return isUrgency; }
}
