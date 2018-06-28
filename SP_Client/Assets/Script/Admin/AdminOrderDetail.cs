using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class AdminOrderDetail : SingletonMonobehaviour<AdminOrderDetail> {

	public Text table;
	public GameObject objPrefab;
	public RectTransform rtScroll;

	int id = -1;

	public void SetInfo(byte tableNo, int id, string packing)
	{
		_Clear ();

		this.id = id;
		table.text = tableNo.ToString () + "번 테이블";

		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++) {			
			string json1 = json [i] ["menu"].ToString ();
			string json2 = json [i] ["cnt"].ToString ();

			EMenuDetail eType = (EMenuDetail)int.Parse (json1);
			int cnt = int.Parse (json2);

			string menu = Info.MenuName (eType) + " " + cnt.ToString ();

			GameObject objElt = Instantiate (objPrefab) as GameObject;

			Transform tr = objElt.transform;
			tr.SetParent (rtScroll);
			tr.InitTransform ();
			objElt.SetActive (true);

			Text desc = tr.Find ("Desc").GetComponent<Text> ();
			desc.text = menu;
		}
	}

	void _Clear()
	{
		for (int i = 0; i < rtScroll.childCount; i++) {
			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
		}
	}

	public void OnConfirm()
	{
		_Clear ();

		PageAdmin.Instance.RemoveElt (true, id);
		OnClose ();
	}

	public void OnClose() {	gameObject.SetActive (false); }
}
