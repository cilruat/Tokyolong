using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public enum ERequestOrderType
{
    eNone = 0,
    eOrder = 1,
}

public class OrderElt : MonoBehaviour {
    
	public Text table;
	public Text order;
	public GameObject objDetail;

    RequestOrder reqOrder = null;

    public void SetInfo(RequestOrder reqOrder)
    {
        this.reqOrder = reqOrder;

        table.text = this.reqOrder.tableNo.ToString ();

        switch ((ERequestOrderType)reqOrder.type)
        {
            case ERequestOrderType.eOrder:      SetOrder(reqOrder.packing);     break;
        }
    }

    void SetOrder(string packing)
    {
        List<SendMenu> listSendMenu = new List<SendMenu>();
        string desc = "";
        JsonData json = JsonMapper.ToObject (packing);
        for (int i = 0; i < json.Count; i++) 
        {
            int menu = int.Parse(json [i] ["menu"].ToString ());
            int cnt =  int.Parse(json [i] ["cnt"].ToString ());
            listSendMenu.Add(new SendMenu(menu, cnt));

            MenuData data = MenuData.Get(menu);

            desc += data.menuName + " " + cnt.ToString ();
            if (i < json.Count - 1)
                desc += ", ";
        }

        order.text = desc;
        objDetail.SetActive (true);
    }        

	public void OnDetail()
	{
        if (reqOrder == null)
        {
            SystemMessage.Instance.Add("요청 목록에 오류가 발생 하였습니다");
            return;
        }

        PageAdmin.Instance.ShowOrderDetail(reqOrder);
	}

    public int GetID() { return reqOrder.id; }
    public byte GetTableNo() { return reqOrder.tableNo; }
}
