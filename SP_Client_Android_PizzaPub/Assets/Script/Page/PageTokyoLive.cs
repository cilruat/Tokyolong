﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTokyoLive : SingletonMonobehaviour<PageTokyoLive> {
   
	public Text txtDiscount;
	public Text txtDesc;
    public Text txtQuesiton;
    public Text[] txtChoice; 
	public Image[] imgCheck;
	public GameObject[] objChoice;
	public GameObject[] objSelect;

    public CountDown countDown;
    public Image imgTime;
    public GameObject objTime;
	public GameObject objSendServer;
	public GameObject objBoard;

	public CountDown countDownPrev;
	public Image imgReady;
	public GameObject objDecoText;

    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

    const int LIMIT_TIME = 15;
	const int LIMIT_TIME_SELECT = 2;
	const int LIMIT_PREV_TIME = 60;

	string first_desc = "도쿄라이브 대손님퀴즈쇼~\n\n총 2문제가 출제가 되는데\n모두 맞추셔야\n할인이 적용됩니다.\n\n한문제라도 틀리면 즉시 종료!!\n\n그럼 첫번째 문제 나갑니다\n\n고고고!!!";
	string[] desc = {		
		"\n\n역시 대단하시네요~~\n\n바로 이어서\n두번째 문제 나갑니다\n\n고고고!!!",
		"\n\n\n정말 잘 푸셨어요~\n\n할인 적용됩니다^^*",
		"\n\n\n아쉽지만 할인은\n다음기회에\nㅠㅠ\n\n안녕~~" };
	
    string[] question1 = { "", "", "", "" };
    string[] question2 = { "", "", "", "" };

	bool showTime = false;
	bool nextQuestion = false;
    	
	int curStage = 1;
    int answer1 = 0;
    int answer2 = 0;
	int selectAnswer = 0;
	    
	public void PrevSet(bool cheat = false)
	{
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		UIManager.Instance.PlayMusic (UIManager.Instance.clipTokyoLive, 3f);

		nextQuestion = false;

		curStage = 1;
		selectAnswer = 0;

		_Init ();

		#if UNITY_EDITOR
		if (Info.GameDiscountWon == -1)
			Info.GameDiscountWon = (short)Random.Range(0, 5);
		#endif

		_CollectQuestion (Info.GameDiscountWon);
		_RandQuestion(ref question1, ref answer1);
		_RandQuestion(ref question2, ref answer2);

		string descDiscount = "";
		switch ((EDiscount)Info.GameDiscountWon) {
		case EDiscount.e500won:		descDiscount = "500￦";		break;
		case EDiscount.e1000won:	descDiscount = "1000￦";	break;
		case EDiscount.e2000won:	descDiscount = "2000￦";	break;
		case EDiscount.e5000won:	descDiscount = "5000￦";	break;
		case EDiscount.eAll:		descDiscount = "전액할인";	break;
		}

		txtDiscount.text = descDiscount;
		countDownPrev.Set (5, () => OnStart ());
	}

	void _CollectQuestion(int diffculty)
	{
		#if UNITY_ANDROID
		string path = Application.streamingAssetsPath + "/TokyoLive_QuestionBook.csv";
		#else
		string path = Application.dataPath;
		int lastIdx = path.LastIndexOf(@"/");
		path = path.Substring(0, lastIdx) + @"\Info\TokyoLive_QuestionBook.csv";
		#endif

		List<Dictionary<string, object>> q = new List<Dictionary<string, object>>();
		q = CSVReader.Read(path);

		diffculty += 1;
		for (int i = 0; i < q.Count; i++) {
			int diff = int.Parse (q [i] ["Difficulty"].ToString ());
			if (diff != diffculty)
				continue;

			data.Add (q [i]);
		}

		Debug.Log ("Data Count: " + data.Count);
	}

	void Update()
	{		
		if (showTime == false)
			return;

		if (imgTime.fillAmount == 0) {
			if (imgTime.gameObject.activeSelf)
				imgTime.gameObject.SetActive (false);
			return;
		}

		float limit_time = selectAnswer > 0 ? LIMIT_TIME_SELECT : LIMIT_TIME;

		float elapsed = countDown.GetElapsed();
		float fill = (limit_time - elapsed) / limit_time;
		imgTime.fillAmount = fill;
	}

	void _Init()
	{		
		showTime = false;
		txtQuesiton.text = "";
		selectAnswer = 0;
		data.Clear ();

		txtDesc.text = "";
		txtDiscount.text = "";
		Color descColor = txtDesc.color;
		txtDesc.color = new Color (descColor.r, descColor.g, descColor.b, 1f);
		txtDesc.gameObject.SetActive (true);

		for (int i = 0; i < 3; i++)
		{
			txtChoice [i].text = "";
			objSelect [i].SetActive (false);
			objChoice [i].SetActive (false);
			imgCheck [i].fillAmount = 0f;
		}

		objSendServer.SetActive (false);
		objTime.SetActive (false);
	}		

	public void OnStart()
	{
		UIManager.Instance.MuteMusic ();
		StartCoroutine (_Start ());
	}

    IEnumerator _Start()
    {
		objDecoText.SetActive (false);

		while (true) {
			imgReady.fillAmount -= Time.deltaTime * 2f;
			if (imgReady.fillAmount <= 0f)
				break;

			yield return null;
		}

		txtDiscount.text = "";

        yield return new WaitForSeconds(.5f);
        UITweenAlpha.Start(objBoard, 0f, 1f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));

        yield return new WaitForSeconds(.5f);

		yield return StartCoroutine (_ShowPrevDesc (first_desc));
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine (_ShowQuestion ());

		while (nextQuestion == false)
			yield return null;			

		yield return StartCoroutine (_ShowPrevDesc (desc[0]));
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine (_ShowQuestion ());
   }

	IEnumerator _ShowPrevDesc(string desc)
	{
		string str = "";
		char[] ch = desc.ToCharArray();
		for (int i = 0; i < ch.Length; i++)
		{
			str += ch[i].ToString();
			txtDesc.text = str;
			yield return new WaitForSeconds(.015f);
		}

		yield return new WaitForSeconds(2.5f);
		UITweenAlpha.Start (txtDesc.gameObject, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
	}

	IEnumerator _ShowQuestion()
	{
		string[] question = curStage == 1 ? question1 : question2;
		
		string str = "";
		char[] ch = question[0].ToCharArray();
		for (int i = 0; i < ch.Length; i++)
		{
			str += ch[i].ToString();
			txtQuesiton.text = str;
			yield return new WaitForSeconds(.015f);
		}

		txtQuesiton.text = question[0];

		for (int i = 0; i < objChoice.Length; i++)
		{
			txtChoice[i].text = question[i + 1];
			float delay = i * .2f;
			UITweenAlpha.Start(objChoice[i], 0f, 1f, TWParam.New(.8f, delay).Curve(TWCurve.CurveLevel2));
		}

		objTime.SetActive(true);
		//UITweenScale.Start(objTime, 1f, 1.1f, TWParam.New(.5f, .7f).Curve(TWCurve.CurveLevel2).Loop(TWLoop.PingPong));
		countDown.Set (LIMIT_TIME, () => _ShowAnswer ());

		showTime = true;
	}		   

    void _RandQuestion(ref string[] question, ref int answer)
    {		
        int rand = Random.Range(0, data.Count);
        if (rand >= data.Count)
            rand -= (rand - data.Count + 1);

		question [0] = data [rand] ["Question"].ToString ();
		question [1] = data [rand] ["Choice1"].ToString ();
		question [2] = data [rand] ["Choice2"].ToString ();
		question [3] = data [rand] ["Choice3"].ToString ();
		answer = int.Parse (data [rand] ["Answer"].ToString ());
		        
        data.RemoveAt(rand);
    }

	void _ShowAnswer()
	{
		showTime = false;
		objTime.SetActive(false);

		StartCoroutine (_ShowAnswerAni ());
	}

	IEnumerator _ShowAnswerAni()
	{
		int answer = curStage == 1 ? answer1 : answer2;
		int idx = answer - 1;
		Image check = imgCheck [idx];

		while (check.fillAmount < 1) {
			check.fillAmount += Time.fixedDeltaTime * 2f;
			yield return null;
		}

		yield return new WaitForSeconds (.75f);

		bool right = answer == selectAnswer;

		_Init ();
		if (curStage == 1 && right)
			_NextQuestion ();
		else
			StartCoroutine (_EndGame (right));
	}

	void _NextQuestion ()
	{		
		nextQuestion = true;
		curStage += 1;
	}

	IEnumerator _EndGame(bool right)
	{		
		if (curStage == 2 && right) {
			yield return StartCoroutine (_ShowPrevDesc (desc [1]));
			yield return new WaitForSeconds (.5f);
			objSendServer.SetActive (true);

			yield return new WaitForSeconds (1f);

			if (Info.TableNum == 0)
				OnClose ();
			else {
				OnClose ();
				Info.ShowResult ();
			}
		} else {
			Info.SURPRISE_STEP = -1;
			yield return StartCoroutine (_ShowPrevDesc (desc [2]));
			OnClose ();
		}

		yield break;
	}

	public void OnClose()
	{
		StartCoroutine (_OnClose ());
	}

	IEnumerator _OnClose()
	{
		UITweenAlpha.Start (gameObject, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		yield return new WaitForSeconds (.8f);

		imgReady.fillAmount = 1f;
		objDecoText.SetActive (true);
		UIManager.Instance.Hide (eUI.eTokyoLive);
	}

	public void OnSelect(int answer)
	{
		if (showTime == false)
			return;

		if (selectAnswer == 0)
			countDown.Set (LIMIT_TIME_SELECT, () => _ShowAnswer ());

		if (selectAnswer > 0)
			objSelect [selectAnswer - 1].SetActive (false);

		objSelect [answer - 1].SetActive (true);
		selectAnswer = answer;
	}

	public void OnCloseGame()
	{
		UIManager.Instance.MuteMusic ();

		_Init ();
		OnClose ();
	}
}