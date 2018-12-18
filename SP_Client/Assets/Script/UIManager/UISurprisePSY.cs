using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISurprisePSY : MonoBehaviour {

	static short PROGRESS = -1;

	public SpriteRenderer sprite;
	public Text txtDiscount;

	static public void Init()
	{
		PROGRESS = -1;
	}		

	public void PrevSet()
	{
		++PROGRESS;
		if (PROGRESS > 4)
			PROGRESS = 4;

		short discount = 0;
		switch (PROGRESS) {
		case 0:		discount = 500;		break;
		case 1:		discount = 1000;	break;
		case 2:		discount = 2000;	break;
		case 3:		discount = 5000;	break;
		}

		txtDiscount.text = discount == 4 ? "전액" : discount.ToString () + "￦";

		StartCoroutine (_OnShow (true));
		UIManager.Instance.PlayMusic (UIManager.Instance.clipSurprise, 3f);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
	}

	public void OnStart()
	{
		Info.GameDiscountWon = PROGRESS;
		OnClose ();

		int randGame = Random.Range (0, Info.TotalGameCount ());
		Info.PlayGame (randGame, gameObject);
	}		

	public void OnClose()
	{		
		StartCoroutine (_OnClose ());
	}

	IEnumerator _OnClose()
	{
		StartCoroutine (_OnShow (false));

		UIManager.Instance.MuteMusic ();
		UITweenAlpha.Start (gameObject, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		yield return new WaitForSeconds (.8f);

		UIManager.Instance.Hide (eUI.eSurprise);
	}

	IEnumerator _OnShow(bool show)
	{
		float alpha = show ? 0f : 1f;
		while (true) {
			alpha += show ? Time.deltaTime : -Time.deltaTime;
			sprite.color = new Color (1f, 1f, 1f, alpha);

			if (show) {
				if (alpha >= 1f)	break;
			} else {
				if (alpha <= 0f)	break;
			}

			yield return null;
		}
	}
}
