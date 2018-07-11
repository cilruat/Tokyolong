using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class AdminOrderDetail : SingletonMonobehaviour<AdminOrderDetail> {

	public Text table;
	public GameObject objPrefab;
	public RectTransform rtScroll;

    RequestOrderMenu reqOrder = null;

    public void SetInfo(RequestOrderMenu reqOrder)
	{
		_Clear ();

        this.reqOrder = reqOrder;
        table.text = reqOrder.tableNo.ToString () + "번 테이블";

        for (int i = 0; i < reqOrder.list.Count; i++)
        {
            EMenuDetail eType = (EMenuDetail) this.reqOrder.list[i].menu;

            string menu = Info.MenuName (eType) + " " +  this.reqOrder.list[i].cnt.ToString ();
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
        JsonData json = JsonMapper.ToJson(reqOrder.list);
        NetworkManager.Instance.Order_Confirm_REQ(reqOrder.id, reqOrder.tableNo, json.ToString());
	}

	public void OnClose() {	gameObject.SetActive (false); }
}
