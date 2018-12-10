using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTouchNumber : SingletonMonobehaviour<PageTouchNumber> {

	class Number
	{
		public int num = 0;
		public Text txt;
		public RawImage img;
		public Button btn;
		public GameObject obj;
		public RectTransform rt;
		public PageTouchNumber page;

		public Number(int num, Text txt, RawImage img, Button btn, GameObject obj, RectTransform rt, PageTouchNumber page)
		{
			this.txt = txt;
			this.img = img;
			this.num = num;
			this.btn = btn;
			this.obj = obj;
			this.rt = rt;
			this.page = page;
		}

		public void SetInfo()
		{
			txt.text = num.ToString ();

			Color[] col = {
				new Color (1f, .5f, 0),
				new Color (0, .625f, 1f),
				Color.green,
				Color.yellow
			};

			img.texture = Random.Range (0, 2) == 0 ? page.texNum1 : page.texNum2;
			img.color = col [Random.Range (0, 4)];

			btn.onClick.RemoveAllListeners ();
			btn.onClick.AddListener (() => _Touch ());
		}

		void _Touch()
		{
			if (page.isStart == false)
				return;

			if (page.CheckNum (num) == false) {
				page.StartCoroutine (_DiffClick ());
				return;
			}
			
			page.TouchNum (num);

			obj.SetActive (false);
			UIManager.Instance.PlaySound ();
		}

		IEnumerator _DiffClick()
		{
			Color backUp = img.color;
			UITween tween = UITweenColor.Start (obj, Color.white, Color.red, TWParam.New (.2f).Curve (TWCurve.CurveLevel2));

			while (tween.IsTweening ())
				yield return null;

			img.color = backUp;
		}
	}

	public GameObject objStart;
	public Text txtTime;
	public Image imgTime;
	public CountDown limitTime;
	public RawImage imgVictory;
	public GameObject objVictory;
	public GameObject objSendServer;
	public GameObject objGameOver;
	public GameObject objReady;
	public GameObject objGo;
	public GameObject objBoard;
	public GameObject objQuit;
	public GameObject objHide;
	public GameObject objPrefab;
	public Text txtNum;

	public Texture texNum1;
	public Texture texNum2;

	public List<RectTransform> listPos = new List<RectTransform>();

	bool tapToStart = false;
	bool isStart = false;
	int finishLimitTime = 0;

	int curNum = 0;
	int finishNum = 0;

	void Awake()
	{
		finishNum = Info.TOUCH_NUMBER_MAX_COUNT;
		finishLimitTime = Info.TOUCH_NUMBER_LIMIT_TIME;
		txtTime.text = Info.practiceGame ? "∞" : finishLimitTime.ToString ();

		objQuit.SetActive (Info.practiceGame);
	}		

	IEnumerator _MakeNumber()
	{
		for (int i = finishNum - 1; i >= 0; i--) {
			GameObject obj = Instantiate (objPrefab) as GameObject;
			int num = i + 1;
			obj.name = "Number" + num.ToString ();
			obj.SetActive (true);

			RectTransform rt = (RectTransform)obj.transform;

			int inputPos = _CheckInputPos (i + 1);
			int randPos = inputPos > -1 ? inputPos : Random.Range (0, listPos.Count);

			RectTransform rtParent = listPos [randPos];
			rt.SetParent (rtParent);
			rt.InitTransform ();

			Number numInfo = new Number (
				num, 
				rt.Find ("Num").GetComponent<Text> (), 
				rt.GetComponent<RawImage> (), 
				rt.GetComponent<Button> (), 
				obj, 
				rt, 
				this);
			
			numInfo.SetInfo ();

			float scale = Random.Range (.8f, 1.2f);
			UITweenScale.Start (obj, .2f, scale, TWParam.New (.6f).Curve (TWCurve.Spring).Speed (TWSpeed.SlowerFaster));

			float waitSec = 1.7f / (float)finishNum;
			yield return new WaitForSeconds (waitSec);
		}
	}

	int _CheckInputPos(int remain)
	{
		int emptyCnt = 0;
		int pos = 0;
		for (int i = 0; i < listPos.Count; i++) {
			if (listPos [i].childCount == 0) {
				pos = i;
				++emptyCnt;
			}
		}

		return remain == emptyCnt ? pos : -1;
	}

	void Update()
	{
		if (isStart == false)
			return;

		if (Info.practiceGame)
			return;

		float elapsed = limitTime.GetElapsed();
		float fill = (finishLimitTime - elapsed) / (float)finishLimitTime;
		imgTime.fillAmount = fill;
	}

	bool CheckNum(int num) { return curNum + 1 == num; }
	void TouchNum(int num)
	{
		curNum = num;
		txtNum.text = curNum.ToString ();

		if (finishNum == num)
			StartCoroutine (_SuccessEndGame ());
	}

	public void OnStart()
	{
		if (tapToStart == false)
			StartCoroutine (_Start ());
	}

	IEnumerator _Start()
	{
		tapToStart = true;

		UITweenAlpha.Start (objStart, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
		yield return new WaitForSeconds (.5f);

		UITweenAlpha.Start(objReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		StartCoroutine (_MakeNumber ());

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.25f);
		UITweenAlpha.Start(objGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);
		UITweenAlpha.Start(objGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		UITweenAlpha.Start(objHide, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		yield return new WaitForSeconds (.3f);

		isStart = true;

		if (Info.practiceGame == false)
			limitTime.Set (finishLimitTime, () => _FailEndGame ());
	}

	IEnumerator _SuccessEndGame ()
	{		
		limitTime.Stop ();
		ShiningGraphic.Start (imgVictory);

		if (Info.practiceGame) {
			yield return new WaitForSeconds (1f);
			ReturnPractiveGame ();
		}
		else {
			objVictory.SetActive (true);
			yield return new WaitForSeconds (4f);

			UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			yield return new WaitForSeconds (1f);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
		}
	}

	void _FailEndGame()
	{
		objGameOver.SetActive (true);
	}

	public void ReturnPractiveGame()
	{
		SceneChanger.LoadScene ("PracticeGame", objBoard);
	}

	public void ReturnHome()
	{		
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
