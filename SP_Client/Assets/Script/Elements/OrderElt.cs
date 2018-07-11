using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class OrderElt : MonoBehaviour {

	public Text table;
	public Text order;
	public GameObject objDetail;

    RequestOrderMenu reqOrder = null;

	int id = -1;
	byte tableNo = 0;

	public void SetInfo(int id, byte tableNo, string packing)
	{
		table.text = tableNo.ToString ();

        List<SendMenu> listSendMenu = new List<SendMenu>();
        string desc = "";
		JsonData json = JsonMapper.ToObject (packing);
		for (int i = 0; i < json.Count; i++) 
        {
            int menu = int.Parse(json [i] ["menu"].ToString ());
            int cnt =  int.Parse(json [i] ["cnt"].ToString ());
            listSendMenu.Add(new SendMenu(menu, cnt));

            EMenuDetail eType = (EMenuDetail)menu;

			desc += Info.MenuName (eType) + " " + cnt.ToString ();
			if (i < json.Count - 1)
				desc += ", ";
		}

        reqOrder = new RequestOrderMenu(id, tableNo, listSendMenu);

        this.id = reqOrder.id;

		order.text = desc;
		objDetail.SetActive (true);
	}

	public void SetInfo(int id, byte tableNo, short discount)
	{
		this.id = id;
		this.tableNo = tableNo;

		table.text = tableNo.ToString ();

		string desc = "";
		if (discount == (short)PageGame.EDiscount.e500won)
			desc = "-500원 할인";
		else if (discount == (short)PageGame.EDiscount.e1000won)
			desc = "-1000원 할인";

		order.text = "게임 성공 (" + desc + ")";
		objDetail.SetActive (false);
	}

	public void OnDetail()
	{
        PageAdmin.Instance.ShowOrderDetail (reqOrder);
	}

	public int GetID() { return id; }
	public byte GetTableNo() { return tableNo; }
}
