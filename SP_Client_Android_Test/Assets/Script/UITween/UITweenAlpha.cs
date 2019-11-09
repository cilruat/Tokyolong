using UnityEngine;
using System.Collections;

public class UITweenAlpha : UITweenRatioType
{
	protected override void SetVal (float ratio)
	{
		if (canvasGroup != null)
		{
			canvasGroup.alpha = GetVal(ratio);
		}
		else if (text != null)
		{
			Color col = text.color;
			col.a = GetVal(ratio);
			text.color = col;
		}
		else if (graphic != null)
		{
			Color col = graphic.color;
			col.a = GetVal(ratio);
			graphic.color = col;
		}
	}

	protected override float GetValFromObject ()
	{
		if (canvasGroup != null)	return canvasGroup.alpha;
		else if (text != null)		return text.color.a;
		else if (graphic != null)	return graphic.color.a;
		return base.GetValFromObject();
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenRatioType.Start<UITweenAlpha>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenRatioType.Start<UITweenAlpha>(obj, end, param);
	}
}
