using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableElt : MonoBehaviour {

	public Text tableNum;
	public GameObject objCover;
	public GameObject objUrgency;

	public void SetTable(int num)
	{
		tableNum.text = num.ToString ();
	}

	public void Login()
	{
		objCover.SetActive (true);
	}

	public void Urgency()
	{
		if (objCover.activeSelf)
			objCover.SetActive (true);
		
		UITweenAlpha.Start (objUrgency, .25f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).Loop (TWLoop.PingPong));
	}
}
