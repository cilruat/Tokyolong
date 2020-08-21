using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PageSsul : PageBase {

	public CanvasGroup[] cgBoard;

	public CanvasGroup cgFirst;
	public GameObject objReturnHome;
	public GameObject objReturnPrev;
	public GameObject objReturnFirst;


    CanvasGroup _prevCG;
	CanvasGroup _curCG;

	public void Awake ()
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