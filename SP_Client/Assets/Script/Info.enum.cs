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
	ePochakaya,			// 튀김
	eIzakaya,			// 탕
	eWomanTarget,    	// 스페셜
	eSashimiNFruit,     // 식사
    eSoup,              // 탕&전골
	eSozu,              // 소주
	eBear,              // 맥주
    eSake,              // 사케
    eFruitSozu,         // 과일 소주
    eFruitMakgeolli,    // 과일 막걸리
    eGin,                // 고급진
    eDrink,             // 음료
}

public enum EMenuDetail : int
{
    eOyakkokkioTeriGyudon           = 1,
    eStakeCubeGyudon                = 2,
    eGarlicShrimpGyudon             = 3,
    eKoiGyudon                      = 4,
    eChadolbagiGyudon               = 5,
    eDaewangEdbikatsu               = 6,
    eAnsimLoskatsu                  = 7,
    eSausageVegetableGratin         = 8,
    eButterCoconutShrimp            = 9,
    eModeumFrenchFries              = 10,
    eCornCheeseBaconBaPotato        = 11,
    eHawaiianCheeseRollKatus        = 12,
    eBeefTartare                    = 13,
    eOkonomiyaki                    = 14,
    eTokyoEbiFry                    = 15,
    eTakoyaki                       = 16,
    eKKanpungYug                    = 17,
    eOkonomiWangEggRoll             = 18,
    eYulingi                        = 19,
    eYasaiYakiSamgyeob              = 20,
    eModeumGaraAge                  = 21,
    eModeumGoloke                   = 22,
    eKobeGyuKatsu                   = 23,
    eSpicyBaconCreamShrimp          = 24,
    eCreamSpicyChicken              = 25,
    eSpicySamgyeobSugjuBokkeum      = 26,
    eChasyuBossam                   = 27,
    eTakoWasabiNSoraWasabi          = 28,
    eTakoChoSashimiSalad            = 29,
    eMilkBingsuNIceHwangdo          = 30,
    eModeumFruithwachae             = 31,
    eBeefTartareNWaterSashimi       = 32,
    eShanghaiSeafoodSpicyJjamppong  = 33,
    eMilleFeuilleChadolNabe         = 34,
    eMilleFeuillePorkKimchiNabe     = 35,
    eNagasakiSeafoodJjamppong       = 36,
    eGamabokoOdeng                  = 37,
    eChamSozu                       = 38,
    eChamiseul                      = 39,
    eCheongha                       = 40,
    eMaehwasu                       = 41,
    eBogbadeunBuladeo               = 42,
    eSimsul                         = 43,
    eBingtanbog                     = 44,
    eHiteBeer                       = 45,
    eSangBeer500cc                  = 46,
    eSangBeer1700cc                 = 47,
    eSangBeer3000cc                 = 48,
    eHakutsuruMaru                  = 49,
    eGanbareOttosang                = 50,
    eJunmai                         = 51,
    eMaruJunmai                     = 52,
    eStrawberrySozu                 = 53,
    eMangoSozu                      = 54,
    eBlueberrySozu                  = 55,
    ePeachSozu                      = 56,
    eWhiteGrapeSozu                 = 57,
    eStrawberryMakgeolli            = 58,
    eMangoMakgeolli                 = 59,
    eBlueberryMakgeolli             = 60,
    eBambaBambaMakgeolli            = 61,
    ePeachMakgeolli                 = 62,
    eWhiteGrapeMakgeolli            = 63,
    eMilkMakgeolli                  = 64,
    eYogurtMakgeolli                = 65,
    eJajagpogtanZombieZu            = 66,
    eJackDanielHoney                = 67,
    eCoke                           = 68,
    eCider                          = 69,
    eFanta                          = 70,
    eHotSix                         = 71,
    eChocoEmong                     = 72,
    eStrawberryEmong                = 73,
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

	public static string MenuTitle(EMenuType eType)
	{		
        string title = "";
		switch (eType) 
        {
            case EMenuType.eTop:            title = "탑";          break;
            case EMenuType.eMeal:           title = "밥&안주";     break;
            case EMenuType.ePochakaya:      title = "포차카야";     break;
            case EMenuType.eIzakaya:        title = "이자카야";     break;
            case EMenuType.eWomanTarget:    title = "여심저격";     break;
            case EMenuType.eSashimiNFruit:  title = "회&과일";      break;
            case EMenuType.eSoup:           title = "탕&전골";      break;
            case EMenuType.eSozu:           title = "소주";         break;
            case EMenuType.eBear:           title = "맥주";         break;
            case EMenuType.eSake:           title = "사케";         break;
            case EMenuType.eFruitSozu:      title = "생과일 소주";   break;
            case EMenuType.eFruitMakgeolli: title = "생과일 막걸리";  break;
            case EMenuType.eGin:            title = "고급진";        break;
            case EMenuType.eDrink:          title = "음료";          break;
        }

        return title;
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
            case EDiscount.e500won:		discountPrice = 500;    break;
            case EDiscount.e1000won:	discountPrice = 1000;   break;
        }

        return discountPrice;
    }
}
