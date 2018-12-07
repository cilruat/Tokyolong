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
	public Text descResult;
	public Image[] imgResult;
	public RectTransform[] rtResultBack;
	public GameObject[] objResultBtns;
	public RectTransform[] rtParent;
	public CanvasGroup[] cgBoards;

	const int FIRST_CARD_CNT = 22;
	const int SECOND_CARD_CNT = 56;

	string firstIdx = "";
	string secondIdx = "";

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

	void _Init()
	{
		firstIdx = "";
		secondIdx = "";

		for (int i = 0; i < 2; i++) {

			int remove_cnt = 0;
			Transform parent = rtParent [i].transform;
			for (int j = 0; j < parent.childCount; j++) {
				Transform child = parent.GetChild (j);
				if (child)
					Destroy (child.gameObject);
			}

			imgResult [i].sprite = null;
			imgResult [i].gameObject.SetActive (false);
			objResultBtns [i].SetActive (false);
			rtResultBack [i].localEulerAngles = Vector3.zero;
		}

		descResult.text = "";
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
		switch (eType) {
		case EShowType.eSelect:		_Init ();								break;
		case EShowType.eFirst:		StartCoroutine (_ShowCard (true));		break;
		case EShowType.eSecond:		StartCoroutine (_ShowCard (false));		break;
		case EShowType.eResult:		StartCoroutine (_ShowResult ()); 		break;
		}

		clicked = false;
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
		for (int i = 0; i < rtResultBack.Length; i++) {
			StartCoroutine (_RollingResultCard (rtResultBack [i], i));
			yield return new WaitForSeconds (.02f);
		}

		string desc = "검색 결과가 없어용 ㅠㅠ 빨리 준비하도록 할게요 ㅠㅠ";
		string find = firstIdx + secondIdx + ".jpg";
		Debug.Log ("find: " + find);
		if (isLove) {
			if (dictLove.ContainsKey (find))
				desc = dictLove [find];
		} else {
			if (dictMoney.ContainsKey (find))
				desc = dictMoney [find];
		}

		descResult.text = desc;
		UITweenAlpha.Start (descResult.gameObject, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
		yield return new WaitForSeconds (.5f);

		for (int i = 0; i < objResultBtns.Length; i++)
			UITweenAlpha.Start (objResultBtns [i], 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

		yield break;
	}

	IEnumerator _RollingResultCard(RectTransform rt, int idx)
	{
		float y = rt.localEulerAngles.y;
		float end_y = 180f;

		Vector3 start = Vector3.up * y;
		Vector3 end = Vector3.up * end_y;

		while (true) {
			Vector3 rot = Vector3.MoveTowards (start, end, 5f);
			rt.Rotate (rot, Space.Self);

			if (rt.localEulerAngles.y >= end_y * .5f)
				imgResult [idx].gameObject.SetActive (true);

			if (rt.localEulerAngles.y >= end_y)
				break;

			yield return null;
		}

		rt.localEulerAngles = new Vector3 (0f, end_y, 0f);
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
			firstIdx = cardNum;
			imgResult [0].sprite = sprite;
		} else {
			secondIdx = cardNum;
			imgResult [1].sprite = sprite;
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
		clicked = true;
		_Move (false);
	}

	public void ReturnHome()
	{
		SceneChanger.LoadScene ("Main", objBoard);
	}
}
