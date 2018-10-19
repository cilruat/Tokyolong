using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public partial class Info : MonoBehaviour {
    
	// Game Info
	public static int EMOJI_DOWN_THE_HILL_FINISH_POINT = 30;
	public static int EMOJI_SLIDING_DOWN_FINISH_POINT = 18;
	public static int CRASH_CAT_LIMIT_TIME = 20;
	public static int FLAPPY_BIRD_LIMIT_TIME = 25;
	public static int PICTURE_PUZZLE_MODE = 3;
	public static int PICTURE_PUZZLE_LIMIT_TIME = 30;
	public static int PAIR_CARD_MODE = 18;
	public static int PAIR_CARD_LIMIT_TIME = 30;

	public static int RING_DING_DONG_FINISH_POINT = 30;
	public static int EGG_MON_FINISH_POINT = 20;
	public static int HAMMER_FINISH_POINT = 60;
	public static int TWO_CARS_FINISH_POINT = 20;
	public static int BRIDGES_FINISH_POINT = 20;
	public static int CRASH_RACING_LIMIT_TIME = 25;

	public const int TOKYOLIVE_MAX_COUNT = 3;
	public static int TOKYOLIVE_PREV_SEC = 20;
	static int[] TOKYOLIVE_START_TIME = { 0, 30 };

	public static int GamePlayCnt = 0;
	public static short GameDiscountWon = -1;	   
	public static int GAMEPLAY_MIN_COUNT = 0;
	public static int GAMEPLAY_MAX_COUNT = 50;

	public static bool practiceGame = false;

	// user Info
    public static byte TableNum = 0;
    public static byte PersonCnt = 0;
    public static ECustomerType ECustomer = ECustomerType.MAN;

    public static int orderCnt = 0;

	// TokyoLive Info
	public static int tokyoLiveCnt = 0;
    public static bool showTokyoLive = false;

    public static bool RunInGameScene = false;

	public static void AnimateChangeObj(CanvasGroup cur, CanvasGroup next, UnityEvent nextCallback = null)
    {
        UITweenAlpha.Start(cur.gameObject, 0f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));
		UITween uiNext = UITweenAlpha.Start (next.gameObject, 1f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
		if (nextCallback != null)
			uiNext.onCompleteFunc = nextCallback;

        cur.blocksRaycasts = false;
        next.blocksRaycasts = true;
    }

	public static string MakeMoneyString(int price)
	{
		return "￦ " + price.ToString ("N0");
	}

	public static bool isCheckScene(string scene)
	{
		return SceneManager.GetActiveScene ().name == scene;
	}

	public static int idRobot = -1;
    public static List<int> listGameCnt_Robot = new List<int>();
    public static List<int> listOrderCnt_Robot = new List<int>();

	public static bool IsInputFieldFocused()
	{
		GameObject objSelected = EventSystem.current.currentSelectedGameObject;
		return objSelected != null && objSelected.GetComponent<InputField> () != null;
	}

	public static void UpdateTokyoLiveTime()
	{
		if (tokyoLiveCnt >= TOKYOLIVE_MAX_COUNT)
			return;

		if (UIManager.Instance.IsActive (eUI.eTokyoLive))
			return;

        if (CheckGameScene(SceneManager.GetActiveScene().name))
            return;

		_CheckTokyoLivePrevStart ();
	}

	static void _CheckTokyoLivePrevStart()
	{
		if (_GetTokyoLivePrevTime (TOKYOLIVE_START_TIME [0]) ||
		    _GetTokyoLivePrevTime (TOKYOLIVE_START_TIME [1])) {
			if (UIManager.Instance.IsActive (eUI.eTokyoLive))
				return;

			GameObject obj = UIManager.Instance.Show (eUI.eTokyoLive);
			PageTokyoLive page = obj.GetComponent<PageTokyoLive> ();
			if (page)
				page.PrevSet ();
		}
	}

	static bool _GetTokyoLivePrevTime(int startTime)
	{
		bool prevStart = false;

		int min = DateTime.Now.Minute;
		int sec = DateTime.Now.Second;

		int prevMin = startTime == 0 ? 59 : startTime - 1;
		if (prevMin == min && sec == 60 - TOKYOLIVE_PREV_SEC)
			prevStart = true;

		return prevStart;
	}

	public static bool IsStartTokyoLiveTime()
	{
		int min = DateTime.Now.Minute;
		int sec = DateTime.Now.Second;
		return sec == 0 &&
		(min == TOKYOLIVE_START_TIME [0] || min == TOKYOLIVE_START_TIME [1]);
	}

	public static int GetGameCntByMenuType(EMenuType eType)
	{
		int cnt = 0;
		switch (eType) {
		case EMenuType.eSozu:
		case EMenuType.eBear:
		case EMenuType.eSake:
		case EMenuType.eFruitSozu:
		case EMenuType.eFruitMakgeolli:
		case EMenuType.eGin:	cnt = 1;	break;
		case EMenuType.eMeal:
		case EMenuType.eDrink:	cnt = 0;	break;
		default:				cnt = 2;	break;
		}

		return cnt;
	}

    public static void AddGameCount()
    {
        GamePlayCnt++;
        GamePlayCnt = Mathf.Clamp(GamePlayCnt, GAMEPLAY_MIN_COUNT, GAMEPLAY_MAX_COUNT);
    }

    public static void AddGameCount(int cnt, bool overwrite = false)
    {
        if (overwrite == false)
            GamePlayCnt += cnt;
        else
            GamePlayCnt = cnt;

        GamePlayCnt = Mathf.Clamp(GamePlayCnt, GAMEPLAY_MIN_COUNT, GAMEPLAY_MAX_COUNT);
    }

    public static void AddOrderCount(int cnt)
    {
        int value = orderCnt + cnt;

        orderCnt = Mathf.Clamp(value, GAMEPLAY_MIN_COUNT, GAMEPLAY_MAX_COUNT);
    }

	const int SURPRISE_REMAIN_MIN = 10;
	public static int surpriseCnt = 0;
	public static float loopSurpriseRemainTime = 0f;
	public static bool waitSurprise = false;

	public static void UpdateSurpriseRemainTime()
	{
		if (surpriseCnt <= 0)
			return;

		if (NetworkManager.isSending)
			return;

		if (UIManager.Instance.IsActive (eUI.eSurprise))
			return;

		if (waitSurprise) {
			if (showTokyoLive)
				return;

			if (CheckGameScene(SceneManager.GetActiveScene().name))
				return;

			NetworkManager.Instance.Surprise_REQ ();
			return;
		}

		loopSurpriseRemainTime += Time.deltaTime;
		if (SURPRISE_REMAIN_MIN <= Mathf.FloorToInt ((loopSurpriseRemainTime) / 60))
			waitSurprise = true;
	}

	public static bool CheckGameScene(string sceneName)
	{
		if (sceneName == "PicturePuzzle"     		||
			sceneName == "PairCards"         		||
			sceneName == "CrashCatMain"      		||
			sceneName == "CrashCatStart"     		||
			sceneName == "FlappyBirdMasterMain"    	||
			sceneName == "EmojiMain"         		||
			sceneName == "Emoji2Main"        		||
			sceneName == "AvoidBullets"      		||
			sceneName == "AvoidGame"         		||
			sceneName == "AvoidMain"         		||
			sceneName == "BallDuetMain"				||
			sceneName == "JumperStepUpMain"			||
			sceneName == "HammerMain"				||
			sceneName == "TwoCarsMain"				||
			sceneName == "BridgesMain"				||
			sceneName == "CrashRacingMain"			||
			sceneName == "CatchMole"				||
			sceneName == "PressNumber"				||
			sceneName == "FindDiffPicture"			)
			return true;
		else
			return false;
	}

	public static void PlayGame(int idx, GameObject obj)
	{
		string sceneName = "";
		switch (idx) {
		case 0:		sceneName = "PairCards";			break;
		case 1:		sceneName = "FlappyBirdMasterMain";	break;
		case 2:		sceneName = "CrashCatStart";		break;
		case 3:		sceneName = "CatchMole";			break;
		case 4:		sceneName = "PressNumber";			break;
		case 5:		sceneName = "FindDiffPicture";		break;
		/*case 6:		sceneName = "PicturePuzzle";		break;
		case 7:		sceneName = "EmojiMain";			break;
		case 8:		sceneName = "Emoji2Main";			break;
		case 9:		sceneName = "BallDuetMain";			break;
		case 10:	sceneName = "JumperStepUpMain";		break;
		case 11:	sceneName = "HammerMain";			break;
		case 12:	sceneName = "TwoCarsMain";			break;
		case 13:	sceneName = "BridgesMain";			break;
		case 14:	sceneName = "CrashRacingMain";		break;*/
		}

		SceneChanger.LoadScene (sceneName, obj);
	}

	public static void AfterDiscountBehavior()
	{		
		if (Info.isCheckScene ("TokyoLive"))
			PageTokyoLive.Instance.OnClose ();
		else if (Info.isCheckScene ("PicturePuzzle"))
			PagePicturePuzzle.Instance.ReturnHome ();
		else if (Info.isCheckScene ("PairCards"))
			PagePairCards.Instance.ReturnHome ();
		else if (Info.isCheckScene ("CrashCatMain"))
			CrashCat.GameManager.instance.ReturnHome ();
		else if (Info.isCheckScene ("EmojiMain"))
			Emoji.GameManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("Emoji2Main"))
			Emoji2.GameManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("FlappyBirdMasterMain"))
			FlappyBirdStyle.FlappyScript.instance.ReturnHome ();
		else if (Info.isCheckScene ("BallDuetMain"))
			OnefallGames.UIManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("JumperStepUpMain"))
			PauseMenu.Instance.ReturnHome ();
		else if (Info.isCheckScene ("HammerMain"))
			Hammer.UIManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("TwoCarsMain"))
			TwoCars.Managers.UI.ReturnHome ();
		else if (Info.isCheckScene ("BridgesMain"))
			Bridges.UIManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("CrashRacingMain"))
			CrashRacing.UIManager.Instance.ReturnHome ();
		/*else if (Info.isCheckScene ("CatchMole"))
			;
		else if (Info.isCheckScene ("PressNumber"))
			;
		else if (Info.isCheckScene ("FindDiffPicture"))
			;*/
	}

	public static int TotalGameCount() { return 6; }
}
