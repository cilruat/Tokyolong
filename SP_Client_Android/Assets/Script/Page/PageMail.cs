using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class PageMail : SingletonMonobehaviour<PageMail>{

    [System.Serializable]
    public class ShowMsg
    {
        public GameObject obj;
        public Text No;
        public Text Msg;
    }

	//지역상수설정
	public const int TABLE_NUM = 32;

	//테이블셋팅부터합니다

	public GameObject page;
	public RectTransform rtScrollTable;

    public GameObject objBoard;
	public GameObject prefabTable;
	public GameObject objTableBoardCover;
	public GameObject objTableMenu;
	public GameObject objMsgWrite;
    public GameObject objLike;

    public RectTransform rtScrollMail;
    public GameObject prefabMail;
    public RectTransform rtScrollLike;
    public GameObject prefabLike;
    public RectTransform rtScrollPresent;
    public GameObject prefabPresent;
    public RectTransform rtScrollPlz;
    public GameObject prefabPlz;

    public GameObject SamDuk;
    public GameObject Young;


    public ShowMsg showMsg;

    UITween tweenUrgency = null;


    List<TableElt> listTable = new List<TableElt>();
    List<MailElt> listMailelt = new List<MailElt>();
    List<LikeElt> listLikeelt = new List<LikeElt>();
    List<PresentElt> listPresentelt = new List<PresentElt>();
    List<PlzElt> listPlzelt = new List<PlzElt>();


    void Awake()
    {
        LoadTable();

        for (int i = 0; i < Info.listLoginTable.Count; i++)
            SetLogin(Info.listLoginTable[i]);
    }


    void Start()
    {
        for (int i = 0; i < Info.myInfo.listMsgInfo.Count; i++)
            CreateMailElt(Info.myInfo.listMsgInfo[i]);

        for (int i = 0; i < Info.myInfo.listLikeInfo.Count; i++)
            CreateLikeElt(Info.myInfo.listLikeInfo[i]);

        for (int i = 0; i < Info.myInfo.listPresentInfo.Count; i++)
            CreatePresentElt(Info.myInfo.listPresentInfo[i]);

        for (int i = 0; i < Info.myInfo.listPlzInfo.Count; i++)
            CreatePlzElt(Info.myInfo.listPlzInfo[i]);

    }

    void LoadTable()
	{
		for (int i = 0; i < TABLE_NUM; i++) 
		{
			GameObject obj = Instantiate(prefabTable) as GameObject;
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

	public void SetData(string tablePacking)
	{
		LoadTable ();
    }


    public void SetLogin(int tableNo)
	{
		for (int i = 0; i < listTable.Count; i++) 
		{
			if (listTable [i].GetTableNo () != tableNo)
				continue;

            listTable [i].Login ();
			break;
		}

	}

    public void SetLogout(int tableNo)
	{
		StopUrgency (tableNo);

		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			listTable [i].Logout ();
			break;
		}
		//LogOut시 해야할행동들 추가
	}

	//계속 접속 시도하게 하는거 체크?
	public bool CheckTableLogin(int tableNo)
	{
		for (int i = 0; i < listTable.Count; i++) {
			if (listTable [i].GetTableNo () != tableNo)
				continue;

			if (listTable [i].IsLogin ())
				return true;
		}
		return false;
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
    public void SetMail(UserMsgInfo info)
    {
        CreateMailElt(info);
    }

    void CreateMailElt(UserMsgInfo info)
    {
        GameObject obj = Instantiate(prefabMail) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollMail);
        tr.InitTransform();

        MailElt elt = obj.GetComponent<MailElt>();
        elt.SetInfo(info);

        listMailelt.Add(elt);
    }

    public void SetLike(UserLikeInfo info)
    {
        CreateLikeElt(info);
    }

    void CreateLikeElt(UserLikeInfo info)
    {
        GameObject obj = Instantiate(prefabLike) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollLike);
        tr.InitTransform();

        LikeElt elt = obj.GetComponent<LikeElt>();
        elt.SetInfo(info);

        listLikeelt.Add(elt);
    }

    public void SetPresent(UserPresentInfo info)
    {
        CreatePresentElt(info);
    }

    void CreatePresentElt(UserPresentInfo info)
    {
        GameObject obj = Instantiate(prefabPresent) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPresent);
        tr.InitTransform();

        PresentElt elt = obj.GetComponent<PresentElt>();
        elt.SetInfo(info);

        listPresentelt.Add(elt);
    }

    public void SetPlz(UserPlzInfo info)
    {
        CreatePlzElt(info);
    }

    void CreatePlzElt(UserPlzInfo info)
    {
        GameObject obj = Instantiate(prefabPlz) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPlz);
        tr.InitTransform();

        PlzElt elt = obj.GetComponent<PlzElt>();
        elt.SetInfo(info);

        listPlzelt.Add(elt);
    }

    public void DeleteMailElt(MailElt elt)
    {
        for (int i = 0; i < Info.myInfo.listMsgInfo.Count; i++)
        {
            //인포에 들어있는 특정한 정보를 꺼낼때 이렇게 쓸것
            UserMsgInfo msg = Info.myInfo.listMsgInfo[i];
            if (msg.tableNo != elt.GetTableNo())
                continue;

            Info.myInfo.listMsgInfo.RemoveAt(i);
            break;
        }

        listMailelt.Remove(elt);
        Destroy(elt.gameObject);
    }


    public void DeletePlzElt(PlzElt elt)
    {
        for (int i = 0; i < Info.myInfo.listPlzInfo.Count; i++)
        {
            //인포에 들어있는 특정한 정보를 꺼낼때 이렇게 쓸것
            UserPlzInfo msg = Info.myInfo.listPlzInfo[i];
            if (msg.tableNo != elt.GetTableNo())
                continue;

            Info.myInfo.listPlzInfo.RemoveAt(i);
            break;
        }

        listPlzelt.Remove(elt);
        Destroy(elt.gameObject);
    }

    public void DeletePresentElt(PresentElt elt)
    {
        for (int i = 0; i < Info.myInfo.listPresentInfo.Count; i++)
        {
            //인포에 들어있는 특정한 정보를 꺼낼때 이렇게 쓸것
            UserPresentInfo msg = Info.myInfo.listPresentInfo[i];
            if (msg.tableNo != elt.GetTableNo())
                continue;

            Info.myInfo.listPresentInfo.RemoveAt(i);
            break;
        }

        listPresentelt.Remove(elt);
        Destroy(elt.gameObject);
    }

    public void DeleteLikeElt(LikeElt elt)
    {
        for (int i = 0; i < Info.myInfo.listLikeInfo.Count; i++)
        {
            //인포에 들어있는 특정한 정보를 꺼낼때 이렇게 쓸것
            UserLikeInfo msg = Info.myInfo.listLikeInfo[i];
            if (msg.tableNo != elt.GetTableNo())
                continue;

            Info.myInfo.listLikeInfo.RemoveAt(i);
            break;
        }

        listLikeelt.Remove(elt);
        Destroy(elt.gameObject);
    }


    //나중에 배열만들어서 정리하자 일단 그냥지금넣기

    public void OpenMap()
    {

        SamDuk.SetActive(true);

    }

    public void CloseMap()
    {

        SamDuk.SetActive(false);

    }

    public void OpenMapYoung()
    {
        Young.SetActive(true);
    }

    public void CloseMapYoung()
    {

        Young.SetActive(false);
    }



    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }

}
