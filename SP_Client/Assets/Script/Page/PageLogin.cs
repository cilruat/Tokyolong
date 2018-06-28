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

		if (PlayerPrefs.HasKey ("set_table") == false)
			inputTable.gameObject.SetActive (true);
		else
			_Connect ();
	}

	void _Connect()
	{
		tableNo = PlayerPrefs.GetString ("set_table");
		txtSuccess.text = "서버에 접속중입니다";

		NetworkManager.Instance.connect ();
	}

    void _EnterCustomer()
    {
        NetworkManager.Instance.EnterCostomer_REQ(howMany, (byte)eType);
    }		

	public void SuccessConnect()
	{
		txtSuccess.text = "정상적으로 서버에 접속하였습니다";

		UITweenAlpha.Start (txtSuccess.gameObject, 0f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objLoginBox, 1f, TWParam.New (1f, 1.5f).Curve (TWCurve.CurveLevel2));
	}

	public void OnInputTable()
	{		
		PlayerPrefs.SetString ("set_table", inputTable.text);
		inputTable.gameObject.SetActive (false);

		_Connect ();
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
