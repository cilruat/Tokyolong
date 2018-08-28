using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public partial class Info : MonoBehaviour {

    public static byte TableNum = 0;
    public static byte PersonCnt = 0;
    public static ECustomerType ECustomer = ECustomerType.MAN;

	// Game Info
	public static byte GamePlayCnt = 0;
	public static short GameDiscountWon = -1;

    const int COUPON_MAX_COUNT = 2;
    const int COUPON_REMAIN_MIN = 20;

    public static int couponCnt = 0;
    public static float loopCouponRemainTime = 0f;
    public static bool waitCoupon = false;
    public static bool mainWaitCoupon = false;

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
}
