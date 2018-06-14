using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenSizeX : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null)
			rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetVal(ratio));
	}

	protected override float GetValFromObject ()
	{
		if (rectTrans != null)
			return rectTrans.rect.width;
		return base.GetValFromObject();
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenSizeX>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenSizeX>(obj, end, param);
	}
}