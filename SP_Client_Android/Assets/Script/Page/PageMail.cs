using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class PageMail : SingletonMonobehaviour<PageMail>{
	//지역상수설정
	public const int TABLE_NUM = 30;

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

    UITween tweenUrgency = null;

	List<TableElt> listTable = new List<TableElt>();

    List<MailElt> listMailelt = new List<MailElt>();
    List<LikeElt> listLikeelt = new List<LikeElt>();
    List<PresentElt> listPresentelt = new List<PresentElt>();
    List<PlzElt> listPlzelt = new List<PlzElt>();


    void Awake()
	{
		//일단 테이블 정보만
		SetData (Info.adminTablePacking);
        //DontDestroyOnLoad 예외처리까지 해야되
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

		if (string.IsNullOrEmpty (tablePacking) == false) 
		{
			JsonData tableJson = JsonMapper.ToObject (tablePacking);
			for (int i = 0; i < tableJson.Count; i++)
				SetLogin (int.Parse (tableJson [i].ToString ()));
		}
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


	//Urgency가 뭐지
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
        MailElt elt = CreateMailElt();
        elt.SetInfo(info);
    }

    MailElt CreateMailElt()
    {
        GameObject obj = Instantiate(prefabMail) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollMail);
        tr.InitTransform();

        MailElt elt = obj.GetComponent<MailElt>();
        listMailelt.Add(elt);

        return elt;

    }



    public void SetLike(UserLikeInfo info)
    {
        LikeElt elt = CreateLikeElt();
        elt.SetInfo(info);
    }

    LikeElt CreateLikeElt()
    {
        GameObject obj = Instantiate(prefabLike) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollLike);
        tr.InitTransform();

        LikeElt elt = obj.GetComponent<LikeElt>();
        listLikeelt.Add(elt);

        return elt;

    }

    public void SetPresent(UserPresentInfo info)
    {
        PresentElt elt = CreatePresentElt();
        elt.SetInfo(info);
    }

    PresentElt CreatePresentElt()
    {
        GameObject obj = Instantiate(prefabPresent) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPresent);
        tr.InitTransform();

        PresentElt elt = obj.GetComponent<PresentElt>();
        listPresentelt.Add(elt);

        return elt;

    }

    public void SetPlz(UserPlzInfo info)
    {
        PlzElt elt = CreatePlzElt();
        elt.SetInfo(info);
    }

    PlzElt CreatePlzElt()
    {
        GameObject obj = Instantiate(prefabPlz) as GameObject;
        obj.SetActive(true);

        Transform tr = obj.transform;
        tr.SetParent(rtScrollPlz);
        tr.InitTransform();

        PlzElt elt = obj.GetComponent<PlzElt>();
        listPlzelt.Add(elt);

        return elt;

    }




    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }

}
