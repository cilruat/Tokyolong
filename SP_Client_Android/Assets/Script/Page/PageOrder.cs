using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public MenuTypeGroup[] menuTypeGroup; // OderMain
	public MenuGroup[] menuGroup;

    public GameObject BillPanel;
	public Bill bill;


    EMenuType eCurMenu = EMenuType.eNone;

    int param = -1;

    Coroutine showRoutine = null;

	protected override void Awake ()
	{
        if (MenuData.loaded == false)
            MenuData.Load();

		base.boards = this.cgBoards;
		base.Awake ();
        base.acFinalIdx = 0;
//        base.acFinal = ShowMenu;

		for (int i = 0; i < menuGroup.Length; i++)
			menuGroup [i].obj.SetActive (false);
	}

    void Start()
    {
//        showRoutine = StartCoroutine(ShowMenu(true));
		OnTabChange (3);
        BillPanel.SetActive(false);
    }

    void ShowMenu()
    {
        for (int i = 0; i < menuTypeGroup.Length; i++)
            menuTypeGroup[i].Show();
    }

    IEnumerator ShowMenu(bool show)
    {
        for (int i = 0; i < menuTypeGroup.Length; i++)
        {
            if(show)
                menuTypeGroup[i].Show();
            else
                menuTypeGroup[i].Hide();
        }

        if (show)
        {
            showRoutine = null;
            yield break;
        }

        bool isShow = true;
        while (isShow)
        {
            for (int i = 0; i < menuTypeGroup.Length; i++)
            {
                isShow = menuTypeGroup[i].show;
                if (isShow)
                    break;
            }

            yield return null;
        }

        showRoutine = null;
        if (show == false)
            _Next(param);
    }

	public void OnClickMenu(int idx)
	{
        if (param == idx)
            return;

        param = idx;

        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowMenu(false));
    }

    void _Next(int idx)
    {
        OnNext ();
        OnTabChange (idx);
        param = -1;
    }

	public void OnClickDetailMenu(int idx)
	{
        EMenuDetail eType = (EMenuDetail)idx;
		bill.SetMenu (eType);
        BillPanel.SetActive(true);

    }

    public void DeleteDetailMenu(int idx)
    {
        EMenuDetail eType = (EMenuDetail)idx;
        bill.RemoveElt(eType);
        BillPanel.SetActive(false);

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

    public void OnBillShow()
    {
        BillPanel.SetActive(true);
    }

    public void CloseBillPanel()
    {
        BillPanel.SetActive(false);
    }


}