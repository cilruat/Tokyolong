using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageGame : PageBase {

	enum EDiscount
	{
		e500won,
		e1000won
	}

	enum EGameType
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

	enum EBoardGame
	{
		PunchKing,
		HammerKing,
		CrocodileRoulette,
		TurnPlate,
		RussianRoulette
	}

	public CanvasGroup[] cgBoards;
	public GameObject objStartDesc;
	public GameObject objCallMessage;
	public GameObject objGameLoading;
	public RectTransform[] rtWinWaiterEasyGames;
	public RectTransform[] rtWinWaiterHardGames;
	public RectTransform[] rtBoardGames;
	public List<SlotMachineElt> listSlotMachine = new List<SlotMachineElt>();

	bool isStopEnable = false;
	int curDiscount = -1;
	int curGameType = -1;
	int curGame = -1;

	protected override void Awake ()
	{
		base.boards = cgBoards;
		base.Awake ();
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
				int randRange = 0;
				RectTransform[] rtElts = null;
				if (curGameType == (int)EGameType.eBoardGame) {
					randRange = Enum.GetValues (typeof(EBoardGame)).Length;
					rtElts = rtBoardGames;
				}
				else if (curGameType == (int)EGameType.eWinWaiter) {
					if (curDiscount == (int)EDiscount.e500won) {
						randRange = Enum.GetValues (typeof(EWaiterEasyGame)).Length;
						rtElts = rtWinWaiterEasyGames;
					} else if (curDiscount == (int)EDiscount.e1000won) {
						randRange = Enum.GetValues (typeof(EWaiterHardGame)).Length;
						rtElts = rtWinWaiterHardGames;
					}
				}

				stopIdx = UnityEngine.Random.Range (0, randRange);
				curGame = stopIdx;

				listSlotMachine [i].SetElts (rtElts);
				break;
			}				

			listSlotMachine [i].StartSlot (stopIdx);
			yield return new WaitForSeconds (.1f);
		}

		isStopEnable = true;
	}

	public void OnStart()
	{
		objStartDesc.SetActive (false);
		StartCoroutine (_StartSlot ());
	}

	public void OnStop()
	{
		if (isStopEnable == false) {
			SystemMessage.Instance.Add ("아직 정지를 할 수 없습니다");
			return;
		}
	}
}
