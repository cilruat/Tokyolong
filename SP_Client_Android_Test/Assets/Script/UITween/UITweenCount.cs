using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenCount : UITweenFloatType
{	
	public enum Type
	{
		Count,
		Second,
		MilliSecond,
		CountMoney
	}
	
	public Type type = Type.Count;
	public string format;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void SetVal (float ratio)
	{
		if (text == null)
			return;
		
		float val = GetVal(ratio);
		
		string str = string.Empty;
		switch (type)
		{
			case Type.Count:		str = ((int)val).ToString(); break;
			case Type.Second:		str = STR.TIME_MMSS((int)val); break;
			case Type.MilliSecond:	str = STR.TIME_MMSSMM(val, text.fontSize); break;
			case Type.CountMoney:	str = val > 0 ? ((int)val).ToString("#,#") : ((int)val).ToString(); break;
		}

		str = string.IsNullOrEmpty(format) == false ? string.Format(format, str) : str;
		text.text = str;
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param, Type type = Type.Count, string format = "{0}")
	{
		UITweenCount tween = UITweenFloatType.Start<UITweenCount>(obj, start, end, param);
		tween.type = type;
		tween.format = format;
		return tween;
	}

	public static UITween Start (GameObject obj, float end, TWParam param, Type type = Type.Count, string format = "{0}")
	{
		UITweenCount tween = UITweenFloatType.Start<UITweenCount>(obj, end, param);
		tween.type = type;
		tween.format = format;
		return tween;
	}
}