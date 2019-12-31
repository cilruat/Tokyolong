using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PageMind : PageBase {

	public CanvasGroup[] cgBoard;

	public CanvasGroup cgFirst;
	public CanvasGroup[] cgTapGroups;
	public GameObject objReturnHome;
	public GameObject objReturnPrev;
	public GameObject objReturnFirst;

	CanvasGroup _prevCG;
	CanvasGroup _curCG;

	protected override void Awake ()
	{
		base.boards = cgBoard;
		base.Awake ();

		_curCG = cgFirst;
		_ChangeReturn (true);
	}

	void _ChangeShow(CanvasGroup nextCG)
	{
		_prevCG = _curCG;
		Info.AnimateChangeObj (_curCG, nextCG);
		_curCG = nextCG;

		if (nextCG.transform.parent.name == "Second") {
			for (int i = 0; i < cgTapGroups.Length; i++) {
				UITweenAlpha.Start (cgTapGroups [i].gameObject, 1f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
				cgTapGroups [i].blocksRaycasts = true;
			}
		} else {
			for (int i = 0; i < cgTapGroups.Length; i++) {
				cgTapGroups [i].alpha = 0;
				cgTapGroups [i].blocksRaycasts = false;
			}
		}
	}

	void _ChangeReturn(bool isHome)
	{
		objReturnHome.SetActive (isHome);
		objReturnPrev.SetActive (!isHome);
		objReturnFirst.SetActive (!isHome);
	}

	public void OnPrev() 
	{
		if (_prevCG == cgFirst)
			OnGoFirst ();
		else
			_ChangeShow (_prevCG);
	}

	public void OnClick(CanvasGroup showCG)
	{
		_ChangeShow (showCG);
		_ChangeReturn (false);
	}

	public void OnGoFirst()
	{
		_ChangeShow (cgFirst);
		_ChangeReturn (true);
	}
}