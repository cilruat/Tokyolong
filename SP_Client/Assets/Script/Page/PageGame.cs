using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public enum ESlotType
{
	eDiscount,
	eGameType,
	eGame,
}

public partial class PageGame : PageBase {
		
	public CanvasGroup[] cgBoards;
	public Text[] txtPlayCnt;
	public GameObject[] objStartDesc;
	public GameObject objCallMessage;
	public GameObject objGameLoading;
	public RectTransform[] rtDiscount;
	public RectTransform[] rtGameTypes;
	public RectTransform[] rtWinWaiterEasyGames;
	public RectTransform[] rtWinWaiterHardGames;
	public RectTransform[] rtBrainSurvival;
	public RectTransform[] rtBoardGames;
	public List<SlotMachineElt> listSlotMachine = new List<SlotMachineElt>();
	public CountDown[] countDown;

	bool isStopEnable = false;
	int curGameType = -1;
	int curGame = -1;

	protected override void Awake ()
	{
		base.boards = cgBoards;
		base.Awake ();

		Info.GameDiscountWon = -1;
	}

	void Start()
	{
		NetworkManager.Instance.UnfinishGamelist_REQ (Info.TableNum);
		_RefreshPlayCnt ();
	}

	IEnumerator _StartSlot()
	{
		isStopEnable = false;

		for (int i = 0; i < listSlotMachine.Count; i++) {
			int stopIdx = -1;
			switch (i) {
			case 0:	
				stopIdx = UnityEngine.Random.Range (0, 2);
				Info.GameDiscountWon = (short)stopIdx;
				break;
			case 1:
				float percent = UnityEngine.Random.Range (0f, 1f);
				Debug.Log ("slot percent: " + percent);
				stopIdx = _GetGameTypeIdx (percent);
				curGameType = stopIdx;
                Debug.Log ("curGameType : " + curGameType);
				break;
			case 2:
				RectTransform[] rtElts = _AllRtElts ();
				listSlotMachine [i].SetElts (rtElts);
				break;
			}				

			listSlotMachine [i].StartSlot (stopIdx);
			yield return new WaitForSeconds (.1f);
		}

		isStopEnable = true;
	}
	
	int _GetGameTypeIdx(float percent)
	{
		if (percent < .4f)			return 0;
		else if (percent < .8f)		return 3;
		else if (percent < .9f)		return 1;
		else						return 2;
	}

	RectTransform[] _AllRtElts()
	{		
		List<RectTransform> list = new List<RectTransform> ();
		for (int i = 0; i < rtWinWaiterEasyGames.Length; i++)	list.Add (rtWinWaiterEasyGames [i]);
		for (int i = 0; i < rtWinWaiterHardGames.Length; i++)	list.Add (rtWinWaiterHardGames[i]);
		for (int i = 0; i < rtBrainSurvival.Length; i++)		list.Add (rtBrainSurvival[i]);
		for (int i = 0; i < rtBoardGames.Length; i++)			list.Add (rtBoardGames[i]);

		return list.ToArray ();
	}

	void _SetActiveAllRtElts(bool active)
	{
		RectTransform[] rt = rtDiscount;
		for (int i = 0; i < rt.Length; i++)
			rt [i].gameObject.SetActive (active);

		rt = rtGameTypes;
		for (int i = 0; i < rt.Length; i++)
			rt [i].gameObject.SetActive (active);

		rt = _AllRtElts ();
		for (int i = 0; i < rt.Length; i++)
			rt [i].gameObject.SetActive (active);
	}

	public void OnStart()
	{		
		if (Info.GamePlayCnt <= 0) {
			SystemMessage.Instance.Add ("게임을 시작할 수 없습니다\n주문을 먼저 해주세요~");
			return;
		}

		if (_CheckSlotAnimating()) {
			SystemMessage.Instance.Add ("이미 슬롯이 동작중입니다");
			return;
		}

		NetworkManager.Instance.SlotStart_REQ ();
	}

	public void FinishStart()
	{
		for (int i = 0; i < objStartDesc.Length; i++)
			objStartDesc [i].SetActive (false);

		StartCoroutine (_StartSlot ());
		_RefreshPlayCnt ();
	}

