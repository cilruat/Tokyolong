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
		eCall = 5,
		eBill,
		eTableSet,
        eRequestMusic,
        eHowToUse,
	}

    public CanvasGroup[] cgBoard;
	public Text txtPlayCnt;
    public Text txtTableNo;

    public GameObject objDiscountChance;
    public GameObject objFireCracker;

    public FlyChance flyChance;

	protected override void Awake ()
	{
        base.boards = cgBoard;
		base.Awake ();

		txtPlayCnt.text = Info.GamePlayCnt.ToString ();
        txtTableNo.text = Info.TableNum.ToString();
	}

    void Start()
    {
        CreateFlyChance();
    }

    public void RefreshGamePlay()
    {
        txtPlayCnt.text = Info.GamePlayCnt.ToString ();
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
            case EMenu.eRequestMusic: NetworkManager.Instance.Request_Music_List_REQ();     break;
            case EMenu.eHowToUse:   UIManager.Instance.Show(eUI.eHowToUse);                 break;
		}
	}

    Coroutine scaleRouine = null;
    public void RefreshGamePlayChance()
    {
		if (Info.isCheckScene("Main") == false)
            return;

        GameObject newObj = Instantiate(objFireCracker, objDiscountChance.transform) as GameObject;
        newObj.gameObject.SetActive(true);

        txtPlayCnt.text = Info.GamePlayCnt.ToString ();

        if (scaleRouine != null)
            StopCoroutine(scaleRouine);

        scaleRouine = StartCoroutine(_ScaleAnim());
    }

    IEnumerator _ScaleAnim()
    {
		if (Info.isCheckScene("Main") == false)
            yield break;

        UITween tween = UITweenScale.Start(objDiscountChance, 1f, 1.2f, TWParam.New(.1f).Curve(TWCurve.Back).Speed(TWSpeed.Slower));
        while (tween.IsTweening())
        {
			if (Info.isCheckScene("Main") == false)
				yield break;

            yield return null;
        }

        tween = UITweenScale.Start(objDiscountChance, 1.2f, 1f, TWParam.New(.1f).Curve(TWCurve.Back).Speed(TWSpeed.Faster));
        while (tween.IsTweening())
        {
			if (Info.isCheckScene("Main") == false)
				yield break;
            
            yield return null;
        }

        scaleRouine = null;
    }

    public void CreateFlyChance()
    {
        if (Info.orderCnt <= 0)
            return;
        
        for (int i = Info.orderCnt; i > 0; i--)
        {
            GameObject objChance = Instantiate(flyChance.gameObject, flyChance.transform.parent) as GameObject;
            objChance.gameObject.SetActive(true);

            Info.orderCnt--;
        }
    }
}
