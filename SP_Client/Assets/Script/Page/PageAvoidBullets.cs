using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageAvoidBullets : MonoBehaviour {

	const int LIMIT_TIME = 20;
	const int MAX_BULLET = 50;

	public float TERM_BULLET = 1f;
	public float SPEED_BULLET = 5f;
	public float SPEED_FLIGHT = 5f;

	public Text txtCountDown;
	public CountDown countDown;
	public Image imgTime;
	public GameObject objBoard;
	public GameObject objSendServer;
	public GameObject objHide;
	public GameObject objBtnStart;
	public GameObject objTxtReady;
	public GameObject objTxtGo;
	public GameObject[] objTimeOut;
	public RectTransform rtBox;
	public RectTransform rtFlight;
	public GameObject objBullet;

	public int[] lvIncTime;
	public int[] bulletCnt;
	int level = 0;

	bool start = false;
	bool end = false;
	bool startBtnClick = false;
	List<Bullet> listBullet = new List<Bullet>();

	void Awake()
	{
		txtCountDown.text = LIMIT_TIME.ToString ();

		for (int i = 0; i < MAX_BULLET; i++) {
			GameObject obj = Instantiate (objBullet) as GameObject;

			RectTransform rt = (RectTransform)obj.transform;
			rt.SetParent (rtBox);
			rt.InitTransform ();

			Bullet bullet = obj.GetComponent<Bullet> ();
			listBullet.Add (bullet);
		}
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (.5f);
		UITweenAlpha.Start(objBoard, 0f, 1f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));
	}

	void Update()
	{
		if (start == false || end)
			return;

		float elapsed = countDown.GetElapsed ();
		float fill = (LIMIT_TIME - elapsed) / (float)LIMIT_TIME;
		imgTime.fillAmount = fill;

		if (elapsed > lvIncTime [level] && (level < lvIncTime.Length - 1))
			level++;
	}

	IEnumerator _CreateBullet()
	{
		while (end == false) {
			yield return new WaitForSeconds (TERM_BULLET);
			for (int i = 0; i < bulletCnt [level]; i++)
				if (end == false)
					_SetBullet ();
		}
	}

	void _SetBullet()
	{
		Vector2 pos = GetRanmdomPos ();
		Vector2 dir = rtFlight.anchoredPosition - pos;

		Bullet unUseBullet = listBullet.Find (b => b.isUse == false);
		if (unUseBullet) {
			unUseBullet.isUse = true;
			unUseBullet.SetDir (dir.normalized);
			unUseBullet.SetPos (pos);
		}
	}

	Vector2 GetRanmdomPos()
	{
		Vector2 pos = Vector2.zero;
		int rand = Random.Range (0, 3);
		switch (rand) {
		case 0:		// 좌
			pos.x = rtBox.rect.xMin + 5f;
			pos.y = Random.Range (0, rtBox.rect.yMax);
			break;
		case 1:		// 우
			pos.x = rtBox.rect.xMax - 5f;
			pos.y = Random.Range (0, rtBox.rect.yMax);
			break;
		case 2:		// 탑
			pos.x = Random.Range(rtBox.rect.xMin, rtBox.rect.xMax);
			pos.y = rtBox.rect.yMax - 5f;
			break;
		}

		return pos;
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
		UITweenAlpha.Start (objBtnStart, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.6f);

		UITweenAlpha.Start(objTxtReady, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (1.2f);

		UITweenAlpha.Start(objTxtReady, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.25f);

		UITweenAlpha.Start(objTxtGo, 0f, 1f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (1f);

		UITweenAlpha.Start(objTxtGo, 1f, 0f, TWParam.New(.5f).Curve(TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.3f);

		objHide.SetActive (false);

		start = true;
		countDown.Set (LIMIT_TIME, () => StartCoroutine (_SuccessEndGame ()));

		StartCoroutine (_CreateBullet ());
	}

	IEnumerator _SuccessEndGame()
	{
		end = true;
		for (int i = 0; i < listBullet.Count; i++)
			Destroy (listBullet [i].gameObject);

		_CreateSuccessEff ();

		yield return new WaitForSeconds (.5f);

		UITweenPosY.Start (rtFlight.gameObject, rtFlight.anchoredPosition.y - 10f, TWParam.New (.5f).Curve (TWCurve.CurveLevel1));
		yield return new WaitForSeconds (.5f);

		UITweenPosY.Start (rtFlight.gameObject, 250f, TWParam.New (.5f).Curve (TWCurve.CurveLevel1));

		yield return new WaitForSeconds (.5f);

		objSendServer.SetActive (true);
		yield return new WaitForSeconds (1f);

		NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}

	void _CreateSuccessEff()
	{
	}

	public void FailEndGame()
	{
		end = true;
		countDown.Stop ();

		for (int i = 0; i < listBullet.Count; i++)
			Destroy (listBullet [i].gameObject);

		StartCoroutine (_FailEndGame ());
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

	public bool CheckInnerBox(Vector2 pos)
	{
		if (pos.x > rtBox.rect.xMin + 4f && pos.x < rtBox.rect.xMax - 4f &&
			pos.y > rtBox.rect.yMin + 4f && pos.y < rtBox.rect.yMax - 4f)
			return true;
		
		return false;
	}
}
