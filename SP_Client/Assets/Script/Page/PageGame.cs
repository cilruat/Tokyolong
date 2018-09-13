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
	public GameObject[] objCallMessage;
	public GameObject objGameLoading;
	public RectTransform[] rtDiscount;
	public RectTransform[] rtGameTypes;
	public RectTransform[] rtWinWaiter;
	public RectTransform[] rtPuzzleGame;
	public RectTransform[] rtTabletGame;
	public List<SlotMachineElt> listSlotMachine = new List<SlotMachineElt>();
	public CountDown countDownGameLoading;

	bool isStopEnable = false;
	bool clickStop = false;
	int curGameType = -1;
	int curGame = -1;

	short runInGameDiscount = (short)EDiscount.e1000won;
	int runInGameType = (int)EGameType.eTabletGame;
	int runInGame = 0;

	protected override void Awake ()
	{
		base.boards = cgBoards;
		base.Awake ();

		Info.GameDiscountWon = -1;
	}

	void Start()
	{		
		RefreshPlayCnt ();
	}

	bool isForceSelectGame = false;
	protected override void Update()
	{
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.F))
			isForceSelectGame = !isForceSelectGame;
	}

    IEnumerator _StartSlot(short discountType)
	{
		isStopEnable = false;

		for (int i = 0; i < listSlotMachine.Count; i++) {
			int stopIdx = -1;
			switch (i) {
                case 0:					
                Info.GameDiscountWon = discountType;
                stopIdx = (short)discountType;
				break;
                case 1:
                if (discountType == (short)EDiscount.e1000won)
                {
                    float percent = UnityEngine.Random.Range(0f, 1f);
                    stopIdx = _GetGameTypeIdx(percent);
                }
                else
                    stopIdx = 2;
				curGameType = stopIdx;
				break;
			case 2:
				/*RectTransform[] rtElts = _AllRtElts ();
				listSlotMachine [i].SetElts (rtElts);*/
				break;
			}				

			listSlotMachine [i].StartSlot (stopIdx);
			yield return new WaitForSeconds (.1f);
		}

		isStopEnable = true;

		yield return new WaitForSeconds (2f);

		if (clickStop == false)
			OnStop ();
	}
	
	int _GetGameTypeIdx(float percent)
	{
		if (Info.RunInGameScene || isForceSelectGame) {
			return runInGameType;
		} else {
			if (percent <= .1f)			return 0;
			else if (percent > .75f)	return 1;
			else						return 2;
		}
	}

	RectTransform[] _AllRtElts()
	{		
		List<RectTransform> list = new List<RectTransform> ();
        for (int i = 0; i < rtWinWaiter.Length; i++)	list.Add (rtWinWaiter[i]);
        for (int i = 0; i < rtPuzzleGame.Length; i++)	list.Add (rtPuzzleGame[i]);
        for (int i = 0; i < rtTabletGame.Length; i++)	list.Add (rtTabletGame[i]);

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
        if (_CheckSlotAnimating()) {
            SystemMessage.Instance.Add ("이미 슬롯이 동작중이에요~~");
            return;
        }

		if (Info.GamePlayCnt <= 0) {
			SystemMessage.Instance.Add ("게임을 시작할 수 없어요~\n주문 먼저 부탁드릴께요~~");
			return;
		}

		if (clickStop) {
			SystemMessage.Instance.Add ("잠시후 게임이 시작되므로 슬롯을 동작할 수 없어요~~");
			return;
		}

		if (Info.RunInGameScene)
			FinishStart(runInGameDiscount);
        else
            NetworkManager.Instance.SlotStart_REQ();
	}

    public void FinishStart(short discountType)
	{
		for (int i = 0; i < objStartDesc.Length; i++)
			objStartDesc [i].SetActive (false);

        StartCoroutine (_StartSlot (discountType));
		RefreshPlayCnt ();
	}

	IEnumerator _StopSlot()
	{
		int randRange = 0;
		RectTransform[] rtElts = null;

		switch (Info.GameDiscountWon) {
		case (short)EDiscount.e1000won:
			if (curGameType == (int)EGameType.eWinWaiter) {
				randRange = Enum.GetValues (typeof(EWinWaiter)).Length;
				rtElts = rtWinWaiter;
			} else if (curGameType == (int)EGameType.ePuzzleGame) {
				randRange = Enum.GetValues (typeof(EPuzzleGame)).Length;
				rtElts = rtPuzzleGame;
			} else if (curGameType == (int)EGameType.eTabletGame) {
				randRange = Enum.GetValues(typeof(ETabletGame)).Length;
				rtElts = rtTabletGame;
			}
			break;
		case (short)EDiscount.e5000won:
		case (short)EDiscount.eHalf:
		case (short)EDiscount.eAll:
			randRange = Enum.GetValues(typeof(ETabletGame)).Length;
			rtElts = rtTabletGame;
			break;
		}			

        if(randRange == 0)
        {
            randRange = Enum.GetValues(typeof(ETabletGame)).Length;
            rtElts = rtTabletGame;
        }

		int stopIdx = UnityEngine.Random.Range (0, randRange);
		if (Info.RunInGameScene || isForceSelectGame)
			stopIdx = runInGame;

		curGame = stopIdx;

		for (int i = 0; i < listSlotMachine.Count; i++) {
			if (i == listSlotMachine.Count - 1) {
				listSlotMachine [i].SetStopIdx (stopIdx);
				listSlotMachine [i].SetElts (rtElts);
			}
				
			listSlotMachine [i].StopSlot (false);
			yield return new WaitForSeconds (1f);
		}			
	}

	public void OnStop()
	{
		if (clickStop)
			return;

		if (isStopEnable == false) {
			SystemMessage.Instance.Add ("아직 정지를 할 수 없습니다");
			return;
		}

		if (_CheckSlotStopAnimating()) {
			SystemMessage.Instance.Add ("슬롯이 정지중입니다");
			return;
		}			

		clickStop = true;
		StartCoroutine (_StopSlot ());
	}

	public void RefreshPlayCnt()
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

	public void OnGoPracticeGame()
	{
		SceneChanger.LoadScene ("PracticeGame", curBoardObj ());
	}

	public void ShowPopup()
	{
		clickStop = false;

		if (curGameType == (int)EGameType.eWinWaiter) {
			UITweenAlpha.Start (objCallMessage[curGame], 0f, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));
			_FinishShowPopup ();
		} else {
			objGameLoading.SetActive (true);
			countDownGameLoading.Set (3, () => _FinishShowPopup ());
		}
	}

	void _FinishShowPopup()
	{
		string sceneName = "";
        if (curGameType == (int)EGameType.eWinWaiter) {
            for (int i = 0; i < objStartDesc.Length; i++)
                objStartDesc [i].SetActive (true);

            objGameLoading.SetActive (false);
            _SetActiveAllRtElts (false);

            Info.GameDiscountWon = -1;            
		} else {            
            if (curGameType == (int)EGameType.ePuzzleGame)
            {
                switch ((EPuzzleGame)curGame)
                {
                    case EPuzzleGame.ePicturePuzzle:    sceneName = "PicturePuzzle";    break;
                    case EPuzzleGame.ePairCards:        sceneName = "PairCards";        break;
                }
            }
            else if (curGameType == (int)EGameType.eTabletGame)
            {
                switch ((ETabletGame)curGame)
                {
                    case ETabletGame.CrashCat:      sceneName = "CrashCatStart";     		break;
                    case ETabletGame.FlappyBird:    sceneName = "FlappyBirdMasterMain";   	break;
                    case ETabletGame.DownHill:      sceneName = "Emoji2Main";       		break;
                    case ETabletGame.SlidingDown:   sceneName = "EmojiMain";        		break;
                    //case ETabletGame.AvoidObject:   sceneName = "AvoidMain";     break;
                }
            }

            SceneChanger.LoadScene (sceneName, curBoardObj ());			
		}

		curGameType = -1;
		curGame = -1;
	}

	public void OnCloseCallMsg(int idx)
	{
		UITweenAlpha.Start (objCallMessage [idx], 1f, 0f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2).DisableOnFinish ());
	}

	public EGameType SelectGameType() { return (EGameType)curGameType; }
}
