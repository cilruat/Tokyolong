using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageFindDiffPicture : SingletonMonobehaviour<PageFindDiffPicture> {

	public GameObject objStart;
	public Text txtCount;
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

	public Transform trFive;

	Image imgLeft;
	Image imgRight;
	RectTransform rtImgTime;

	bool isStart = false;
	bool tapToStart = false;
	int finishLimitTime = 0;

	int finish = 0;
	float timeSize = 0f;
	List<GameObject> listObjL = new List<GameObject> ();
	List<GameObject> listObjR = new List<GameObject> ();

	void Awake()
	{
		if (Info.GameDiscountWon == -1)
			Info.GameDiscountWon = 0;

		rtImgTime = imgTime.rectTransform;
		timeSize = rtImgTime.rect.width;

		Transform tr = null;
		switch (Info.GameDiscountWon) {
		case (short)EDiscount.e1000won:		tr = trFive;	finish = 5;		break;
		}

		int rand = Random.Range (0, tr.childCount);
		Transform findPicture = null;

		for (int i = 0; i < tr.childCount; i++) {
			if (i != rand)
				continue;

			findPicture = tr.GetChild (i);
			break;
		}

		_SetAnswer (true, findPicture);
		_SetAnswer (false, findPicture);

		finishLimitTime = Info.FIND_DIFF_PICTURE_LIMIT_TIME;
		txtTime.text = Info.practiceGame ? "∞" : finishLimitTime.ToString ();

		_RefreshCount (0);
		objQuit.SetActive (Info.practiceGame);

		StartCoroutine (_SlideShowimg ());
	}

	void Update()
	{
		if (isStart = false)
			return;

		if (Info.practiceGame)
			return;

		float elpased = limitTime.GetElapsed ();
		float fill = (finishLimitTime - elpased) / (float)finishLimitTime;

		rtImgTime.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, timeSize * fill);

		if (Input.GetMouseButtonDown (0))
			_CheckPos (Input.mousePosition);
	}

	void _CheckPos(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay (pos);
	}

	void _SetAnswer(bool isLeft, Transform parent)
	{
		string str = isLeft ? "Left" : "Right";
		Transform tr = parent.Find (str);

		if (isLeft) {
			imgLeft = tr.GetComponent<Image> ();
			imgLeft.fillAmount = 0;
		} else {
			imgRight = tr.GetComponent<Image> ();
			imgRight.fillAmount = 0;
		}

		for (int i = 0; i < finish; i++) {
			int num = i + 1;
			Transform child = tr.Find (num.ToString ());
			GameObject obj = child.Find ("Circle").gameObject;
			obj.SetActive (false);

			Button btn = child.GetComponent<Button> ();
			btn.onClick.RemoveAllListeners ();

			int idx = i;
			btn.onClick.AddListener (() => _OnFind (idx));

			if (isLeft)
				listObjL.Add (obj);
			else
				listObjR.Add (obj);
		}
	}

	IEnumerator _SlideShowimg()
	{
		while (tapToStart == false)
			yield return null;

		float fill = 0f;
		while (imgLeft.fillAmount < 1 || imgRight.fillAmount < 1) {
			fill += Time.deltaTime * .5f;
			imgLeft.fillAmount = fill;
			imgRight.fillAmount = fill;
			yield return null;
		}

		imgLeft.fillAmount = 1f;
		imgRight.fillAmount = 1f;
	}

	void _OnFind(int idx)
	{
		for (int i = 0; i < listObjL.Count; i++) {
			if (i != idx)
				continue;
			
			listObjL [i].SetActive (true);
			listObjR [i].SetActive (true);
			break;
		}

		List<GameObject> listFind = listObjL.FindAll (x => x.activeSelf);
		_RefreshCount (listFind.Count);

		if (finish == listFind.Count)
			StartCoroutine (_SuccessEndGame ());
	}

	void _RefreshCount(int count)
	{
		txtCount.text = "<color=#ffaa00>" + count.ToString () + "</color>/" + finish.ToString ();
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

		CanvasGroup cv = objHide.GetComponent<CanvasGroup> ();
		cv.blocksRaycasts = false;
	}

	IEnumerator _SuccessEndGame ()
	{
		// show sendserver obj
		limitTime.Stop ();

		ShiningGraphic.Start (imgLeft);
		ShiningGraphic.Start (imgRight);
		yield return new WaitForSeconds (.5f);

		objVictory.SetActive (true);
		yield return new WaitForSeconds (4f);

		UITweenAlpha.Start (objVictory, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objSendServer, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

		yield return new WaitForSeconds (1f);

		if (Info.practiceGame)
			ReturnPractiveGame ();
		else {
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
