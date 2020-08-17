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

    public GameObject SamDuk;

    public ShowMsg showMsg;

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


    //Urgency가 뭐지 ==호출
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
        listMailelt.Remove(elt);
        Destroy(elt.gameObject);
    }


    public void DeletePlzElt(PlzElt elt)
    {
        listPlzelt.Remove(elt);
        Destroy(elt.gameObject);
    }

    public void DeletePresentElt(PresentElt elt)
    {
        listPresentelt.Remove(elt);
        Destroy(elt.gameObject);
    }

    public void DeleteLikeElt(LikeElt elt)
    {
        listLikeelt.Remove(elt);
        Destroy(elt.gameObject);
    }


    public void OpenMap()
    {

        SamDuk.SetActive(true);

    }

    public void CloseMap()
    {

        SamDuk.SetActive(false);

    }


    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }

}
