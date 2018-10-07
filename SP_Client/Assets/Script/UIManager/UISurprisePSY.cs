using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISurprisePSY : MonoBehaviour {

	public SpriteRenderer sprite;
	public Text txtDiscount;

	int discount = 0;

	public void PrevSet()
	{
		/*float randDiscount = Random.Range (0f, 1f);
		if (randDiscount <= .9f)	discount = 1000;
		else						discount = 2000;

		Debug.Log ("discount: " + discount + ", rand: " + randDiscount);*/

		discount = 1000;
		Info.GameDiscountWon = discount == 1000 ? (short)EDiscount.e1000won : (short)EDiscount.e2000won;
		txtDiscount.text = discount.ToString () + "￦";

		StartCoroutine (_OnShow (true));
		UIManager.Instance.PlayMusic (UIManager.Instance.clipSurprise, 3f);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
	}

	public void OnStart()
	{
		int range = discount == 1000 ? Info.TotalGameCount () : 4;
		int randGame = Random.Range (0, range);

		_GoGame (randGame);
	}

	void _GoGame(int idx)
	{
		OnClose ();
		Info.PlayGame (idx, gameObject);
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
