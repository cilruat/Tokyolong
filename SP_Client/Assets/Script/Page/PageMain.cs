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

	protected override void Awake ()
	{
		base.Awake ();
	}

	public void OnClickMenu(int idx)
	{
		EMenu e = (EMenu)idx;

		if (e == EMenu.eOrder)
			SceneChanger.LoadScene ("Order", gameObject);

		switch (e) {
		case EMenu.eChat:			
			break;
		case EMenu.eOrder:			
			SceneChanger.LoadScene ("Order", gameObject);
			break;
		case EMenu.eGame:			
			break;
		case EMenu.eService:		
			SceneChanger.LoadScene ("Service", gameObject);
			break;
		case EMenu.eShowChat:			
			break;
		case EMenu.eCall:			
			NetworkManager.Instance.WaiterCall_REQ ();
			break;
		case EMenu.eBill:			
			break;
		case EMenu.eTableSet:			
			break;
		}
	}		
}
