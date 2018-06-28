using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageService : PageBase {

	public CanvasGroup[] cgBoards;

	protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();
	}

	public void OnBillDetail()
	{
		UIManager.Instance.Show (eUI.eBillDetail);
	}		

	public void OnMusicRequest()
	{
		UIManager.Instance.Show (eUI.eMusicRequest);
	}
}