	IEnumerator _StopSlot()
	{
		int randRange = 0;
		RectTransform[] rtElts = null;

		if (curGameType == (int)EGameType.eBrainSurvival) {
			randRange = Enum.GetValues (typeof(EBrainSurvival)).Length;
			rtElts = rtBrainSurvival;
		} else if (curGameType == (int)EGameType.eBoardGame) {
			randRange = Enum.GetValues (typeof(EBoardGame)).Length;
			rtElts = rtBoardGames;
		} else if (curGameType == (int)EGameType.eWinWaiter) {
			if (Info.GameDiscountWon == (int)EDiscount.e500won) {
				randRange = Enum.GetValues (typeof(EWaiterEasyGame)).Length;
				rtElts = rtWinWaiterEasyGames;
			} else if (Info.GameDiscountWon == (int)EDiscount.e1000won) {
				randRange = Enum.GetValues (typeof(EWaiterHardGame)).Length;
				rtElts = rtWinWaiterHardGames;
			}
		}

		int stopIdx = UnityEngine.Random.Range (0, randRange);
		curGame = stopIdx;
		
		for (int i = 0; i < listSlotMachine.Count; i++) {
			if (i == listSlotMachine.Count - 1) {
				listSlotMachine [i].SetStopIdx (stopIdx);
				listSlotMachine [i].SetElts (rtElts);
			}
				
			listSlotMachine [i].StopSlot (false);
			yield return new WaitForSeconds (.1f);
		}			
	}

	public void OnStop()
	{
		if (isStopEnable == false) {
			SystemMessage.Instance.Add ("아직 정지를 할 수 없습니다");
			return;
		}

		if (_CheckSlotStopAnimating()) {
			SystemMessage.Instance.Add ("슬롯이 정지중입니다");
			return;
		}

		StartCoroutine (_StopSlot ());
	}

	void _RefreshPlayCnt()
	{
		for (int i = 0; i < txtPlayCnt.Length; i++)
			txtPlayCnt [i].text = Info.GamePlayCnt.ToString ();
	}

	bool _CheckSlotAnimating()
	{
		bool isAni = false;
		for (int i = 0; i < listSlotMachine.Count; i++) {
			if (listSlotMachine [i].Animating () == false)
				continue;
			
			isAni = true;
			break;
		}

		return isAni;
	}

	bool _CheckSlotStopAnimating()
	{
		bool isAni = false;
		for (int i = 0; i < listSlotMachine.Count; i++) {
			if (listSlotMachine [i].StopAnimating () == false)
				continue;

			isAni = true;
			break;
		}

		return isAni;
	}

	public void OnCheckPrev()
	{
		if (_CheckSlotAnimating()) {
			SystemMessage.Instance.Add ("슬롯이 동작중일경우 이전화면으로 갈 수 없습니다");
			return;
		}

		base.OnPrev ();
	}

	public void OnCheckNext()
	{
		if (Info.GamePlayCnt <= 0) {
			SystemMessage.Instance.Add ("게임을 시작할 수 없습니다\n주문을 먼저 해주세요~");
			return;
		}

		base.OnNext ();
	}

	public void OnGoOrder()
	{
		SceneChanger.LoadScene ("Order", curBoardObj ());
	}

	public void ShowPopup()
	{
        Debug.Log ("ShowPopup!!!!");
		int countIdx = 0;
        if (curGameType == (int)EGameType.eBrainSurvival ||
        curGameType == (int)EGameType.eTokyoLive)
        {
            Debug.Log ("objGameLoading!!!!");
            objGameLoading.SetActive(true);
        }
		else {
			countIdx = 1;
			objCallMessage.SetActive (true);
            Debug.Log ("ReportOfflineGame_REQ!!!!");
			NetworkManager.Instance.ReportOfflineGame_REQ ((byte)curGameType, (byte)curGame, (byte)Info.GameDiscountWon);
		}
		
		countDown[countIdx].Set (3, () => _FinishShowPopup ());
	}

	void _FinishShowPopup()
	{
		string sceneName = "";
		if (curGameType == (int)EGameType.eBrainSurvival ||
		    curGameType == (int)EGameType.eTokyoLive) {

			sceneName = "TokyoLive";
			if (curGameType == (int)EGameType.eBrainSurvival) {
				switch ((EBrainSurvival)curGame) {
				case EBrainSurvival.ePicturePuzzle:
					sceneName = "PicturePuzzle";
					break;
				case EBrainSurvival.ePairCards:
					sceneName = "PicturePuzzle";
                    break;
				}
			}

			SceneChanger.LoadScene (sceneName, curBoardObj ());
		} else {
			for (int i = 0; i < objStartDesc.Length; i++)
				objStartDesc [i].SetActive (true);

			objGameLoading.SetActive (false);
			objCallMessage.SetActive (false);

			_SetActiveAllRtElts (false);

			Info.GameDiscountWon = -1;
			OnPrev ();
		}

		curGameType = -1;
		curGame = -1;
	}

	public EGameType SelectGameType() { return (EGameType)curGameType; }
}
