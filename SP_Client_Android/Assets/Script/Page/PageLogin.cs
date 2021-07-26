﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PageLogin : PageBase {

	public Text txtSuccess;
	public GameObject objLoginBox;
    public CanvasGroup[] cgBoards;

	public InputField inputTable;
	public InputField inputIP;
	public InputField inputPORT;

    string IP = "";
    string PORT = "";

	bool successConnect = false;
	string tableNo = "";
    byte howMany = 0;
    ECustomerType eType = ECustomerType.MAN;

    //Typing Reapeat Effect
    public Text tx, tx2;
    public string m_text, m_text2 = "";
    public float TimeToWait, TimeToWait2;
    public float stringInterval, stringInterval2;



    protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();
		base.acFinal = _EnterCustomer;
		base.acFinalIdx = 2;


#if UNITY_ANDROID
        tableNo = PlayerPrefs.GetString ("set_table_no");
		IP = PlayerPrefs.GetString ("set_IP");
		PORT = PlayerPrefs.GetString ("set_PORT");
		if (string.IsNullOrEmpty (tableNo) || string.IsNullOrEmpty (IP) || string.IsNullOrEmpty (PORT))
			inputTable.gameObject.SetActive (true);
		else
		#endif
			_DataLoad ();
	}

	void _DataLoad()
	{
		#if UNITY_ANDROID
		inputTable.gameObject.SetActive (false);
		inputIP.gameObject.SetActive (false);
		inputPORT.gameObject.SetActive (false);

		_ShowLoginBox ();
		#else
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
		#endif

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
			
			if (Time.timeSinceLevelLoad > timeToStart + 1.5f) {
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
            StartCoroutine(_PreventOverLapLoginBtn()); // 로그인버튼 누르면 코루틴 실행
            return;
		}
	}

	public void SuccessConnect()
	{
		successConnect = true;
		string desc = "도쿄마츠리에 오신것을 환영합니다\n" + "테이블 넘버: " + tableNo;
		SystemMessage.Instance.Add (desc);

		StartCoroutine (_SendLoginREQ ());

        StartCoroutine(FirstTyping());
        StartCoroutine(SecondTyping());

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

    //코루틴 추가 == 한번에 여러번 터치하는것을 방지

    IEnumerator _PreventOverLapLoginBtn()
    {
        objLoginBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        objLoginBox.gameObject.SetActive(true);

    }



    public void SuccessLogin()
	{
		howMany = 1;
		eType = (ECustomerType)Random.Range (0, 3);

		OnNext ();

		if (waitRoutine != null)
			StopCoroutine(waitRoutine);

		waitRoutine = StartCoroutine(WaitShowNoticeAndHome());
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
        yield return new WaitForSeconds(5.5f);

        OnNext();
        waitRoutine = null;
    }

	public void OnSetTableNo()
	{		
		string table = inputTable.text;
		if (string.IsNullOrEmpty (table) == false) {
			PlayerPrefs.SetString ("set_table_no", table);
			tableNo = table;

			inputTable.gameObject.SetActive (false);
			inputIP.gameObject.SetActive (true);
		} else
			SystemMessage.Instance.Add ("테이블 번호를 입력하세요");
	}

	public void OnSetIP()
	{
		string ip = inputIP.text;
		if (string.IsNullOrEmpty (ip) == false) {
			PlayerPrefs.SetString ("set_IP", ip);
			IP = ip;
			inputIP.gameObject.SetActive (false);
			inputPORT.gameObject.SetActive (true);
		} else
			SystemMessage.Instance.Add ("IP를 입력하세요");
	}

	public void OnSetPORT()
	{
		string port = inputPORT.text;
		if (string.IsNullOrEmpty (port) == false) {
			PlayerPrefs.SetString ("set_PORT", port);
			PORT = port;
			_DataLoad ();
		} else
			SystemMessage.Instance.Add ("PORT를 입력하세요");
	}

	public void ClickedDelete()
    {
		PlayerPrefs.DeleteAll();
    }


    IEnumerator FirstTyping()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToWait);
            for (int i = 0; i < m_text.Length; i++)
            {
                tx.text = m_text.Substring(0, i);

                yield return new WaitForSeconds(stringInterval);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SecondTyping()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToWait2);
            for (int i = 0; i < m_text2.Length; i++)
            {
                tx2.text = m_text2.Substring(0, i);

                yield return new WaitForSeconds(stringInterval2);

            }
            yield return new WaitForSeconds(5f);
        }

    }

}
