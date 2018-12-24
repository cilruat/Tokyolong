using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISurpriseStart : MonoBehaviour {

	public void PrevSet()
	{
		UIManager.Instance.PlayMusic (UIManager.Instance.clipSurprise, 3f);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
	}

	public void OnStart()
	{
		Info.SURPRISE_STEP = 0;
		Info.GameDiscountWon = (short)EDiscount.e500won;
		OnClose ();

		int randGame = Random.Range (0, Info.TotalGameCountWithTokyoLive ());
		if (randGame == Info.TotalGameCountWithTokyoLive () - 1) {
			GameObject obj = UIManager.Instance.Show (eUI.eTokyoLive);
			PageTokyoLive ui = obj.GetComponent<PageTokyoLive>();
			ui.PrevSet();
		} else
			Info.PlayGame (randGame, gameObject);
	}		

	public void OnClose()
	{		
		StartCoroutine (_OnClose ());
	}

	IEnumerator _OnClose()
	{
		UIManager.Instance.MuteMusic ();
		UITweenAlpha.Start (gameObject, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		yield return new WaitForSeconds (.8f);

		UIManager.Instance.Hide (eUI.eSurpriseStart);
	}		
}
