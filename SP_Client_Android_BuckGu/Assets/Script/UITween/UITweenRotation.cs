using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITweenRotation : UITweenVector3Type
{
	protected override void SetVal (float ratio)
	{
		trans.localEulerAngles = GetVal(ratio);
	}

	protected override Vector3 GetValFromObject ()
	{
		return trans.localEulerAngles;
	}

	public static UITween Start (GameObject obj, Vector3 start, Vector3 end, TWParam param)
	{
		return UITweenVector3Type.Start<UITweenRotation>(obj, start, end, param);
	}

	public static UITween Start (GameObject obj, Vector3 end, TWParam param)
	{
		return UITweenVector3Type.Start<UITweenRotation>(obj, end, param);
	}
}