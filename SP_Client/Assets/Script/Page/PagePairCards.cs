using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagePairCards : MonoBehaviour {

	const int LIMIT_TIME = 20;
	const int NORMAL_MODE_CARD_COUNT = 18;
	const int HARD_MODE_CARD_COUNT = 24;

	public Text txtCountDown;
	public CountDown countDown;
	public Image imgTime;
	public GridLayoutGroup grid;
	public GameObject objCard;
	public GameObject objBoard;
	public GameObject objSendServer;
	public GameObject objHide;
	public GameObject objBtnStart;
	public GameObject objTxtReady;
	public GameObject objTxtGo;
	public GameObject[] objTimeOut;
	public List<Texture> listCards = new List<Texture> ();

	bool start = false;
	bool end = false;
	List<CardElt> listElt = new List<CardElt> ();

	void Awake()
	{
		txtCountDown.text = LIMIT_TIME.ToString ();
		_SetCards ();
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (.5f);
		UITweenAlpha.Start(objBoard, 0f, 1f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			listElt [0].Rolling ();
		}

		if (start == false || end)
			return;
		
		float elapsed = countDown.GetElapsed ();
		float fill = (LIMIT_TIME - elapsed) / (float)LIMIT_TIME;
		imgTime.fillAmount = fill;
	}

	void _SetCards()
	{
		int mode = Random.Range (0, 2);
		int cnt = mode == 0 ? NORMAL_MODE_CARD_COUNT : HARD_MODE_CARD_COUNT;

		int prev_pairNum = 0;
		int pairNum = 0;
		Texture tex = null;
		for (int i = 0; i < cnt; i++) {
			GameObject obj = Instantiate (objCard) as GameObject;
			obj.name = "Card" + i.ToString ();
			obj.SetActive (true);

			RectTransform rt = (RectTransform)obj.transform;
			rt.SetParent (grid.transform);
			rt.InitTransform ();

			if (i % 2 == 0) {
				++pairNum;
				tex = _GetRandCardImg ();
			}

			CardElt elt = rt.GetComponent<CardElt> ();
			elt.SetIdx (i, pairNum);
			elt.SetImg (tex);

			prev_pairNum = pairNum;

			listElt.Add (elt);
		}


		_MixCards ();
	}

	Texture _GetRandCardImg()
	{
		System.Random random = new System.Random ();
		int rand = random.Next (listCards.Count);

		Texture tex = listCards [rand];
		listCards.RemoveAt (rand);

		return tex;
	}

	void _MixCards()
	{
		System.Random random = new System.Random ();
		for (int i = 0; i < listElt.Count; i++) {
			int rand = random.Next (listElt.Count);
			listElt[i].transform.SetSiblingIndex (rand);
		}
	}		

	public void OnStart()
	{
		StartCoroutine (_ReadyGo ());
	}

	IEnumerator _ReadyGo()
	{
		UITweenAlpha.Start(objBtnStart, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));

		float sec = .5f / (float)(listElt.Count);
		for (int i = 0; i < listElt.Count; i++) {
			listElt [i].objImg.SetActive (true);
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (3f);

		UITweenAlpha.Start(objTxtReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		for (int i = 0; i < listElt.Count; i++) {
			listElt [i].objImg.SetActive (false);
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (1f);

		UITweenAlpha.Start(objTxtReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.25f);

		UITweenAlpha.Start(objTxtGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (1f);

		UITweenAlpha.Start(objTxtGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.3f);

		objHide.SetActive (false);

		start = true;
		countDown.Set (LIMIT_TIME, () => StartCoroutine (_FailEndGame ()));
	}

	List<KeyValuePair<int,int>> listChecks = new List<KeyValuePair<int, int>>();
	public void CheckPair(int idx, int pairNum)
	{
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].idx == idx && listElt [i].objImg.activeSelf)
				return;
		}
			
		for (int i = 0; i < listChecks.Count; i++) {
			if (listChecks [i].Key == idx)
				return;
		}

		listChecks.Add (new KeyValuePair<int, int> (idx, pairNum));

		for (int i = 0; i < listChecks.Count / 2; i++) {
			int check = i * 2;
			KeyValuePair<int,int> first = listChecks [check];
			KeyValuePair<int,int> second = listChecks [check + 1];

			if (first.Value == second.Value) {
				listElt [first.Key].Find ();
				listElt [second.Key].Find ();

				_CheckAllPair ();
			}
			else
				StartCoroutine (_DelayedHide (first.Key, second.Key));

			_RemoveListChecks (first.Key, second.Key);
		}
	}

	void _CheckAllPair()
	{
		bool isFinish = true;
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].isFind)
				continue;

			isFinish = false;
			break;
		}

		if (isFinish) {
			end = true;
			countDown.Stop ();
			StartCoroutine (_SuccessEndGame ());
		}
	}

	IEnumerator _DelayedHide(int firstIdx, int secondIdx)
	{
		yield return new WaitForSeconds (1f);

		listElt [firstIdx].Hide ();
		listElt [secondIdx].Hide ();
	}

	void _RemoveListChecks(int firstIdx, int secondIdx)
	{
		for (int i = 0; i < listChecks.Count; i++) {
			if (listChecks [i].Key == firstIdx ||
				listChecks [i].Key == secondIdx) {
				listChecks.RemoveAt (i);
				--i;
			}
		}
	}

	IEnumerator _SuccessEndGame()
	{
		float sec = 1f / (float)(listElt.Count);
		for (int i = 0; i < listElt.Count; i++) {
			ShiningGraphic.Start (listElt [i].img);
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (.25f);

		objSendServer.SetActive (true);
		yield return new WaitForSeconds (1f);

		//NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
		Debug.Log("Game_Discount_REQ");
	}

	IEnumerator _FailEndGame()
	{
		UITweenAlpha.Start (objTimeOut [0], 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (1.5f);

		UITweenAlpha.Start (objTimeOut [1], 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.25f);

		UITweenAlpha.Start (objTimeOut [2], 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (1.5f);
		ReturnHome ();
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Game", objBoard);
	}
}
