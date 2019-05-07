using UnityEngine;
using System.Collections;

public class UITweenPosY : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null)	rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, GetVal(ratio));
		else					trans.localPosition = new Vector3(trans.localPosition.x, GetVal(ratio), trans.localPosition.z);
	}

	protected override float GetValFromObject ()
	{
		if (rectTrans != null)	return rectTrans.anchoredPosition.y;
		else					return trans.localPosition.y;
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosY>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosY>(obj, end, param);
	}
}
