using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageBase : SingletonMonobehaviour<PageBase> {

	protected virtual void Awake()
	{
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2));
	}
}
