using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UITweenCustom : UITweenRatioType
{
	[System.Serializable]
	public class TweenableEvent : UnityEvent<float> {}

	[SerializeField]
	public TweenableEvent updateFunc;

	protected override void SetVal (float ratio)
	{
		if (updateFunc != null)
			updateFunc.Invoke(Mathf.LerpUnclamped(start, end, ratio));
	}
}
