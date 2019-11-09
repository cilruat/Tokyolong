using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagePairCards : SingletonMonobehaviour<PagePairCards> {

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
	public GameObject objGameOver;
	public GameObject objQuit;
	public List<Texture> listCards = new List<Texture> ();

	public bool start = false;
	public bool end = false;
	bool startBtnClick = false;
	List<CardElt> listElt = new List<CardElt> ();

	void Awake()
	{
		txtCountDown.text = Info.practiceGame ? "∞" : Info.PAIR_CARD_LIMIT_TIME.ToString ();
		_SetCards ();

		objQuit.SetActive (Info.practiceGame);
	}		

	void Update()
	{
		if (start == false || end)
			return;

		if (Info.practiceGame)
			return;

		float elapsed = countDown.GetElapsed ();
		float fill = (Info.PAIR_CARD_LIMIT_TIME - elapsed) / (float)Info.PAIR_CARD_LIMIT_TIME;
		imgTime.fillAmount = fill;
	}

	void _SetCards()
	{		
		int cnt = Info.PAIR_CARD_MODE;

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
		if (startBtnClick)
			return;

		startBtnClick = true;
		StartCoroutine (_ReadyGo ());
	}

	IEnumerator _ReadyGo()
	{
		UITweenAlpha.Start(objBtnStart, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.7f);

		float sec = .5f / (float)(listElt.Count);
		for (int i = 0; i < listElt.Count; i++) {
			listElt [i].Rolling (true);
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (3f);

		UITweenAlpha.Start(objTxtReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		for (int i = 0; i < listElt.Count; i++) {
			listElt [i].Rolling (false);
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

		if (Info.practiceGame == false)
			countDown.Set (Info.PAIR_CARD_LIMIT_TIME, () => _FailEndGame ());
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
		float sec = 1f / (float)(grid.transform.childCount);
		for (int i = 0; i < grid.transform.childCount; i++) {
			Transform tr = grid.transform.GetChild (i);
			CardElt elt = tr.GetComponent<CardElt> ();

			ShiningGraphic.Start (elt.img);
			yield return new WaitForSeconds (sec);
		}

		yield return new WaitForSeconds (.25f);

		if (Info.practiceGame)
			ReturnPractiveGame ();
		else {
			objSendServer.SetActive (true);
			yield return new WaitForSeconds (1f);
			objSendServer.SetActive (true);

			if (Info.TableNum == 0)
				ReturnHome ();
			else
				Info.ShowResult ();
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
