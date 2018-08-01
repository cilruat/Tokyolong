using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageOrder : PageBase {

	[System.Serializable]
	public class MenuGroup
	{
		public EMenuType eType;
		public GameObject obj;
		public Toggle tg;
	}

	public CanvasGroup[] cgBoards;
	public MenuGroup[] menuGroup;
	public Bill bill;

	EMenuType eCurMenu = EMenuType.eNone;

	protected override void Awake ()
	{
		base.boards = this.cgBoards;
		base.Awake ();

		for (int i = 0; i < menuGroup.Length; i++)
			menuGroup [i].obj.SetActive (false);

        if (MenuData.loaded == false)
            MenuData.Load();
	}

	public void OnClickMenu(int idx)
	{
		OnNext ();
		OnTabChange (idx);
	}

	public void OnClickDetailMenu(int idx)
	{
		EMenuDetail eType = (EMenuDetail)idx;
		bill.SetMenu (eType);
	}

	public void OnTabChange(int idx)
	{
		EMenuType eSelect = (EMenuType)idx;
		if (eSelect == EMenuType.eNone)		return;
		if (eCurMenu == eSelect)			return;

		for (int i = 0; i < menuGroup.Length; i++) {
			if (menuGroup [i].eType == eCurMenu)
				menuGroup [i].obj.SetActive (false);
			if (menuGroup [i].eType == eSelect) {
				menuGroup [i].obj.SetActive (true);
				menuGroup [i].tg.isOn = true;
			}
		}

		eCurMenu = eSelect;
	}

	public void OnPrevBoard()
	{
		for (int i = 0; i < menuGroup.Length; i++)
			menuGroup [i].tg.isOn = false;

		base.OnPrev ();
	}

    public void OnBillConfrim()
    {
        NetworkManager.Instance.Order_Detail_REQ();
    }
}