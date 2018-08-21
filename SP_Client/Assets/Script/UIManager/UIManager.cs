using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum eUI
{
	eChat           = 0,
	eTableSetting,
	eBillDetail,
	eBillSending,
	eMusicRequest,
	eWaiting        = 5,
    eHowToUse,
    eShowLog,
    eCoupon,

	eNone = 100,
}

public class UIManager : SingletonMonobehaviour<UIManager> {

	[System.Serializable]
	public class UI
	{
		public eUI ui;
		public GameObject obj;
	}

	public GameObject objShadow;
	public List<UI> listUI;

    public UIAlarm uiAlarm;

	public Canvas canvas;
    public ClickStar clickStarA;
    public ClickStar clickStarB;

	bool isMouseClickEff = false;

	eUI curUI = eUI.eNone;
	Dictionary<eUI, GameObject> dicObject = new Dictionary<eUI, GameObject> ();

    static UIManager single = null;

    void Awake () 
	{
        if (single != null)
        {
            Destroy(this.gameObject);
            return;
        }

        single = UIManager.Instance;

		collect();
		Hide_All ();

        VLogSave.Start();

        Application.targetFrameRate = 60;

        Info.RunInGameScene = Info.isCheckScene("Game");

		if (PageBase.Instance != null )
			isMouseClickEff = true;

        DontDestroyOnLoad(this);
	}

	void collect()
	{
		for (int i = 0; i < listUI.Count; i++) 
			dicObject.Add (listUI [i].ui, listUI [i].obj);
	}		

    public void Show(int pageIdx) { Show((eUI)pageIdx); }
	public GameObject Show(eUI page)
	{
        switch (page)
        {
            case eUI.eWaiting:
                elapsedTime = 0f;
                waiting = true;
                break;
            case eUI.eShowLog:
                break;
            case eUI.eCoupon:
                curUI = page;
                break;
            default:
                curUI = page;
                objShadow.SetActive (true);
                break;
        }

		dicObject [page].SetActive (true);
		return dicObject [page];
	}

    public void Hide(int pageIdx) { Hide((eUI)pageIdx); }
	public void Hide(eUI page)
	{
        switch (page)
        {
            case eUI.eWaiting:
                elapsedTime = 0f;
                waiting = false;
                break;
            case eUI.eShowLog:
                break;
            case eUI.eCoupon:
                Info.waitCoupon = false;
                Info.loopCouponRemainTime = 0f;
                break;
            default:
                curUI = eUI.eNone;
                objShadow.SetActive (false);
                break;
        }
		
		dicObject [page].SetActive (false);
	}
		
	public void Hide_All()
	{
		objShadow.SetActive (false);
		foreach (KeyValuePair<eUI, GameObject> pair in dicObject)
			pair.Value.SetActive (false);
	}

	public bool IsActive(eUI page)
	{
		return dicObject [page].activeSelf;
	}

	public GameObject GetUI(eUI page)
	{
		return dicObject [page];
	}

	public GameObject GetCurUI()
	{
		if (curUI == eUI.eNone)
			return null;

		return dicObject [curUI];
	}

	public void ShowChatAlarm()     {   uiAlarm.ShowAlarm(EAlarmType.eMessage,"새로운 메세지가\n도착 했다냥~", _ShowChat);  }
	public void ShowOrderAlarm()    {   uiAlarm.ShowAlarm(EAlarmType.eMessage,"주문이\n접수 됐다냥~", _ShowBillDetail);    }
	public void ShowDiscountAlarm() {   uiAlarm.ShowAlarm(EAlarmType.eMessage,"할인이\n접수 됐다냥~", _ShowBillDetail);    }

    void _ShowChat()
    {
        uiAlarm.HideAlarm();

        if (curUI != eUI.eNone && curUI != eUI.eChat)
            Hide(curUI);

        UIChat uiChat = Show(eUI.eChat).GetComponent<UIChat>();
        uiChat.ShowChatTable();
    }

    void _ShowBillDetail()
    {
        uiAlarm.HideAlarm();

        if (curUI != eUI.eNone && curUI != eUI.eBillDetail)
            Hide(curUI);

        NetworkManager.Instance.Order_Detail_REQ();
    }

    public void ShowLog()
    {
        ShowLog showLog = GetUI(eUI.eShowLog).GetComponent<ShowLog>();
        if (showLog.show == false)
        {
            Show(eUI.eShowLog);
            showLog.Show();
        }
        else
        {
            showLog.Hide();
            Hide_All();
        }
    }
		

    bool waiting = false;
    const float WAIT_DISCONNECT = 10f;
    float elapsedTime = 0f;

	bool showClickA = false;

    void Update()
    {
        if (waiting)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= WAIT_DISCONNECT)
            {
                SystemMessage.Instance.Add("네트워크 문제가 발생 하였습니다");
                NetworkManager.Instance.disconnect();
                Hide(eUI.eWaiting);
                SceneChanger.LoadScene ("Login", PageBase.Instance.curBoardObj ());
            }
        }

		if (Input.GetKeyDown(KeyCode.P))
		{
			Info.GamePlayCnt += 1;
			((PageGame)PageBase.Instance).RefreshPlayCnt();
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			
			if (Input.GetKeyDown (KeyCode.L))
				UIManager.Instance.ShowLog ();

			if (Input.GetKeyDown (KeyCode.C))
				showClickA = !showClickA;
		}			

		if (Input.GetMouseButtonDown (0)) {
			if (isMouseClickEff == false)
				return;

            GameObject showEff = showClickA ? clickStarA.gameObject : clickStarB.gameObject;
            GameObject objEff = null;
            if(Info.isCheckScene("Admin"))
                objEff = Instantiate (showEff, PageAdmin.Instance.transform) as GameObject;
            else
                objEff = Instantiate (showEff, PageBase.Instance.transform) as GameObject;

            ClickStar clickStar = objEff.GetComponent<ClickStar>();
            clickStar.ShowClickStar(Input.mousePosition);
		}

        if(Info.TableNum != (byte)0 && Info.isCheckScene("Login") == false)
            Info.UpdateCouponRemainTime();
    }
}
