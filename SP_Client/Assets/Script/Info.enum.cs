using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECustomerType : byte
{
	MAN = 0,
	WOMAN,
	COUPLE,
}

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

public partial class Info : MonoBehaviour {

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
}
