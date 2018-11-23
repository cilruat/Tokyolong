using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PageTaro : SingletonMonobehaviour<PageTaro> {

	enum EShowType
	{
		eSelect = 0,
		eFirst,
		eSecond,
		eResult,
	}

	public GameObject objCard;
	public GameObject objBoard;
	public Image[] imgResult;	
	public RectTransform[] rtParent;
	public CanvasGroup[] cgBoards;

	const int FIRST_CARD_CNT = 22;
	const int SECOND_CARD_CNT = 56;

	int firstIdx = -1;
	int secondIdx = -1;
	Sprite firstCard = null;
	Sprite secondCard = null;

	bool isLove = false;
	bool clicked = false;
	EShowType eType = EShowType.eSelect;

	Dictionary<string, string> dictLove = new Dictionary<string, string> ();
	Dictionary<string, string> dictMoney = new Dictionary<string, string> ();

	void Awake()
	{
		_DataLoad (@"\Info\TaroLove.csv", true);
		_DataLoad (@"\Info\TaroMoney.csv", false);
	}

	void _DataLoad(string csv_path, bool isLove)
	{
		string path = Application.dataPath;
		int lastIdx = path.LastIndexOf (@"/");

		path = path.Substring(0, lastIdx) + csv_path;
		List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
		data = CSVReader.Read(path);

		for (int i = 0; i < data.Count; i++) {
			string result = data [i] ["result"].ToString ();
			string desc = data [i] ["desc"].ToString ();

			if (isLove)	dictLove.Add (result, desc);
			else		dictMoney.Add (result, desc);
		}
	}

	void _Move(bool isNext)
	{
		int change = isNext ? (int)eType + 1 : 0;

		UnityEvent ue = new UnityEvent ();
		ue.AddListener (() => _RefreshBoard ());

		Info.AnimateChangeObj (cgBoards [(int)eType], cgBoards [change], ue);
		eType = (EShowType)change;
	}

	void _RefreshBoard()
	{
		clicked = false;

		switch (eType) {
		case EShowType.eSelect:		break;
		case EShowType.eFirst:		StartCoroutine (_ShowCard (true));		break;
		case EShowType.eSecond:		StartCoroutine (_ShowCard (false));		break;
		case EShowType.eResult:		break;
		}
	}

	IEnumerator _ShowSelect()
	{
		yield break;
	}

	IEnumerator _ShowCard(bool isFirst)
	{
		int cnt = isFirst ? FIRST_CARD_CNT : SECOND_CARD_CNT;
		RectTransform parent = isFirst ? rtParent [0] : rtParent [1];

		for (int i = 0; i < cnt; i++) {
			GameObject obj = Instantiate (objCard) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (parent);
			tr.InitTransform ();

			Button btn = obj.GetComponent<Button> ();
			btn.onClick.RemoveAllListeners ();
			btn.onClick.AddListener (() => StartCoroutine (_ClickCard (obj.GetComponent<Image> ())));

			yield return new WaitForSeconds (.01f);
		}			
	}

	IEnumerator _ShowResult()
	{
		yield break;
	}

	IEnumerator _ClickCard(Image img)
	{
		if (img == null)	yield break;
		if (clicked)		yield break;

		clicked = true;

		int randCard = Random.Range (0, eType == EShowType.eFirst ? FIRST_CARD_CNT : SECOND_CARD_CNT);
		string cardNum = randCard < 10 ? randCard.ToString ("00") : randCard.ToString ();

		string frontName = eType == EShowType.eFirst ? "Sprites/Taro22/" : "Sprites/Taro56/";
		string endName = eType == EShowType.eFirst ? "Taro_mj" : "Taro_";
		string fullName = frontName + endName + cardNum;

		Sprite sprite = Resources.Load<Sprite> (fullName);
		img.sprite = sprite;

		if (eType == EShowType.eFirst) {
			firstIdx = randCard;
			firstCard = sprite;
		} else {
			secondIdx = randCard;
			secondCard = sprite;
		}

		yield return new WaitForSeconds (.75f);
		_Move (true);
	}		

	public void OnSelect(bool isLove)
	{
		if (clicked)
			return;

		clicked = true;

		this.isLove = isLove;
		_Move (true);
	}

	public void ReturnRestart()
	{
		firstIdx = -1;
		secondIdx = -1;
		firstCard = null;
		secondCard = null;

		_Move (false);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
