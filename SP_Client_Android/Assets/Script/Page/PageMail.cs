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



    UITween tweenUrgency = null;

	List<TableElt> listTable = new List<TableElt>();


	void Awake()
	{
		//일단 테이블 정보만
		SetData (Info.adminTablePacking);
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

	//이거없는데 안쓰는거네ㅋㅋㅋ 이런식으로 할수있다 참고할것
	public void SendMSG(byte tableNo)
	{
		objMsgWrite.SetActive(true);
		MailMsgWrite.Instance.SetInfo(tableNo);
	}

    public void SendLike(byte tableNo)
    {
        objLike.SetActive(true);
        MailLike.Instance.SetInfo(tableNo);
    }



    // AdminTableMenu 와 같이 스크립트 생성해서 함ㅜ 만ㅡㄹ고 여서 REQ 보내기 작ㅓㅂ등을 하

    public void ReturnHome()
    {
        SceneChanger.LoadScene("Main", objBoard);

    }

}
