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

	public GameObject prefabTable;
	public GameObject objTableBoardCover;
	public GameObject objTableMenu;


	UITween tweenUrgency = null;

	List<TableElt> listTable = new List<TableElt>();


	void Awake()
	{
		//일단 테이블 정보만
		SetData (Info.mailTablePacking); 
	}

	void LoadTable()
	{
		//Instantiate() Destroy() 개념, 실행도중 게임오브젝트 생성, 파괴
		/*# AS, IS 연산자의 개념
		부모 클래스를 자식 클래스에 대입하는 경우가 발생하고, 그러한 행위를 도와주는 것이 AS, IS 연산자
		AS 연산자 : 형변환이 가능하면 형변환을 수행하고, 그렇지 않으면 null 값을 대입
		IS 연산자 : 형변환이 가능한 여부를 불린형으로 결과값을 반환한다*/
		
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
		Info.mailTablePacking = "";
	}


	//테이블 정보가 같으면 로그인 하고, 다르면 넘어가라
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

	//추가 로그아웃하면 테이블의 좋아요, 쪽지목록, 선물목록, 좋아요 목록 지워야할까
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

	//메세지와 좋아요등의 함수를 생성 처리할것

	public void SendMSG(int tableNo)
	{

	}



	// AdminTableMenu 와 같이 스크립트 생성해서 함ㅜ 만ㅡㄹ고 여서 REQ 보내기 작ㅓㅂ등을 하

}
