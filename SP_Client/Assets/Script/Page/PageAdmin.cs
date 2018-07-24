using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class PageAdmin : SingletonMonobehaviour<PageAdmin> {

	const int TABLE_NUM = 42;

	public GameObject page;
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
    public GameObject objTableOrder;
	public UnfinishGameList unfinishGame;

	UITween tweenUrgency = null;
	List<TableElt> listTable = new List<TableElt>();
	List<OrderElt> listOrder = new List<OrderElt>();
	List<MusicElt> listMusic = new List<MusicElt>();

    void Awake()
    {
        SetData(Info.adminTablePacking, Info.adminOrderPacking, Info.adminMusicPacking);
    }

	void LoadTable()
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

    public void SetData(string tablePacking, string orderPacking, string musicPacking)
    {
        LoadTable();

        JsonData tableJson = JsonMapper.ToObject(tablePacking);
        for (int i = 0; i < tableJson.Count; i++)
            SetLogin(int.Parse(tableJson[i].ToString()));

        JsonData orderJson = JsonMapper.ToObject(orderPacking);
        for (int i = 0; i < orderJson.Count; i++)
        {
            byte type = byte.Parse(orderJson[i]["type"].ToString());
            int id = byte.Parse(orderJson[i]["id"].ToString());
            byte tableNo = byte.Parse(orderJson[i]["tableNo"].ToString());
            string packing = orderJson[i]["packing"].ToString();

            SetOrder(new RequestOrder(type, id, tableNo, packing));
        }

        JsonData musicJson = JsonMapper.ToObject(musicPacking);
        for (int i = 0; i < musicJson.Count; i++)
        {
            int id = byte.Parse(musicJson[i]["id"].ToString());
            byte tableNo = byte.Parse(musicJson[i]["tableNo"].ToString());
            string title = musicJson[i]["title"].ToString();
            string singer = musicJson[i]["singer"].ToString();

            SetRequestMusic(new RequestMusicInfo(id, tableNo, title, singer));
        }

        Info.adminTablePacking = "";
        Info.adminOrderPacking = "";
        Info.adminMusicPacking = "";
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
        for (int i = 0; i < listMusic.Count; i++) {
            if (listMusic [i].GetTableNo () != tableNo)
                continue;

            int id = listMusic [i].GetID ();
            RemoveElt (false, id);
            --i;
        }


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

    public void SetOrder(RequestOrder reqOrder)
    {
        GameObject obj = Instantiate (prefabOrder) as GameObject;
        obj.SetActive (true);

        Transform tr = obj.transform;
        tr.SetParent (rtScrollOrder);
        tr.InitTransform ();

        OrderElt elt = obj.GetComponent<OrderElt>();
        elt.SetInfo(reqOrder);
        listOrder.Add(elt);
    }

    public void SetRequestMusic(RequestMusicInfo info)
    {
        MusicElt elt = CreateMusicElt();
        elt.SetInfo(info);
    }

    public void SetRequestMusic(string packing)
    {
        MusicElt elt = CreateMusicElt();
        elt.SetInfo(packing);
    }

    MusicElt CreateMusicElt()
    {
        GameObject obj = Instantiate (prefabMusic) as GameObject;
        obj.SetActive (true);

        Transform tr = obj.transform;
        tr.SetParent (rtScrollMusic);
        tr.InitTransform ();

        MusicElt elt = obj.GetComponent<MusicElt> ();
        listMusic.Add (elt);

        return elt;
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

	public void ShowTableMenu(byte tableNo)
	{
		objTableMenu.SetActive (true);
		AdminTableMenu.Instance.SetInfo (tableNo);
	}

    public void ShowBillConfirm(byte tableNo, string orderPacking, string discountPacking)
    {
        List<KeyValuePair<EMenuDetail, int>> listOrder = new List<KeyValuePair<EMenuDetail, int>>(); 
        JsonData json = JsonMapper.ToObject(orderPacking);
        for (int i = 0; i < json.Count; i++)
        {
            int menu = int.Parse(json[i]["menu"].ToString());
            int cnt = int.Parse(json[i]["cnt"].ToString());

            EMenuDetail eType = (EMenuDetail)menu;
            listOrder.Add(new KeyValuePair<EMenuDetail, int>(eType, cnt));
        }

        List<short> listDiscount = new List<short>();
        JsonData jsonDiscount = JsonMapper.ToObject(discountPacking);
        for (int i = 0; i < jsonDiscount.Count; i++)
            listDiscount.Add(short.Parse(jsonDiscount[i].ToString()));

        ShowBillConfirm(tableNo, listOrder, listDiscount);
    }

    public void ShowBillConfirm(byte tableNo, List<KeyValuePair<EMenuDetail,int>> listOrder, List<short> listDiscount)
	{
		objBillConfirm.SetActive (true);
        AdminBillConfirm.Instance.SetInfo (tableNo, listOrder, listDiscount);
	}

    public void ShowOrderDetail(RequestOrder reqOrder)
	{
        if (reqOrder == null)
            return;

		objOrderDetail.SetActive (true);
        AdminOrderDetail.Instance.SetInfo (reqOrder);
	}

    public void ShowTableOrder(byte tableNo)
    {
        objTableOrder.SetActive (true);
        AdminTableOrderInput.Instance.SetTable(tableNo);
    }

	public void ShowUnfinishGameList(string packing)
	{
		AdminTableMenu.Instance.OnClose ();
		unfinishGame.SetInfo (packing);
		unfinishGame.gameObject.SetActive (true);
	}

	public void RemoveUnfinishGame(int id)
	{
		unfinishGame.RemoveUnfinish (id);
	}

	void Update()
	{
	}		
}
