using UnityEngine;
using System.Collections;

public class UITweenColor : UITweenColorType
{
	protected override void SetVal (float ratio)
	{
		if (text != null)			text.color = GetVal(ratio);
		else if (graphic != null)	graphic.color = GetVal(ratio);
	}

	protected override Color GetValFromObject ()
	{
		if (text != null)			return text.color;
		else if (graphic != null)	return graphic.color;
		return base.GetValFromObject();
	}

	public static UITween Start (GameObject obj, Color start, Color end, TWParam param)
	{
		return UITweenColorType.Start<UITweenColor>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, Color end, TWParam param)
	{
		return UITweenColorType.Start<UITweenColor>(obj, end, param);
	}
}
