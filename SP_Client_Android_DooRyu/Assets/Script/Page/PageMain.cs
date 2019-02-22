﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageMain : PageBase {

	enum EMenu : byte
	{
		eChat = 0,
		eOrder,
		eGame,
		eService,
		eShowChat,
		eNotice = 5,
		eBill,
		eTableSet,
        eRequestMusic,
        eHowToUse,
		eRoulette = 10,
		eTaro,
		ePractice,
		ePvP,
	}

    public CanvasGroup[] cgBoard;
	public Text txtPlayCnt;
    public Text txtTableNo;

    public GameObject objDiscountChance;
    public GameObject objFireCracker;

    public FlyChance flyChance;

	protected override void Awake ()
	{
        base.boards = cgBoard;
		base.Awake ();

		txtPlayCnt.text = Info.GamePlayCnt.ToString ();
        txtTableNo.text = Info.TableNum.ToString();

		Info.practiceGame = false;
	}

    void Start()
    {
        StartFlyChance();
    }

    public void RefreshGamePlay()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString ();
    }

	public void OnClickMenu(int idx)
	{
        if (flyRoutine != null)
            return;

		EMenu e = (EMenu)idx;
		switch (e) {
        case EMenu.eChat:       	SceneChanger.LoadScene("TableStatus", curBoardObj());   	break;
        case EMenu.eOrder:      	SceneChanger.LoadScene ("Order", curBoardObj());        	break;
        case EMenu.eGame:			SceneChanger.LoadScene ("Game", curBoardObj());         	break;
        case EMenu.eService:		SceneChanger.LoadScene ("Service", curBoardObj());			break;
        case EMenu.eShowChat:   	UIManager.Instance.Show(eUI.eChat);                     	break;
		case EMenu.eNotice:			SceneChanger.LoadScene ("Notice", curBoardObj());  			break;
        case EMenu.eBill:       	NetworkManager.Instance.Order_Detail_REQ(); 		    	break;
	    case EMenu.eTableSet:   	UIManager.Instance.Show (eUI.eTableSetting);		    	break;
        case EMenu.eRequestMusic: 	NetworkManager.Instance.Request_Music_List_REQ();     		break;
        case EMenu.eHowToUse:   	UIManager.Instance.Show(eUI.eHowToUse);                 	break;
		case EMenu.eRoulette:		SystemMessage.Instance.Add ("추후 업데이트 예정이예요~");	break;
		case EMenu.eTaro:			SceneChanger.LoadScene("Taro", curBoardObj());				break;
		case EMenu.ePractice:		SceneChanger.LoadScene("PracticeGame", curBoardObj());   	break;
		case EMenu.ePvP:			SystemMessage.Instance.Add ("광장코아 오픈 후 업데이트 예정이예요~");	break;
		}
	}

    public void RefreshGamePlayChance()
    {
		if (Info.isCheckScene("Main") == false)
            return;

        txtPlayCnt.text = Info.GamePlayCnt.ToString ();

        GameObject newObj = Instantiate(objFireCracker, objDiscountChance.transform) as GameObject;
        newObj.gameObject.SetActive(true);

        txtPlayCnt.text = Info.GamePlayCnt.ToString ();
    }

    Coroutine flyRoutine = null;
    public void StartFlyChance()
    {
        if (Info.orderCnt <= 0)
            return;

        flyRoutine = StartCoroutine(_CreateFlyChance());
    }

    IEnumerator _CreateFlyChance()
    {
        for (int i = Info.orderCnt; i > 0; i--)
        {
            GameObject objChance = Instantiate(flyChance.gameObject, flyChance.transform.parent) as GameObject;
            objChance.gameObject.SetActive(true);

            Info.AddOrderCount(-1);
            yield return new WaitForSeconds(0.1f);
        }

        flyRoutine = null;
    }
}
