using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class AdminTableOrderBill : MonoBehaviour {
    
	public GameObject prefab;
	public RectTransform rtScroll; 
	public Text totalPrice;

    List<TableOrderBillElt> listElt = new List<TableOrderBillElt>();

    byte tableNo = 0;
    public void SetTable(byte tableNo) 
    {
        this.tableNo = tableNo; 
        _Clear();
    }

	public void SetMenu(EMenuDetail eType)
	{
		int findIdx = -1;
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].MenuDetailType () != eType)
				continue;

			findIdx = i;
			break;
		}

		if (findIdx != -1) {
			listElt [findIdx].OnChangeValue (true);
		} else {
			GameObject obj = Instantiate (prefab) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScroll);
			tr.InitTransform ();

            TableOrderBillElt elt = obj.GetComponent<TableOrderBillElt> ();
			elt.SetInfo (eType);
			listElt.Add (elt);

			CalcTotalPrice ();
		}			
	}

	public void CalcTotalPrice()
	{
		int total = 0;
		for (int i = 0; i < listElt.Count; i++)
			total += listElt [i].GetPrice ();
        
		totalPrice.text = Info.MakeMoneyString (total);
	}

	public void RemoveElt(EMenuDetail eType)
	{
		int findIdx = -1;
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].MenuDetailType () != eType)
				continue;

			findIdx = i;
			listElt.RemoveAt (i);
			break;
		}

		if (findIdx == -1)
			return;

		for (int i = 0; i < rtScroll.childCount; i++) {
			if (i != findIdx)
				continue;

			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
			break;
		}

		CalcTotalPrice ();
	}

	void _Clear()
	{
		for (int i = 0; i < rtScroll.childCount; i++) {
			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
		}

		listElt.Clear ();

		CalcTotalPrice ();
	}

	public void FinishOrder()
	{
        if (tableNo == (byte)0)
        {
            SystemMessage.Instance.Add ("테이블 정보가 잘못되었습니다.");
            return;
        }

        if (listElt.Count == 0) 
        {
            SystemMessage.Instance.Add ("주문내역이 없습니다");
            return;
        }

        if (AdminTableOrderInput.Instance.waitComplete)
            return;

		List<SendMenu> list = new List<SendMenu> ();
		for (int i = 0; i < listElt.Count; i++) {
			int menu = (int)listElt [i].MenuDetailType ();
			int cnt = listElt [i].GetCount ();
			SendMenu send = new SendMenu (menu, cnt);
			list.Add (send);
		}

		JsonData json = JsonMapper.ToJson (list);
        NetworkManager.Instance.Table_Order_Input_REQ (this.tableNo, json.ToString ());
        AdminTableOrderInput.Instance.waitComplete = true;
	}
}
