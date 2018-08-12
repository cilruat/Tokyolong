using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public enum ERequestOrderType
{
    eNone = 0,
    eOrder = 1,
    eDiscount,
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
            case ERequestOrderType.eDiscount:   SetDiscount(reqOrder.packing);  break;
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

    void SetDiscount(string packing)
    {
        string desc = "";
        short discount = short.Parse(packing);
        if (discount == (short)EDiscount.e5000won)
            desc = "-5000원 할인";
        else if (discount == (short)EDiscount.e1000won)
            desc = "-1000원 할인";

        order.text = "게임 성공 (" + desc + ")";
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
