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
	}
}
