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
	// ---------------------Not Used ---------------------
	public static int EMOJI_DOWN_THE_HILL_FINISH_POINT = 30;
	public static int EMOJI_SLIDING_DOWN_FINISH_POINT = 18;
	public static int RING_DING_DONG_FINISH_POINT = 30;
	public static int EGG_MON_FINISH_POINT = 20;
	public static int HAMMER_FINISH_POINT = 60;
	public static int TWO_CARS_FINISH_POINT = 20;
	public static int BRIDGES_FINISH_POINT = 20;
	public static int CRASH_RACING_LIMIT_TIME = 25;
	public static int CATCH_MOLE_LIMIT_TIME = 30;
	// ---------------------Not Used ---------------------

	// ------------------Set Difficulty------------------
	public static int CRASH_CAT_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 15;
			case EDiscount.e2000won:	return 25;
			case EDiscount.e5000won:	return 40;
			case EDiscount.eAll:		return 50;
			default:					return 50;
			}
		}
	}

	public static int FLAPPY_BIRD_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 15;
			case EDiscount.e2000won:	return 30;
			case EDiscount.e5000won:	return 45;
			case EDiscount.eAll:		return 55;
			default:					return 55;
			}
		}
	}

	public static int PICTURE_PUZZLE_MODE {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 3;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 4;
			case EDiscount.eAll:		return 5;
			default:					return 5;
			}
		}
	}
	public static int PICTURE_PUZZLE_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	
			case EDiscount.e2000won:	return 20;
			case EDiscount.e5000won:	return 15;
			case EDiscount.eAll:		return 10;
			default:					return 10;
			}
		}
	}

	public static int PAIR_CARD_MODE {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	
			case EDiscount.e2000won:	return 18;
			case EDiscount.e5000won:	return 24;
			case EDiscount.eAll:		return 30;
			default:					return 30;
			}
		}
	}
	public static int PAIR_CARD_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 25;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 20;
			case EDiscount.eAll:		return 15;
			default:					return 15;
			}
		}
	}

	public static int TOUCH_NUMBER_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 15;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 12;
			case EDiscount.eAll:		return 10;
			default:					return 10;
			}
		}
	}
	public static int TOUCH_NUMBER_MAX_COUNT {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	
			case EDiscount.e2000won:	return 15;
			case EDiscount.e5000won:	return 20;
			case EDiscount.eAll:		return 25;
			default:					return 25;
			}
		}
	}

	public static int FIND_DIFF_PICTURE_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 25;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 18;
			case EDiscount.eAll:		return 15;
			default:					return 15;
			}
		}
	}
	// ------------------Set Difficulty------------------

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

	public static int GetGameCntByMenuType(EMenuType eType)
	{
		int cnt = 0;
		switch (eType) {
		case EMenuType.eTop:
		case EMenuType.eEasy:
		case EMenuType.eIzakaya:
		case EMenuType.eWomanTarget:
		case EMenuType.eHappy:
		case EMenuType.eSoup:			cnt = 2;	break;
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

	const int SURPRISE_REMAIN_MIN = 30;
	public static int surpriseCnt = 0;
	public static float loopSurpriseRemainTime = 0f;
	public static bool waitSurprise = false;

	/*static short _surprise_step = -1;
	public static short SURPRISE_STEP
	{
		set{ _surprise_step = value; Debug.Log ("Set SURPRISE_STEP: " + SURPRISE_STEP); }
		get{ return _surprise_step; }
	}*/
	public static short SURPRISE_STEP = -1;

	public static void UpdateSurpriseRemainTime()
	{
		if (surpriseCnt <= 0)
			return;

		if (Info.SURPRISE_STEP > -1)
			return;

		if (NetworkManager.isSending)
			return;

		if (UIManager.Instance.IsActive (eUI.eSurpriseStart))
			return;

		if (waitSurprise) {			
			if (SceneManager.GetActiveScene ().name == "Game")
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
			sceneName == "FlappyBirdMasterMain"    	||
			sceneName == "CrashCatMain"      		||
			sceneName == "CrashCatStart"     		||
			sceneName == "TouchNumber"				||
			sceneName == "FindDiffPicture"			/*||
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
			sceneName == "CrashRacingMain"*/
			)
			return true;
		else
			return false;
	}

	public static void PlayGame(int idx, GameObject obj)
	{
		string sceneName = "";
		switch (idx) {
		case 0:		sceneName = "PicturePuzzle";		break;
		case 1:		sceneName = "PairCards";			break;
		case 2:		sceneName = "FlappyBirdMasterMain";	break;
		case 3:		sceneName = "CrashCatStart";		break;
		case 4:		sceneName = "TouchNumber";			break;
		case 5:		sceneName = "FindDiffPicture";		break;
		/*case 6:		sceneName = "EmojiMain";			break;
		case 7:		sceneName = "Emoji2Main";			break;
		case 8:		sceneName = "BallDuetMain";			break;
		case 9:		sceneName = "JumperStepUpMain";		break;
		case 10:	sceneName = "HammerMain";			break;
		case 11:	sceneName = "TwoCarsMain";			break;
		case 12:	sceneName = "BridgesMain";			break;
		case 13:	sceneName = "CrashRacingMain";		break;*/
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
		else if (Info.isCheckScene ("FlappyBirdMasterMain"))
			FlappyBirdStyle.FlappyScript.instance.ReturnHome ();
		else if (Info.isCheckScene ("TouchNumber"))
			PageTouchNumber.Instance.ReturnHome ();
		else if (Info.isCheckScene ("FindDiffPicture"))
			PageFindDiffPicture.Instance.ReturnHome ();
		/*else if (Info.isCheckScene ("EmojiMain"))
			Emoji.GameManager.Instance.ReturnHome ();
		else if (Info.isCheckScene ("Emoji2Main"))
			Emoji2.GameManager.Instance.ReturnHome ();		
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
			CrashRacing.UIManager.Instance.ReturnHome ();*/
	}

	public static int TotalGameCount() { return 6; }
	public static int TotalGameCountWithTokyoLive() { return TotalGameCount () + 1;	}

	public static Color HexToColor(string hex)
	{
		if (hex.IndexOf ("#") > -1)
			hex.Replace ("#", "");

		if (hex.Length > 6) {
			int start = hex.Length - 6;
			hex = hex.Substring (start, 6);
		}

		int _hex = int.Parse (hex, System.Globalization.NumberStyles.AllowHexSpecifier);
		int r = _hex >> 16;
		int g = (_hex & 0x00ff00) >> 8;
		int b = _hex & 0x0000ff;

		return new Color (r / 255f, g / 255f, b / 255f);
	}

	public static void ShowResult()
	{
		if (Info.SURPRISE_STEP > -1) {
			GameObject obj = UIManager.Instance.Show (eUI.eSurpriseResult);
			UISurpriseResult ui = obj.GetComponent<UISurpriseResult>();
			ui.Show();
		}
		else
			NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}
}
