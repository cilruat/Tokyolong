using System.Collections;
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
		eMind,
		eHitNMiss,
		eArcadeGame = 15,
		eSsul,
		eInssa,
		eMail,
        eSelectGame,
        eCashShop = 20,
        eWebView,
    }

    public CanvasGroup[] cgBoard;
	public Text txtPlayCnt;
    public Text txtTableNo;

    public GameObject objDiscountChance;
    public GameObject objFireCracker;

    public FlyChance flyChance;

    public GameObject objSubScroll;

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
        objSubScroll.SetActive(false);
		StartFlyChance ();

        if (Info.firstOrder)
			StartCoroutine (_FirstOrderTokyoLive ());
    }

	IEnumerator _FirstOrderTokyoLive()
	{
		yield return new WaitForSeconds (.5f);

		GameObject obj = UIManager.Instance.Show (eUI.eFirstOrderDesc);
		FirstOrderDesc first = obj.GetComponent<FirstOrderDesc> ();
		first.Show ();
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
        case EMenu.eOrder:      	SceneChanger.LoadScene ("LoadingOrder", curBoardObj());        	break;
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
		case EMenu.eMind:			SceneChanger.LoadScene ("Mind", curBoardObj ());			break;
		case EMenu.eHitNMiss:		SceneChanger.LoadScene ("HitNMiss", curBoardObj ());		break;
		case EMenu.eArcadeGame:		SceneChanger.LoadScene ("ArcadeGame", curBoardObj ());		break;
		case EMenu.eSsul:			SceneChanger.LoadScene ("Ssul", curBoardObj ());			break;
		case EMenu.eInssa:			SceneChanger.LoadScene ("InSsa", curBoardObj ());			break;
		case EMenu.eMail:			SceneChanger.LoadScene ("LoadingChat", curBoardObj ());			break;
        case EMenu.eSelectGame:     SceneChanger.LoadScene("LoadingGame", curBoardObj());        break;
        case EMenu.eCashShop:       SceneChanger.LoadScene("CashShop", curBoardObj());          break;
        case EMenu.eWebView:        SceneChanger.LoadScene("WebView", curBoardObj()); break;

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

    public void ShowSubScroll()
    {
        objSubScroll.SetActive(true);
    }

    public void HideSubScroll()
    {
        objSubScroll.SetActive(false);
    }
}
