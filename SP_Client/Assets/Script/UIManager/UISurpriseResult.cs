using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISurpriseResult : MonoBehaviour {

	public GameObject objMiddle;
	public UIDiscountAnimation uiDiscount;
	public List<GameObject> listComplete = new List<GameObject> ();

	void _Init()
	{
		objMiddle.SetActive (false);
		uiDiscount.gameObject.SetActive (false);

		for (int i = 0; i < listComplete.Count; i++)
			listComplete [i].SetActive (false);
	}

	public void Show()
	{
		UIManager.Instance.PlayMusic (UIManager.Instance.clipMagnificent);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));

		for (int i = 0; i < listComplete.Count; i++) {
			if (i < Info.SURPRISE_STEP)
				listComplete [i].SetActive (true);
			else if (i == Info.SURPRISE_STEP) {
				UITweenScale.Start (listComplete [i], 1.8f, 1f, TWParam.New (.5f, 1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
				UITweenAlpha.Start (listComplete [i], 0f, 1f, TWParam.New (.5f, 1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
			}
		}

		StartCoroutine (_Show ());
	}

	IEnumerator _Show()
	{
		yield return new WaitForSeconds (1.5f);

		if (Info.SURPRISE_STEP < (short)EDiscount.eAll)
			UITweenAlpha.Start (objMiddle, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		else
			UITweenAlpha.Start (uiDiscount.gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
	}

	public void OnStart()
	{
		++Info.SURPRISE_STEP;
		Info.GameDiscountWon = Info.SURPRISE_STEP;

		OnClose ();
		StartCoroutine (_OnStart ());
	}

	IEnumerator _OnStart()
	{
		yield return new WaitForSeconds (.8f);

		int randGame = Random.Range (0, Info.TotalGameCountWithTokyoLive ());
		if (randGame == Info.TotalGameCountWithTokyoLive () - 1) {
			GameObject obj = UIManager.Instance.Show (eUI.eTokyoLive);
			PageTokyoLive ui = obj.GetComponent<PageTokyoLive> ();
			ui.PrevSet ();
		} else
			Info.PlayGame (randGame, gameObject);
	}

	public void OnClose()
	{
		if (Info.SURPRISE_STEP > -1)
			NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
		else
			FinishClose ();
	}

	public void FinishClose()
	{
		_Init ();
		Info.SURPRISE_STEP = -1;

		StartCoroutine (_OnFinishClose ());
	}

	IEnumerator _OnFinishClose()
	{
		UIManager.Instance.MuteMusic ();
		UITweenAlpha.Start (gameObject, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		yield return new WaitForSeconds (1f);

		Info.AfterDiscountBehavior ();
		UIManager.Instance.Hide (eUI.eSurpriseResult);
	}		
}
