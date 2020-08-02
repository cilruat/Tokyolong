using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using LitJson;

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
			case EDiscount.e1000won:	return 20;
			case EDiscount.e2000won:	return 30;
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
			case EDiscount.e1000won:	return 25;
			case EDiscount.e2000won:	return 40;
			case EDiscount.e5000won:	return 50;
			case EDiscount.eAll:		return 60;
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
			case EDiscount.e2000won:	return 30;
			case EDiscount.e5000won:	return 36;
			case EDiscount.eAll:		return 36;
			default:					return 30;
			}
		}
	}
	public static int PAIR_CARD_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 20;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 15;
			case EDiscount.eAll:		return 10;
			default:					return 15;
			}
		}
	}

	public static int TOUCH_NUMBER_LIMIT_TIME {
		get{
			switch ((EDiscount)GameDiscountWon) {
			case EDiscount.e500won:
			case EDiscount.e1000won:	return 8;
			case EDiscount.e2000won:	
			case EDiscount.e5000won:	return 5;
			case EDiscount.eAll:		return 3;
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
    public static int AdminTableNum = 10000;
    public static byte TableNum = 0;
    public static byte PersonCnt = 0;
    public static ECustomerType ECustomer = ECustomerType.MAN;

    public static int orderCnt = 0;
	public static bool firstOrder = false;

    public static bool RunInGameScene = false;

	public static bool CheckOwnerEvt = false;
	public static eUI OwnerUI = eUI.eOwnerGame;

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

	public static bool CheckGameScene(string sceneName)
	{
		if (sceneName == "PicturePuzzle"     		||
			sceneName == "PairCards"         		||
			sceneName == "FlappyBirdMasterMain"    	||
			sceneName == "CrashCatMain"      		||
			sceneName == "CrashCatStart"     		||
			sceneName == "TouchNumber"				||
			sceneName == "FindDiffPicture"			||
			sceneName == "Trickery"					||
			sceneName == "CS_Level5"				||
			sceneName == "STK_Level9_63"			||
			sceneName == "TakguMain"				||
			sceneName == "SFGGame"					||
			sceneName == "CS_Game"					||
			sceneName == "2Players_Touchdown"		||
			sceneName == "TableDemo"				||
			sceneName == "AirHockeyGame"			||
			sceneName == "CrazyBallGame"			||
			sceneName == "WAMGame"					||
			sceneName == "OneTapSoccer"				||
			sceneName == "LiveExample-01"			||
			sceneName == "LiveExample-02"			||
			sceneName == "LiveExample-03"			||
			sceneName == "CSHard"					||
			sceneName == "ECCGameTerrain"			||
			sceneName == "SRGStage03"				||
			sceneName == "SGTGame"					||
			sceneName == "BSKArcade"				||
            sceneName == "Mail"                     ||
            sceneName == "TwoCarsMain"				

			/*
			sceneName == "EmojiMain"         		||
			sceneName == "Emoji2Main"        		||
			sceneName == "AvoidBullets"      		||
			sceneName == "AvoidGame"         		||
			sceneName == "AvoidMain"         		||
			sceneName == "BallDuetMain"				||
			sceneName == "JumperStepUpMain"			||
			sceneName == "HammerMain"				||
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
		else if (Info.isCheckScene ("Trickery"))
			gamemanager.instance.ReturnHome ();
			
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
		NetworkManager.Instance.Game_Discount_REQ (Info.GameDiscountWon);
	}

	public static void SendEventMenu()
	{
		int menu = (int)EMenuDetail.eSozuFreeEvent;
		int cnt = 1;
		SendMenu send = new SendMenu (menu, cnt);

		List<SendMenu> list = new List<SendMenu> (){ send };

		JsonData json = JsonMapper.ToJson (list);
		NetworkManager.Instance.Order_REQ (json.ToString (), 0);
	}
}
