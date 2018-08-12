using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class RequestOrder
{
    public byte type;
    public int id;
    public byte tableNo;
    public string packing;

    public RequestOrder()
    {
        this.type = 0;
        this.id = -1;
        this.tableNo = 0;
        this.packing = string.Empty;
    }

    public RequestOrder(byte type, int id, byte tableNo, string packing)
    {
        this.type = type;
        this.id = id;
        this.tableNo = tableNo;
        this.packing = packing;
    }
}

public class AdminOrderDetail : SingletonMonobehaviour<AdminOrderDetail> {

	public Text table;
	public GameObject objPrefab;
	public RectTransform rtScroll;
    public Text confrimDesc;

    RequestOrder reqOrder = null;

    public void SetInfo(RequestOrder reqOrder)
	{
		_Clear ();

        this.reqOrder = reqOrder;
        table.text = reqOrder.tableNo.ToString () + "번 테이블";

        switch ((ERequestOrderType)reqOrder.type)
        {
            case ERequestOrderType.eOrder:      SetOrder(reqOrder.packing);     break;
        }
	}

    void SetOrder(string packing)
    {
        JsonData json = JsonMapper.ToObject (reqOrder.packing);
        for (int i = 0; i < json.Count; i++) 
        {
            int menu = int.Parse(json [i] ["menu"].ToString ());
            int cnt =  int.Parse(json [i] ["cnt"].ToString ());

            MenuData data = MenuData.Get(menu);

            string strMenu = data.menuName + " " +  cnt.ToString ();
            GameObject objElt = Instantiate (objPrefab) as GameObject;
            Transform tr = objElt.transform;
            tr.SetParent (rtScroll);
            tr.InitTransform ();
            objElt.SetActive (true);
            Text desc = tr.Find ("Desc").GetComponent<Text> ();
            desc.text = strMenu;
        }

        confrimDesc.text = "주문 내역 확인";
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
        NetworkManager.Instance.Order_Confirm_REQ(reqOrder.type, reqOrder.id, reqOrder.tableNo, reqOrder.packing);
	}

	public void OnClose() {	gameObject.SetActive (false); }
}
