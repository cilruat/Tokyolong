using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PageTaro : SingletonMonobehaviour<PageTaro> {

	public GameObject objCard;
	public GameObject objBoard;
	public Image[] imgResult;
	public CanvasGroup[] cgBoards;

	const int FIRST_CARD_CNT = 22;
	const int SECOND_CARD_CNT = 56;

	bool isLove = false;
	int curBoardIdx = 0;
	Dictionary<string, string> dictLove = new Dictionary<string, string> ();
	Dictionary<string, string> dictMoney = new Dictionary<string, string> ();

	void Awake ()
	{
		_DataLoad (@"\Info\TaroLove.csv");
		_DataLoad (@"\Info\TaroMoney.csv");
	}

	void _DataLoad(string csv_path)
	{
		string path = Application.dataPath;
		int lastIdx = path.LastIndexOf (@"/");

		path = path.Substring(0, lastIdx) + csv_path;
		List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
		data = CSVReader.Read(path);
	}

	void _Move(bool isNext)
	{
		int change = isNext ? curBoardIdx + 1 : 0;

		UnityEvent ue = new UnityEvent ();
		ue.AddListener (() => StartCoroutine (_RefreshBoard ()));

		Info.AnimateChangeObj (cgBoards [curBoardIdx], cgBoards [change], ue);
		curBoardIdx = change;
	}

	IEnumerator _RefreshBoard()
	{
		yield return new WaitForSeconds (1f);
	}

	public void OnSelect(bool isLove)
	{
		this.isLove = isLove;
		_Move (true);
	}

	public void ReturnRestart()
	{
		_Move (false);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
