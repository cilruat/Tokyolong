using UnityEngine;
using System.Collections;

public class UITweenPosX : UITweenFloatType
{
	protected override void SetVal (float ratio)
	{
		if (rectTrans != null)	rectTrans.anchoredPosition = new Vector2(GetVal(ratio), rectTrans.anchoredPosition.y);
		else if(trans != null)	trans.localPosition = new Vector3(GetVal(ratio), trans.localPosition.y, trans.localPosition.z);
	}

	protected override float GetValFromObject ()
	{
		if (rectTrans != null)	return rectTrans.anchoredPosition.x;
		else if(trans != null)	return trans.localPosition.x;
		return 0;
	}

	public static UITween Start (GameObject obj, float start, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosX>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, float end, TWParam param)
	{
		return UITweenFloatType.Start<UITweenPosX>(obj, end, param);
	}
}
