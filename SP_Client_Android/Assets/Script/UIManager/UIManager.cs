using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BugSplat;

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
	eSurpriseStart,		// Not Used
	eSurpriseResult,	// Not Used
	eTokyoLive		= 10,
	eDiscountAni,
	eFirstOrderDesc,
	eOwnerGame,
	eOwnerQuiz,
	eOwnerTrick,
	eTokyoQuiz,

	eNone = 100,
}

public class UIManager : SingletonMonobehaviour<UIManager> {

	static UIManager single = null;

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
    public ClickStar clickStar;
	public ClickStar clickHollWeen;

	public AudioClip clipTokyoLive;
	public AudioClip clipSurprise;
	public AudioClip clipCelebration;
	public AudioClip clipMagnificent;
	public AudioClip clipOwnerGame;
	public AudioClip clipOwnerQuiz;
	public AudioClip clipOwnerTrick;

	public AudioSource audioSound;
	public AudioSource audioBell;
	public AudioSource audioMusic;

	public Reporter reporter;

    [System.NonSerialized]public bool isMouseClickEff = false;

	eUI curUI = eUI.eNone;
	Dictionary<eUI, GameObject> dicObject = new Dictionary<eUI, GameObject> ();

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

		reporter.Initialize (gameObject);
		reporter.SetCallback ((success, message) => {
			Debug.Log("BugSplat Report Posted: " + success + ", BugSplat API Response: " + message);
		});

		reporter.prompt = false;

        Application.targetFrameRate = 60;

		Info.RunInGameScene = Info.isCheckScene ("Game") || Info.isCheckScene ("PracticeGame");

		if (PageBase.Instance != null )
			isMouseClickEff = true;

        DontDestroyOnLoad(this);
	}

	void collect()
	{
		for (int i = 0; i < listUI.Count; i++) {
			if (listUI [i].obj != null)
				dicObject.Add (listUI [i].ui, listUI [i].obj);
		}
	}		

	public void SetCamera()
	{
		if (canvas == null)
			return;

		if (canvas.worldCamera == null)
			canvas.worldCamera = Camera.main;
	}

    public void Show(int pageIdx) { Show((eUI)pageIdx); }
	public GameObject Show(eUI page)
	{
		switch (page) {
		case eUI.eWaiting:		
			elapsedTime = 0f;	
			waiting = true;		
			break;
		case eUI.eShowLog:		break;
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
		switch (page) {
		case eUI.eWaiting:		
			elapsedTime = 0f;	
			waiting = false;	
			break;
		case eUI.eShowLog:		
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
		foreach (KeyValuePair<eUI, GameObject> pair in dicObject) {
			if (pair.Value != null)
				pair.Value.SetActive (false);
		}
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

	public void ShowChatAlarm()     {   uiAlarm.ShowAlarmChat(_ShowChat);  }
	public void ShowOrderAlarm()    {   uiAlarm.ShowAlarm(EAlarmType.eMessage,"주문이\n접수 됐다냥~", _ShowBillDetail);    }
	public void ShowDiscountAlarm() {   uiAlarm.ShowAlarm(EAlarmType.eMessage,"할인이\n접수 됐다냥~", _ShowBillDetail);    }

    void _ShowChat()
    {
        uiAlarm.HideAlarmChat();

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
            
		if (Input.GetKey (KeyCode.LeftShift)) {
			
			if (Input.GetKeyDown (KeyCode.L))
				UIManager.Instance.ShowLog ();

            #if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.P))
			{
				Info.GamePlayCnt += 1;
				if (Info.isCheckScene ("Game"))
					((PageGame)PageBase.Instance).RefreshPlayCnt ();
			}				

			if (Input.GetKeyDown (KeyCode.Z))
				PlayMusic (clipTokyoLive, 3f);

			if (Input.GetKeyDown (KeyCode.X))
				MuteMusic ();

			if (Input.GetKeyDown (KeyCode.C))
				UIManager.Instance.Show(eUI.eOwnerGame);

			if(Input.GetKeyDown(KeyCode.T))
			{
				GameObject obj = Show (eUI.eTokyoLive);
				PageTokyoLive ui = obj.GetComponent<PageTokyoLive>();
				ui.PrevSet();
			}
            #endif
		}

		if (Input.GetMouseButtonDown (0)) {
			if (isMouseClickEff == false)
				return;

			if (Info.CheckGameScene (SceneManager.GetActiveScene ().name))
				return;

			GameObject showEff = clickStar.gameObject;
            GameObject objEff = null;
			if (Info.isCheckScene ("Admin"))
				objEff = Instantiate (showEff, PageAdmin.Instance.transform) as GameObject;
			else {
				objEff = Instantiate (showEff, PageBase.Instance.transform) as GameObject;
				PlaySound ();
			}

            ClickStar click = objEff.GetComponent<ClickStar>();
			click.ShowClickStar(Input.mousePosition);
		}

		if (Info.CheckOwnerEvt) {
			if (NetworkManager.isSending)
				return;

			if (SceneManager.GetActiveScene ().name == "Login")
				return;

			if (IsActive (Info.OwnerUI))
				return;

			if (Info.CheckGameScene (SceneManager.GetActiveScene ().name))
				return;

			GameObject objUI = UIManager.Instance.Show (Info.OwnerUI);
			OwnerEvent evt = objUI.GetComponent<OwnerEvent> ();
			evt.Show ();

			Info.CheckOwnerEvt = false;
			Info.OwnerUI = eUI.eOwnerGame;
		}
    }

	public void PlaySound()
	{
		if (audioSound == null)
			return;

		audioSound.Play ();
	}

	public void PlayBell()
	{
		if (audioBell == null)
			return;

		audioBell.Play ();
	}

	public void PlayMusic(AudioClip clip, float volumeScale = 1f)
	{
		if (audioMusic == null)
			return;

		audioMusic.volume = 1f;
		audioMusic.clip = clip;
		audioMusic.Play ();
		//audioMusic.PlayOneShot(clip, volumeScale);
	}

	public void StopMusic()
	{
		if (audioMusic == null)
			return;

		audioMusic.Stop ();
	}

	const float MUTE_FADE_IN = .5f;
	public void MuteMusic()
	{
		StartCoroutine (_MuteMusic ());
	}

	IEnumerator _MuteMusic()
	{
		audioMusic.volume = 1;

		float duration = MUTE_FADE_IN;
		while (duration > 0) {
			duration = Mathf.Max (0, duration - Time.unscaledDeltaTime);
			float rate = duration / MUTE_FADE_IN;
			audioMusic.volume = rate;
			yield return null;
		}

		audioMusic.volume = 0;
		audioMusic.Stop ();
	}

	public void QuitPos()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#endif
		Application.Quit ();
	}
}
