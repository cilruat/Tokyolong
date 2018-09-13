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

	bool successConnect = false;
	string tableNo = "";
    byte howMany = 0;
    ECustomerType eType = ECustomerType.MAN;

	protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();
		base.acFinal = _EnterCustomer;
		base.acFinalIdx = 4;

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

			_ShowLoginBox ();
        }

        MenuData.Load();
	}

	void _ShowLoginBox()
	{		
		UITweenAlpha.Start (objLoginBox, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2));

		if (Info.TableNum != 0)
			StartCoroutine (_AutoLogin ());
	}

    void _EnterCustomer()
    {
        NetworkManager.Instance. EnterCostomer_REQ(howMany, (byte)eType);
    }		

	IEnumerator _AutoLogin()
	{
		float timeToStart = Time.timeSinceLevelLoad;
		while (true) {			
			if (successConnect)
				break;
			
			if (Time.timeSinceLevelLoad > timeToStart + 3f) {
				timeToStart = Time.timeSinceLevelLoad;
				OnLogin ();
			}				

			yield return null;
		}
	}

	public void OnLogin()
	{
		if (NetworkManager.Instance.is_connected() == false) {			
			NetworkManager.Instance.connect (IP, PORT);
			return;
		}
	}

	public void SuccessConnect()
	{
		successConnect = true;
		string desc = "정상적으로 서버에 접속하였습니다\n" + "테이블 넘버: " + tableNo;
		SystemMessage.Instance.Add (desc);

		StartCoroutine (_SendLoginREQ ());
	}

	IEnumerator _SendLoginREQ()
	{
        Info.Init();
		yield return new WaitForSeconds (.5f);

		if (string.IsNullOrEmpty (tableNo) == true) {
			SystemMessage.Instance.Add ("테이블 넘버가 셋팅되지 않았습니다");
			yield break;
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

        if (waitRoutine != null)
            StopCoroutine(waitRoutine);

        waitRoutine = StartCoroutine(WaitShowNoticeAndHome());
    }

    Coroutine waitRoutine = null;
    IEnumerator WaitShowNoticeAndHome()
    {
        yield return new WaitForSeconds(3.5f);

        OnNext();
        waitRoutine = null;
    }
}
