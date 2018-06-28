using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class TableElt : MonoBehaviour {

	public Text tableNum;
	public GameObject objCover;
	public GameObject objUrgency;

	bool isLogin = false;
	bool isUrgency = false;
	byte tableNo = 0;
	UITween tweenUrgency;

	Dictionary<EMenuDetail, int> dictOrder = new Dictionary<EMenuDetail, int>();

	public void SetTable(int num)
	{
		this.tableNo = (byte)num;
		tableNum.text = num.ToString ();
	}

	public void SetOrder(string packing)
	{
		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++) {
			string json1 = json [i] ["menu"].ToString ();
			string json2 = json [i] ["cnt"].ToString ();

			EMenuDetail eType = (EMenuDetail)int.Parse (json1);
			int cnt = int.Parse (json2);
			_SetOrder (eType, cnt);
		}
	}

	void _SetOrder(EMenuDetail eMenu, int cnt)
	{
		if (dictOrder.ContainsKey (eMenu))
			dictOrder [eMenu] += cnt;
		else
			dictOrder.Add (eMenu, cnt);
	}
		
	public void Login()
	{
		isLogin = true;
		objCover.SetActive (true);
	}

	public void Logout()
	{		
		isLogin = false;
		dictOrder.Clear ();
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

	public void OnTableMenu()
	{
		if (isLogin == false)
			return;

		List<KeyValuePair<EMenuDetail,int>> list = new List<KeyValuePair<EMenuDetail, int>> ();
		foreach (KeyValuePair<EMenuDetail, int> pair in dictOrder)
			list.Add (pair);

		PageAdmin.Instance.ShowTableMenu (tableNo, list);
	}
		
	public byte GetTableNo() { return this.tableNo; }
	public bool GetUrgency() { return isUrgency; }
}
