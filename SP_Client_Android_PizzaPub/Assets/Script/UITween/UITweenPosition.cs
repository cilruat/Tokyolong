using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenPosition : UITweenVector3Type
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null && _use_rectTrans)	rectTrans.anchoredPosition = GetVal(ratio);
		else										trans.localPosition = GetVal(ratio);
	}

	protected override Vector3 GetValFromObject ()
	{
		if (rectTrans != null && _use_rectTrans)	return rectTrans.anchoredPosition;
		else										return trans.localPosition;
	}

	public static UITween Start (GameObject obj, Vector3 start, Vector3 end, TWParam param, bool use_rectTrans = true)
	{
		return UITweenVector3Type.Start<UITweenPosition>(obj, start, end, param, use_rectTrans);
	}

	public static UITween Start (GameObject obj, Vector3 end, TWParam param, bool use_rectTrans = true)
	{
		return UITweenVector3Type.Start<UITweenPosition>(obj, end, param, use_rectTrans);
	}
}
