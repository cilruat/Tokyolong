using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerEvent : MonoBehaviour {

	public eUI uiType;

	public CountDown countdown;
	public GameObject objSelect;
	public GameObject objLoading;

	void _Init()
	{
		objSelect.SetActive (true);
		objLoading.SetActive (false);
	}

	void _ShowLoading()
	{
		UITweenAlpha.Start (objSelect, 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		UITweenAlpha.Start (objLoading, 0f, 1f, TWParam.New (.5f, .5f).Curve (TWCurve.CurveLevel2));

		countdown.Stop ();
		countdown.Set (5, () => StartCoroutine (_StartEvent ()));
	}

	IEnumerator _StartEvent()
	{
		Info.GameDiscountWon = 0;
		countdown.Stop ();
		UIManager.Instance.StopMusic ();

		yield return null;

		switch (uiType) {
		case eUI.eOwnerGame:
			int randGame = Random.Range (0, Info.TotalGameCount ());
			Info.PlayGame (randGame, gameObject);
			break;
		case eUI.eOwnerQuiz:
			GameObject obj = UIManager.Instance.Show (eUI.eTokyoQuiz);
			TokyoQuiz quiz = obj.GetComponent<TokyoQuiz> ();
			quiz.OnStart ();
			break;
		case eUI.eOwnerTrick:
			SceneChanger.LoadScene ("Trickery", gameObject);
			break;
		}

		yield return null;
		UIManager.Instance.Hide (uiType);
	}

	public void Show()
	{
		_Init ();

		switch (uiType) {
		case eUI.eOwnerGame:	UIManager.Instance.PlayMusic (UIManager.Instance.clipOwnerGame, 3f);	break;
		case eUI.eOwnerQuiz:	UIManager.Instance.PlayMusic (UIManager.Instance.clipOwnerQuiz, 3f);	break;
		case eUI.eOwnerTrick:	UIManager.Instance.PlayMusic (UIManager.Instance.clipOwnerTrick, 3f);	break;
		}

		UITweenAlpha.Start (objSelect, 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		countdown.Set (20, () => OnCancel ());
	}

	public void OnOK()
	{
		_ShowLoading ();
	}

	public void OnCancel()
	{
		Info.GameDiscountWon = -1;
		countdown.Stop ();
		UIManager.Instance.StopMusic ();
		UIManager.Instance.Hide (uiType);
	}
}