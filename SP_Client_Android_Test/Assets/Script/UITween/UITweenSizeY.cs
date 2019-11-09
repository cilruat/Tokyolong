using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenSizeY : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null)
			rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetVal(ratio));
	}

	protected override float GetValFromObject ()
	{
		if (rectTrans != null)
			return rectTrans.rect.height;
		return base.GetValFromObject();
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenSizeY>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenSizeY>(obj, end, param);
	}
}