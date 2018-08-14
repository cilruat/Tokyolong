using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public partial class Info : MonoBehaviour {

    public static byte TableNum = 0;
    public static byte PersonCnt = 0;
    public static ECustomerType ECustomer = ECustomerType.MAN;

	// Game Info
	public static byte GamePlayCnt = 0;
	public static short GameDiscountWon = -1;

    const int COUPON_MAX_COUNT = 3;
    const int COUPON_REMAIN_MIN = 20;

    public static int couponCnt = 0;
    public static float loopCouponRemainTime = 0f;
    public static bool waitCoupon = false;

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
	public static List<byte> listGamePlayCnt_Robot = new List<byte>();

    public static void UpdateCouponRemainTime()
    {
        if (couponCnt >= COUPON_MAX_COUNT)
            return;

        if (waitCoupon)
            return;
        
        loopCouponRemainTime += Time.deltaTime;
        if (COUPON_REMAIN_MIN <= Mathf.FloorToInt((Info.loopCouponRemainTime) / 60))
        {
            NetworkManager.Instance.Coupon_REQ();
            waitCoupon = true;
        }
    }
}
