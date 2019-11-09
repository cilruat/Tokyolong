using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenScaleEx : UITweenVector3Type
{
	protected override void SetVal (float ratio)
	{
		if (trans != null)
			trans.localScale = GetVal(ratio);
	}

	protected override Vector3 GetValFromObject ()
	{
		return trans.localScale;
	}

	public static UITween Start (GameObject obj, Vector3 start, Vector3 end, TWParam param)
	{
		return UITweenVector3Type.Start<UITweenScaleEx>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, Vector3 end, TWParam param)
	{
		return UITweenVector3Type.Start<UITweenScaleEx>(obj, end, param);
	}
}