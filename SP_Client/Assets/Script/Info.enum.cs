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
	eTop = 0,		    // 탑메뉴
	eMeal,			    // 밥&안주 식사
	ePochakaya,			// 포차카야
	eIzakaya,			// 이자카야
	eWomanTarget,    	// 여심저격
	eSashimiNFruit = 5, // 회,과일
    eSoup,              // 탕,전골
	eSozu,              // 소주
	eBear,              // 맥주
    eSake,              // 사케
    eFruitSozu = 10,    // 과일 소주
    eFruitMakgeolli,    // 과일 막걸리
    eGin,               // 고급진
    eDrink,             // 음료&아이스크림
}

public enum EMenuDetail : int
{
    eMenu1		= 1,
	eMenu2      = 2,
	eMenu3      = 3,
	eMenu4      = 4,
	eMenu5      = 5,
	eMenu6      = 6,
	eMenu7      = 7,
	eMenu8      = 8,
	eMenu9      = 9,
	eMenu10     = 10,
	eMenu11     = 11,
	eMenu12     = 12,
	eMenu13     = 13,
	eMenu14     = 14,
	eMenu15     = 15,
	eMenu16     = 16,
	eMenu17     = 17,
	eMenu18     = 18,
	eMenu19     = 19,
	eMenu20     = 20,
	eMenu21     = 21,
	eMenu22     = 22,
	eMenu23     = 23,
	eMenu24     = 24,
	eMenu25     = 25,
	eMenu26     = 26,
	eMenu27     = 27,
	eMenu28     = 28,
	eMenu29     = 29,
	eMenu30     = 30,
	eMenu31     = 31,
	eMenu32     = 32,
	eMenu33  	= 33,
	eMenu34     = 34,
	eMenu35     = 35,
	eMenu36     = 36,
    eNone       = 37,
    eChamSozu               = 38,
    eChamiseul              = 39,
    eCheongha               = 40,
    eMaehwasu               = 41,
    eBogbadeunBuladeo       = 42,
    eSimsul                 = 43,
    eBingtanbog             = 44,
    eHiteBeer               = 45,
    eSangBeer500cc          = 46,
    eSangBeer1700cc         = 47,
    eSangBeer3000cc         = 48,
    eHakutsuruMaru          = 49,
    eGanbareOttosang        = 50,
    eJunmai                 = 51,
    eMaruJunmai             = 52,
    eStrawberrySozu         = 53,
    eMangoSozu              = 54,
    eBlueberrySozu          = 55,
    ePeachSozu              = 56,
    eWhiteGrapeSozu         = 57,
    eStrawberryMakgeolli    = 58,
    eMangoMakgeolli         = 59,
    eBlueberryMakgeolli     = 60,
    eBambaBambaMakgeolli    = 61,
    ePeachMakgeolli         = 62,
    eWhiteGrapeMakgeolli    = 63,
    eMilkMakgeolli          = 64,
    eYogurtMakgeolli        = 65,
    eJajagpogtanZombieZu	= 66,
    eJackDanielHoney        = 67,
    eCoke                   = 68,
    eCider                  = 69,
    eFanta                  = 70,
    eHotSix                 = 71,
    eChocoEmong             = 72,
    eStrawberryEmong        = 73,
	ePear					= 74,
	eHungaeSalmonSalad		= 75,
	eBeefTadakki			= 76,
	eBagirakSoup			= 77,
	eMeal					= 78,
	eSimsul12				= 79,
	ePigBar					= 80,
	eWaterMelonBar			= 81,
	eMelonBar				= 82,
	eScrewBar				= 83,
}

/// <summary>
/// game
/// </summary>
public enum EDiscount
{
	e500won = 0,
	e1000won,
	e2000won,
	e5000won,
    eAll,
    eDirect,
}

public enum EGameType
{
	eWinWaiter,
	ePuzzleGame,
    eTabletGame
}

public enum EWinWaiter
{
	eRockPaperScissors,
    PunchKing,
    HammerKing,
	CrocodileRoullet,
	RussianRoullet
}

public enum EPuzzleGame
{
	ePicturePuzzle,
	ePairCards,
	eTouchNumber,
	eFindDiffPicture,
}

public enum ETabletGame
{
	CrashCat = 0,
	FlappyBird,
    /*SlidingDown,
    DownHill, 
    RingDingDong,
	EggMon = 5,
	Hammer,
	TwoCars,
	Bridges,
	CrashRacing*/
}

public partial class Info : MonoBehaviour {

	public static string MenuTitle(EMenuType eType)
	{		
        string title = "";
		switch (eType) 
        {
            case EMenuType.eTop:            title = "탑";          	break;
            case EMenuType.eMeal:           title = "밥&사리";     	break;
            case EMenuType.ePochakaya:      title = "포차카야";     break;
            case EMenuType.eIzakaya:        title = "이자카야";     break;
            case EMenuType.eWomanTarget:    title = "여심저격";     break;
            case EMenuType.eSashimiNFruit:  title = "회&과일";      break;
            case EMenuType.eSoup:           title = "탕&나베";      break;
            case EMenuType.eSozu:           title = "소주";         break;
            case EMenuType.eBear:           title = "맥주";         break;
            case EMenuType.eSake:           title = "사케";         break;
            case EMenuType.eFruitSozu:      title = "수제 과일 소주";   	break;
            case EMenuType.eFruitMakgeolli: title = "수제 과일 막걸리타";	break;
            case EMenuType.eGin:            title = "고급진";        		break;
			case EMenuType.eDrink:          title = "아이스크림&음료";      break;
        }

        return title;
	}

    public static int GetDiscountPrice(short type) { return GetDiscountPrice((EDiscount)type); }
    public static int GetDiscountPrice(EDiscount type)
    {
        int discountPrice = 0;
		switch (type) {
		case EDiscount.e500won:		discountPrice = 500;	break;
		case EDiscount.e1000won:	discountPrice = 1000;	break;
		case EDiscount.e2000won:	discountPrice = 2000;	break;
		case EDiscount.e5000won:	discountPrice = 5000;	break;
		}

        return discountPrice;
    }

    public static string GameTitle(EGameType eType) { return ""; }
    public static string GameName(EGameType eType, int game, EDiscount eDis) { return ""; }
}
