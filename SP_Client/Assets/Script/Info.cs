using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public partial class Info : MonoBehaviour {

	const int COUPON_MAX_COUNT = 2;
	const int COUPON_REMAIN_MIN = 20;
	const int TOKYOLIVE_MAX_COUNT = 3;
	static int[] TOKYOLIVE_START_TIME = { 41, 30 };

    public static byte TableNum = 0;
    public static byte PersonCnt = 0;
    public static ECustomerType ECustomer = ECustomerType.MAN;

	// Game Info
	public static byte GamePlayCnt = 0;
	public static short GameDiscountWon = -1;	   

	// Coupon Info
    public static int couponCnt = 0;
    public static float loopCouponRemainTime = 0f;
    public static bool waitCoupon = false;
    public static bool mainWaitCoupon = false;

	// TokyoLive Info
	public static int tokyoLiveCnt = 0;

    public static bool RunInGameScene = false;

	public static void AnimateChangeObj(CanvasGroup cur, CanvasGroup next, UnityEvent nextCallback = null)
    {
        UITweenAlpha.Start(cur.gameObject, 0f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));
		UITween uiNext = UITweenAlpha.Start (next.gameObject, 1f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
		if (nextCallback != null)
			uiNext.onCompleteFunc = nextCallback;

        cur.blocksRaycasts = false;
        next.blocksRaycasts = true;
    }

	public static string MakeMoneyString(int price)
	{
		return "￦ " + price.ToString ("N0");
	}

	public static bool isCheckScene(string scene)
	{
		return SceneManager.GetActiveScene ().name == scene;
	}

	public static int idRobot = -1;
    public static List<int> listGameCnt_Robot = new List<int>();
    public static List<int> listOrderCnt_Robot = new List<int>();

    public static void UpdateCouponRemainTime()
    {
        if (couponCnt >= COUPON_MAX_COUNT)
            return;
        
        if (waitCoupon)
        {
            if (mainWaitCoupon && Info.isCheckScene("Main"))
            {
                mainWaitCoupon = false;
                NetworkManager.Instance.Coupon_REQ();
            }

            return;
        }
        
        loopCouponRemainTime += Time.deltaTime;
        if (COUPON_REMAIN_MIN <= Mathf.FloorToInt((Info.loopCouponRemainTime) / 60))
        {
            waitCoupon = true;

            if (Info.isCheckScene("Main"))
                NetworkManager.Instance.Coupon_REQ();
            else
                mainWaitCoupon = true;
        }
    }

	public static bool IsInputFieldFocused()
	{
		GameObject objSelected = EventSystem.current.currentSelectedGameObject;
		return objSelected != null && objSelected.GetComponent<InputField> () != null;
	}

	public static void UpdateTokyoLiveTime()
	{
		if (tokyoLiveCnt >= TOKYOLIVE_MAX_COUNT)
			return;

		_CheckTokyoLivePrev_1Min ();
	}

	static void _CheckTokyoLivePrev_1Min()
	{
		int min = DateTime.Now.Minute;
		int sec = DateTime.Now.Second;

		int prev_1Min = min + 1;
		Debug.Log ("prev_1Min: " + prev_1Min);
		if (sec == 0 &&
		    (prev_1Min == _GetTokyoLivePrevTime (TOKYOLIVE_START_TIME [0]) ||
		    prev_1Min == _GetTokyoLivePrevTime (TOKYOLIVE_START_TIME [1]))) {
			GameObject obj = UIManager.Instance.Show (eUI.eTokyoLive);
			PageTokyoLive page = obj.GetComponent<PageTokyoLive> ();
			if (page)
				page.PrevSet ();
		}
	}

	static int _GetTokyoLivePrevTime(int startTime)
	{
		return startTime == 0 ? 59 : startTime - 1;
	}

	public static bool IsStartTokyoLiveTime()
	{
		int min = DateTime.Now.Minute;
		int sec = DateTime.Now.Second;
		return sec == 0 && 
			(min == TOKYOLIVE_START_TIME [0] || min == TOKYOLIVE_START_TIME [1]);
	}		
}
