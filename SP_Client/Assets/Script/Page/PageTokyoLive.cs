using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTokyoLive : SingletonMonobehaviour<PageTokyoLive> {
   
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

    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

    const int LIMIT_TIME = 15;
	const int LIMIT_TIME_SELECT = 2;

	string[] desc = {
		"도쿄라이브 대손님퀴드쇼~\n\n총 2문가 출제가 되는데\n모두 맞추셔야\n할인이 적용됩니다.\n\n한문제라도 틀리면 즉시 종료!!\n\n자 그럼 첫번째 문제 나갑니다\n\n고고고!!!", 
		"\n\n역시 대단하시네요~~\n\n바로 이어서\n두번째 문제 나갑니다\n\n고고고!!!",
		"\n\n\n정말 잘 푸셨어요~\n\n할인 적용됩니다^^*",
		"\n\n\n아쉽지만 할인은 다음기회에\nㅠㅠ\n\n안녕~~" };
	
    string[] question1 = { "", "", "", "" };
    string[] question2 = { "", "", "", "" };

	bool showTime = false;
	bool nextQuestion = false;
    	
	int curStage = 1;
    int answer1 = 0;
    int answer2 = 0;
	int selectAnswer = 0;

    void Awake()
    {
		Application.runInBackground = true;

        string path = Application.dataPath;
        int lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\Info\TokyoLive_QuestionBook.csv";

        data = CSVReader.Read(path);

        _RandQuestion(ref question1, ref answer1);
        _RandQuestion(ref question2, ref answer2);

		_Init ();              
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
		txtQuesiton.text = "";
		selectAnswer = 0;

		txtDesc.text = "";
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
	}

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        UITweenAlpha.Start(objBoard, 0f, 1f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));

        yield return new WaitForSeconds(.5f);

		yield return StartCoroutine (_ShowPrevDesc (desc[0]));
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine (_ShowQuestion ());

		while (nextQuestion == false)
			yield return null;			

		yield return StartCoroutine (_ShowPrevDesc (desc[1]));
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
		UITweenScale.Start(objTime, 1f, 1.1f, TWParam.New(.5f, .7f).Curve(TWCurve.CurveLevel2).Loop(TWLoop.PingPong));
		countDown.Set (LIMIT_TIME, () => _ShowAnswer ());

		showTime = true;
	}		   

    void _RandQuestion(ref string[] question, ref int answer)
    {		
        int rand = Random.Range(0, data.Count);
        if (rand >= data.Count)
            rand -= (rand - data.Count + 1);

        Dictionary<string, object> randQuestion = data[rand];

        int idx = 0;
        foreach (object obj in randQuestion.Values)
        {
            if (idx == randQuestion.Values.Count - 1)
            {
                string s = obj.ToString();
                answer = int.Parse(s);
            }
            else
                question[idx] = obj.ToString();
            ++idx;
        }

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
			yield return StartCoroutine (_ShowPrevDesc (desc [2]));
			yield return new WaitForSeconds (.5f);
			objSendServer.SetActive (true);

			yield return new WaitForSeconds (1f);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
		} else {
			yield return StartCoroutine (_ShowPrevDesc (desc [3]));
			ReturnHome ();
		}

		yield break;
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objBoard);
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
}