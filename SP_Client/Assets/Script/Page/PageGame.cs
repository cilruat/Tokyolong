using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESlotType
{
	eDiscount,
	eGameType,
	eGame,
}

public class PageGame : PageBase {

	enum EDiscount
	{
		e500won,
		e1000won
	}

	public enum EGameType
	{
		eWinWaiter,
		eTokyoLive,
		eBrainSurvival,
		eBoardGame
	}

	enum EWaiterEasyGame
	{
		eRockPaperScissors,
		eSniffling,
		eFrontBack,
		eLieDetector
	}

	enum EWaiterHardGame
	{
		e369,
		eSpeakingWords,
		Chopsticks,
		Dice,
		TraditionalPlay
	}

	enum EBrainSurvival
	{
		eLadderRiding,
		eFindNumber
	}

	enum EBoardGame
	{
		PunchKing,
		HammerKing,
		CrocodileRoulette,
		TurnPlate,
		RussianRoulette
	}
		
	public CanvasGroup[] cgBoards;
	public Text[] txtPlayCnt;
	public GameObject[] objStartDesc;
	public GameObject objCallMessage;
	public GameObject objGameLoading;
	public RectTransform[] rtWinWaiterEasyGames;
	public RectTransform[] rtWinWaiterHardGames;
	public RectTransform[] rtBrainSurvival;
	public RectTransform[] rtBoardGames;
	public List<SlotMachineElt> listSlotMachine = new List<SlotMachineElt>();
	public CountDown[] countDown;

	bool isStopEnable = false;
	int curDiscount = -1;
	int curGameType = -1;
	int curGame = -1;

	protected override void Awake ()
	{
		base.boards = cgBoards;
		base.Awake ();
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
				curDiscount = stopIdx;
				break;
			case 1:		
				stopIdx = UnityEngine.Random.Range (0, 4);
				curGameType = stopIdx;
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
		RectTransform[] rt = _AllRtElts ();
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
		
		for (int i = 0; i < objStartDesc.Length; i++)
			objStartDesc [i].SetActive (false);
		StartCoroutine (_StartSlot ());

		if (Info.GamePlayCnt > 0) {
			Info.GamePlayCnt -= 1;
			_RefreshPlayCnt ();
		}
	}

	IEnumerator _StopSlot()
	{
		int randRange = 0;
		RectTransform[] rtElts = null;

		if (curGameType == (int)EGameType.eBrainSurvival) {
			randRange = Enum.GetValues (typeof(EBrainSurvival)).Length;
			rtElts = rtBrainSurvival;
		}
		if (curGameType == (int)EGameType.eBoardGame) {
			randRange = Enum.GetValues (typeof(EBoardGame)).Length;
			rtElts = rtBoardGames;
		} else if (curGameType == (int)EGameType.eWinWaiter) {
			if (curDiscount == (int)EDiscount.e500won) {
				randRange = Enum.GetValues (typeof(EWaiterEasyGame)).Length;
				rtElts = rtWinWaiterEasyGames;
			} else if (curDiscount == (int)EDiscount.e1000won) {
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
				
			listSlotMachine [i].StopSlot ();
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
		int countIdx = 0;
		if (curGameType == (int)EGameType.eBrainSurvival ||
		    curGameType == (int)EGameType.eTokyoLive)
			objGameLoading.SetActive (true);
		else {
			countIdx = 1;
			objCallMessage.SetActive (true);
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
				case EBrainSurvival.eLadderRiding:
					sceneName = "LadderRiding";
					break;
				case EBrainSurvival.eFindNumber:
					sceneName = "FindNumber";
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
			OnPrev ();
		}
	}

	public EGameType SelectGameType() { return (EGameType)curGameType; }
}
