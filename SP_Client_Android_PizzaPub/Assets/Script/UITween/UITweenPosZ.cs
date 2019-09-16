using UnityEngine;
using System.Collections;

public class UITweenPosZ : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null)	rectTrans.anchoredPosition3D = new Vector3(rectTrans.anchoredPosition.x, rectTrans.anchoredPosition.y, GetVal(ratio));
		else					trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, GetVal(ratio));
	}

	protected override float GetValFromObject ()
	{
		if (rectTrans != null)	return rectTrans.anchoredPosition3D.z;
		else					return trans.localPosition.z;
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosZ>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosZ>(obj, end, param);
	}
}
