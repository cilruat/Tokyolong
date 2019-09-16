using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenScale : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (trans != null)
			trans.localScale = Vector3.one * GetVal(ratio);
	}

	protected override float GetValFromObject ()
	{
		return trans.localScale.x;
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenScale>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenScale>(obj, end, param);
	}
}