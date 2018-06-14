using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECustomerType : byte
{
    MAN = 0,
    WOMAN,
    COUPLE,
}

public class Info : MonoBehaviour {

	public static void AnimateChangeObj(CanvasGroup cur, CanvasGroup next, UnityEvent nextCallback = null)
    {
        UITweenAlpha.Start(cur.gameObject, 0f, TWParam.New(1f).Curve(TWCurve.CurveLevel2));
		UITween uiNext = UITweenAlpha.Start (next.gameObject, 1f, TWParam.New (1f, 1f).Curve (TWCurve.CurveLevel2));
		if (nextCallback != null)
			uiNext.onCompleteFunc = nextCallback;

        cur.blocksRaycasts = false;
        next.blocksRaycasts = true;
    }
}
