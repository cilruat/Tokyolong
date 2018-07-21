using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECustomerType : byte
{
	MAN = 0,
	WOMAN,
	COUPLE,
}

/// <summary>
/// menu
/// </summary>
public enum EMenuType : int
{
	eNone = -1,
	eTop = 0,		// 탑메뉴
	eParch,			// 볶음
	eFruit,			// 과일
	eFried,			// 튀김
	eSoup,			// 탕
	eSpecial = 5,	// 스페셜
	eCook,			// 식사
	eAlcohol,		// 주류
	eDrink,			// 음료
}

public enum EMenuDetail : int
{
	eNakgiBBokum = 0,
	eGawilModum,
	eSaeuTuikim,
	eNagasaki,
	eKrimSpageti,
	eGonggiBob = 5,
	eSoju,
	eCola,
}

/// <summary>
/// game
/// </summary>
public enum EDiscount
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

public enum EWaiterEasyGame
{
	eRockPaperScissors,
	eSniffling,
	eFrontBack,
	eLieDetector
}

public enum EWaiterHardGame
{
	e369,
	eSpeakingWords,
	Chopsticks,
	Dice,
	TraditionalPlay
}

public enum EBrainSurvival
{
	ePicturePuzzle,
	ePairCards
}

public enum EBoardGame
{
	PunchKing,
	HammerKing,
	CrocodileRoulette,
	TurnPlate,
	RussianRoulette
}

public partial class Info : MonoBehaviour {

    public static Dictionary<EMenuType, List<EMenuDetail>> dictStaticMenu = new Dictionary<EMenuType, List<EMenuDetail>>()
    {
        { EMenuType.eParch,     new List<EMenuDetail>() { EMenuDetail.eNakgiBBokum } },
        { EMenuType.eFruit,     new List<EMenuDetail>() { EMenuDetail.eGawilModum } },
        { EMenuType.eFried,     new List<EMenuDetail>() { EMenuDetail.eSaeuTuikim } },
        { EMenuType.eSoup,      new List<EMenuDetail>() { EMenuDetail.eNagasaki } },
        { EMenuType.eSpecial,   new List<EMenuDetail>() { EMenuDetail.eKrimSpageti } },
        { EMenuType.eCook,      new List<EMenuDetail>() { EMenuDetail.eGonggiBob } },
        { EMenuType.eAlcohol,   new List<EMenuDetail>() { EMenuDetail.eSoju } },
        { EMenuType.eDrink,     new List<EMenuDetail>() { EMenuDetail.eCola } },
    };

	public static void MenuTitle(EMenuType eType, ref string title, ref string subDesc)
	{		
		switch (eType) {
		case EMenuType.eTop:		title = "탑";		subDesc = "울집에서 젤 나가";	break;
		case EMenuType.eParch:		title = "볶음";		subDesc = "일본식 퓨전 볶음";	break;
		case EMenuType.eFruit:		title = "과일";		subDesc = "싱싱한 제철 과일";	break;
		case EMenuType.eFried:		title = "튀김";		subDesc = "튀기면 뭔들";		break;
		case EMenuType.eSoup:		title = "탕";		subDesc = "국물이 따끈따끈";	break;
		case EMenuType.eSpecial:	title = "스페셜";	subDesc = "특별한 맛이 끌릴때";	break;
		case EMenuType.eCook:		title = "식사";		subDesc = "배고플때 딱이야";	break;
		case EMenuType.eAlcohol:	title = "주류";		subDesc = "이성의 끈을 놓자";	break;
		case EMenuType.eDrink:		title = "음료";		subDesc = "시원한게 땡길때";	break;
		}
	}

	public static string MenuName(EMenuDetail eType)
	{
		string name = "";
		switch (eType) {
		case EMenuDetail.eNakgiBBokum:		name = "낚지볶음";			break;
		case EMenuDetail.eGawilModum:		name = "과일모듬";			break;
		case EMenuDetail.eSaeuTuikim:		name = "새우튀김";			break;
		case EMenuDetail.eNagasaki:			name = "나가사키짬뽕";		break;
		case EMenuDetail.eKrimSpageti:		name = "크림스파게티";		break;
		case EMenuDetail.eGonggiBob:		name = "공기밥";			break;
		case EMenuDetail.eSoju:				name = "소주";				break;
		case EMenuDetail.eCola:				name = "콜라";				break;
		}
		return name;
	}

	public static int MenuPrice(EMenuDetail eType)
	{
		int price = 0;
		switch (eType) {
		case EMenuDetail.eNakgiBBokum:		price = 9000;		break;
		case EMenuDetail.eGawilModum:		price = 12000;		break;
		case EMenuDetail.eSaeuTuikim:		price = 7900;		break;
		case EMenuDetail.eNagasaki:			price = 11000;		break;
		case EMenuDetail.eKrimSpageti:		price = 10000;		break;
		case EMenuDetail.eGonggiBob:		price = 1000;		break;
		case EMenuDetail.eSoju:				price = 4000;		break;
		case EMenuDetail.eCola:				price = 2000;		break;
		}
		return price;
	}

	public static string GameTitle(EGameType eType)
	{
		string title = "";
		switch (eType) {
		case EGameType.eWinWaiter:		title = "직원을 이겨라";		break;
		case EGameType.eTokyoLive:		title = "도쿄라이브";			break;
		case EGameType.eBrainSurvival:		title = "브레인서바이벌";	break;
		case EGameType.eBoardGame:		title = "이거실화?보드게임";	break;
		}
		return title;
	}

	public static string GameName(EGameType eType, int game, EDiscount eDis)
	{
		string name = "";
		switch (eType) {
		case EGameType.eWinWaiter:
			if (eDis == EDiscount.e500won) {
				switch ((EWaiterEasyGame)game) {
				case EWaiterEasyGame.eRockPaperScissors:	name = "가위바위보";	break;
				case EWaiterEasyGame.eSniffling:			name = "홀짝";			break;
				case EWaiterEasyGame.eFrontBack:			name = "앞뒤";			break;
				case EWaiterEasyGame.eLieDetector:			name = "거짓말탐지기";	break;
				}
			} else if (eDis == EDiscount.e1000won) {
				switch ((EWaiterHardGame)game) {
				case EWaiterHardGame.e369:				name = "369";					break;
				case EWaiterHardGame.eSpeakingWords:	name = "탕수육";				break;
				case EWaiterHardGame.Chopsticks:		name = "젓가락 뽑기";			break;
				case EWaiterHardGame.Dice:				name = "주사위 합이 10이상";	break;
				case EWaiterHardGame.TraditionalPlay:	name = "모&윷을 뽑아라";		break;
				}
			}
			break;
		case EGameType.eBoardGame:
			switch ((EBoardGame)game) {
			case EBoardGame.PunchKing:			name = "펀치킹";				break;
			case EBoardGame.HammerKing:			name = "해머킹";				break;
			case EBoardGame.CrocodileRoulette:	name = "악어룰렛";				break;
			case EBoardGame.TurnPlate:			name = "돌려 돌려 돌림판!";		break;
			case EBoardGame.RussianRoulette:	name = "러시안 룰렛";			break;
			}
			break;
		}
		return name;
	}

    public static int GetDiscountPrice(short type) { return GetDiscountPrice((EDiscount)type); }
    public static int GetDiscountPrice(EDiscount type)
    {
        int discountPrice = 0;
        switch(type)
        {
            case EDiscount.e500won:discountPrice =      500;    break;
            case EDiscount.e1000won:discountPrice =     1000;   break;
        }

        return discountPrice;
    }
}
