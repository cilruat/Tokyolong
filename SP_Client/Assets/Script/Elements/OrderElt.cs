using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class OrderElt : MonoBehaviour {

	public Text table;
	public Text order;

	int id = -1;
	byte tableNo = 0;
	string packing = "";

	public void SetInfo(int id, byte tableNo, string packing)
	{
		this.id = id;
		this.tableNo = tableNo;
		this.packing = packing;

		table.text = tableNo.ToString ();

		string desc = "";
		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++) {
			string json1 = json [i] ["menu"].ToString ();
			string json2 = json [i] ["cnt"].ToString ();

			EMenuDetail eType = (EMenuDetail)int.Parse (json1);
			int cnt = int.Parse (json2);

			desc += Info.MenuName (eType) + " " + cnt.ToString ();
			if (i < json.Count - 1)
				desc += ", ";
		}

		order.text = desc;
	}

	public void OnDetail()
	{
		PageAdmin.Instance.ShowOrderDetail (tableNo, id, packing);
	}

	public int GetID() { return id; }
	public byte GetTableNo() { return tableNo; }
}
