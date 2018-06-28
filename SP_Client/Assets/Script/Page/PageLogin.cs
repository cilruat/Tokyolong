using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PageLogin : PageBase {

	public InputField inputTable;
	public Text txtSuccess;
	public GameObject objLoginBox;
    public CanvasGroup[] cgBoards;

	string tableNo = "";
    byte howMany = 0;
    ECustomerType eType = ECustomerType.MAN;

	protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();
		base.acFinal = _EnterCustomer;
		base.acFinalIdx = 4;

		Debug.Log (Application.dataPath);

		string path = Application.dataPath + @"\tableNo.txt";
		tableNo = System.IO.File.ReadAllText (path);

		if (string.IsNullOrEmpty(tableNo))
			txtSuccess.text = "텍스트 파일을 읽어오지 못했습니다";
		else
			_Connect ();
	}

	void _Connect()
	{		
		txtSuccess.text = "서버에 접속중입니다";
		NetworkManager.Instance.connect ();
	}

    void _EnterCustomer()
    {
        NetworkManager.Instance.EnterCostomer_REQ(howMany, (byte)eType);
    }		

	public void SuccessConnect()
	{
		txtSuccess.text = "정상적으로 서버에 접속하였습니다\n" + "테이블 넘버: " + tableNo;

		UITweenAlpha.Start (txtSuccess.gameObject, 0f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objLoginBox, 1f, TWParam.New (1f, 1.5f).Curve (TWCurve.CurveLevel2));
	}		

	public void OnLogin()
	{
		if (string.IsNullOrEmpty (tableNo) == true) {
			SystemMessage.Instance.Add ("테이블 넘버가 셋팅되지 않았습니다");
			return;
		}

		NetworkManager.Instance.Login_REQ (tableNo);
	}

    public void OnSelect(int number)
    {
        howMany = (byte)number;
        OnNext();
    }

    public void OnType(int type)
    {
        eType = (ECustomerType)type;
        OnNext();
    }		   
}
