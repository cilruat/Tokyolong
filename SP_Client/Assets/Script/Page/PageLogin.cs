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

    string IP = "";
    string PORT = "";

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

        string path = Application.dataPath;
        int lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\Info\tableNo.txt";
		tableNo = System.IO.File.ReadAllText (path);

        path = Application.dataPath;
        lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\Info\ServerInfo.txt";
        string server = System.IO.File.ReadAllText(path);

        if (string.IsNullOrEmpty(tableNo) || string.IsNullOrEmpty(server))
            txtSuccess.text = "텍스트 파일을 읽어오지 못했습니다";
        else
        {
            string[] LINE_SPLIT_RE = { "\r\n", "\n\r", "\n", "\r" };
            string[] serverInfo = server.Split(LINE_SPLIT_RE, System.StringSplitOptions.RemoveEmptyEntries);
            IP = serverInfo[0];
            PORT = serverInfo[1];

            _Connect ();
        }
	}

	void _Connect()
	{
		if (NetworkManager.Instance.is_connected() == false) {
			txtSuccess.text = "서버에 접속중입니다";
            NetworkManager.Instance.connect (IP, PORT);
		} else
			_ShowLoginBox ();
	}

    void _EnterCustomer()
    {
        NetworkManager.Instance. EnterCostomer_REQ(howMany, (byte)eType);
    }

	public void SuccessConnect()
	{
		txtSuccess.text = "정상적으로 서버에 접속하였습니다\n" + "테이블 넘버: " + tableNo;
		_ShowLoginBox ();
	}	

	void _ShowLoginBox()
	{
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
