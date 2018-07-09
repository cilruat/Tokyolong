using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageAdmin : SingletonMonobehaviour<PageAdmin> {

	const int TABLE_NUM = 30;
	const int MAX_ID = 1000;

	public RectTransform rtScrollTable;
	public RectTransform rtScrollOrder;
	public RectTransform rtScrollMusic;
	public GameObject prefabTable;
	public GameObject prefabOrder;
	public GameObject prefabMusic;
	public GameObject objTableBoardCover;
	public GameObject objTableMenu;
	public GameObject objBillConfirm;
	public GameObject objOrderDetail;

	UITween tweenUrgency = null;
	List<TableElt> listTable = new List<TableElt>();
	List<OrderElt> listOrder = new List<OrderElt>();
	List<MusicElt> listMusic = new List<MusicElt>();

	int orderID = 0;

	void Awake()
	{
		for (int i = 0; i < TABLE_NUM; i++) {
			GameObject obj = Instantiate (prefabTable) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScrollTable);
			tr.InitTransform ();

			TableElt elt = obj.GetComponent<TableElt> ();

			int tableNum = i + 1;
			elt.SetTable (tableNum);

			listTable.Add (elt);
		}
	}

	public void SetLogin(int tableNo)
	{
		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;
			
			listTable [i].Login ();
			break;
		}
	}

	public void SetLogout(int tableNo)
	{
		// 주문 내역에서도 해당 테이블 주문 지우기
		for (int i = 0; i < listOrder.Count; i++) {
			if (listOrder [i].GetTableNo () != tableNo)
				continue;

			int id = listOrder [i].GetID ();
			RemoveElt (true, id);
			--i;
		}

		// 신청곡 내역에서도 해당 테이블 신청곡 지우기

		StopUrgency (tableNo);

		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			listTable [i].Logout ();
			break;
		}

		if (objBillConfirm.activeSelf)
			objBillConfirm.SetActive (false);
	}

	public void Urgency(int tableNo)
	{
		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			listTable [i].Urgency ();
			break;
		}

		if (tweenUrgency == null)
			tweenUrgency = UITweenAlpha.Start (objTableBoardCover, .25f, 1f, TWParam.New (.8f).Curve (TWCurve.Linear).Loop (TWLoop.PingPong));
	}

	public void StopUrgency(int tableNo)
	{		
		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			listTable [i].StopUrgency ();
			break;
		}

		bool NoStop = false;
		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetUrgency () == false)
				continue;
			
			NoStop = true;
			break;
		}

		if (NoStop == false && tweenUrgency != null) {
			tweenUrgency.StopTween ();
			tweenUrgency = null;
			objTableBoardCover.SetActive (false);
		}
	}

	public void SetOrder(bool isOrder, byte tableNo, string packing)
	{
        GameObject obj = Instantiate (prefabOrder) as GameObject;
		obj.SetActive (true);

		Transform tr = obj.transform;
        tr.SetParent (rtScrollOrder);
		tr.InitTransform ();

        for (int i = 0; i < listTable.Count; i++)
        {
            if (listTable[i].GetTableNo() != tableNo)
                continue;

            listTable[i].SetOrder(packing);
            break;
        }

        OrderElt elt = obj.GetComponent<OrderElt>();
        elt.SetInfo(orderID, tableNo, packing);
        listOrder.Add(elt);

        ++orderID;
        if (orderID > MAX_ID)
            orderID = 0;
	}

	public void SetOrder(byte tableNo, short discount)
	{
		GameObject obj = Instantiate (prefabOrder) as GameObject;
		obj.SetActive (true);

		Transform tr = obj.transform;
		tr.SetParent (rtScrollOrder);
		tr.InitTransform ();

		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			listTable [i].SetOrder (discount);
			break;
		}

		OrderElt elt = obj.GetComponent<OrderElt> ();
		elt.SetInfo (orderID, tableNo, discount);
		listOrder.Add (elt);

		++orderID;
		if (orderID > MAX_ID)
			orderID = 0;
	}

    public void SetRequestMusic(string packing)
    {
        GameObject obj = Instantiate (prefabMusic) as GameObject;
        obj.SetActive (true);

        Transform tr = obj.transform;
        tr.SetParent (rtScrollMusic);
        tr.InitTransform ();

        MusicElt elt = obj.GetComponent<MusicElt> ();
        elt.SetInfo (packing);
        listMusic.Add (elt);
    }

    public void RemoveRequestMusic(int id)
    {
        int removeIdx = -1;
        for (int i = 0; i < listMusic.Count; i++)
        {
            if (id != listMusic[i].GetID())
                continue;

            removeIdx = i;
        }

        if (removeIdx == -1)
            return;

        DestroyImmediate(listMusic[removeIdx].gameObject);
        listMusic.RemoveAt(removeIdx);
    }

	public void RemoveElt(bool isOrder, int id)
	{
		int findIdx = -1;
		int length = isOrder ? listOrder.Count : listMusic.Count;
		for (int i = 0; i < length; i++) {
			int compare = isOrder ? listOrder [i].GetID () : listMusic [i].GetID ();
			if (compare != id)
				continue;

			findIdx = i;
			if (isOrder)	listOrder.RemoveAt (i);
			else			listMusic.RemoveAt (i);
			break;
		}

		if (findIdx == -1)
			return;

		RectTransform rtScroll = isOrder ? rtScrollOrder : rtScrollMusic;
		for (int i = 0; i < rtScroll.childCount; i++) {
			if (i != findIdx)
				continue;

			Transform child = rtScroll.GetChild (i);
			if (child)
				DestroyImmediate (child.gameObject);
			break;
		}
	}		

	public void ShowTableMenu(byte tableNo, List<KeyValuePair<EMenuDetail,int>> list)
	{
		objTableMenu.SetActive (true);
		AdminTableMenu.Instance.SetInfo (tableNo, list);
	}

	public void ShowBillConfirm(byte tableNo, List<KeyValuePair<EMenuDetail,int>> list)
	{
		objBillConfirm.SetActive (true);
		AdminBillConfirm.Instance.SetInfo (tableNo, list);
	}

	public void ShowOrderDetail(byte tableNo, int id, string packing)
	{
		objOrderDetail.SetActive (true);
		AdminOrderDetail.Instance.SetInfo (tableNo, id, packing);
	}

	void Update()
	{
	}		
}
