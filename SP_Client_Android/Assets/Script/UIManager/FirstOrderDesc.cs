using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstOrderDesc : MonoBehaviour {

	public float delayTime = 0f;

	void Awake()
	{
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (.75f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
	}

	public void Show()
	{
		StartCoroutine (_DelayTokyoLive ());
	}

	IEnumerator _DelayTokyoLive()
	{
		yield return new WaitForSeconds (delayTime);

		GameObject obj = UIManager.Instance.Show (eUI.eTokyoLive);
		PageTokyoLive ui = obj.GetComponent<PageTokyoLive>();
		ui.PrevSet();

		UIManager.Instance.Hide (eUI.eFirstOrderDesc);
	}
}