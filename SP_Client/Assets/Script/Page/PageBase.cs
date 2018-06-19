using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PageBase : SingletonMonobehaviour<PageBase> {

	protected UnityAction acFinal;
	protected int acFinalIdx = -1;

	protected CanvasGroup[] boards;
	protected int curBoardIdx = 0;

	protected virtual void Awake()
	{
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2));

		if (boards != null) {
			for (int i = 0; i < boards.Length; i++) {
				boards [i].alpha = i == 0 ? 1f : 0f;
				boards [i].blocksRaycasts = i == 0;
			}
		}
	}		

	void _OnChangeBoard(bool isNext)
	{
		int change = isNext ? curBoardIdx + 1 : curBoardIdx - 1;

		UnityEvent ue = null;
		if (change == acFinalIdx) {
			ue = new UnityEvent ();
			ue.AddListener (acFinal);
		}

		Info.AnimateChangeObj (boards [curBoardIdx], boards [change], ue);
		curBoardIdx = change;
	}

	public void OnPrev() { _OnChangeBoard (false); }
	public void OnNext() { _OnChangeBoard (true); }

	public void ReturnHome()
	{ 
		SceneChanger.LoadScene ("Main", boards [0].gameObject);
	}

	public GameObject curBoardObj() { return boards [curBoardIdx].gameObject; }

}
