﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMain : PageBase {

	enum EMenu : byte
	{
		eChat = 0,
		eOrder,
		eGame,
		eService,
		eShowChat,
		eCall = 5,
		eBill,
		eTableSet,
	}

    public CanvasGroup[] cgBoard;

	protected override void Awake ()
	{
        base.boards = cgBoard;
		base.Awake ();
	}

	public void OnClickMenu(int idx)
	{
		EMenu e = (EMenu)idx;
		switch (e) {
            case EMenu.eChat:       SceneChanger.LoadScene("TableStatus", curBoardObj());   break;
            case EMenu.eOrder:      SceneChanger.LoadScene ("Order", curBoardObj());        break;
            case EMenu.eGame:		SceneChanger.LoadScene ("Game", curBoardObj());         break;
            case EMenu.eService:	SceneChanger.LoadScene ("Service", curBoardObj());		break;
            case EMenu.eShowChat:   UIManager.Instance.Show(eUI.eChat);                     break;
		    case EMenu.eCall:		NetworkManager.Instance.WaiterCall_REQ ();  		    break;
            case EMenu.eBill:       NetworkManager.Instance.Order_Detail_REQ(); 		    break;
		    case EMenu.eTableSet:   UIManager.Instance.Show (eUI.eTableSetting);		    break;
		}
	}		
}
