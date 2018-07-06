using System.Collections;
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
			SceneChanger.LoadScene ("TableStatus", gameObject);
			break;
		case EMenu.eOrder:			
			SceneChanger.LoadScene ("Order", gameObject);
			break;
		case EMenu.eGame:		
            SceneChanger.LoadScene ("Game", gameObject);
			break;
		case EMenu.eService:		
			SceneChanger.LoadScene ("Service", gameObject);
			break;
		case EMenu.eShowChat:
			UIManager.Instance.Show (eUI.eChat);
			break;
		case EMenu.eCall:			
			NetworkManager.Instance.WaiterCall_REQ ();
			break;
        case EMenu.eBill:
            NetworkManager.Instance.Order_Detail_REQ();
			break;
		case EMenu.eTableSet:
			UIManager.Instance.Show (eUI.eTableSetting);
			break;
		}
	}		
}
