using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class UIDiscountAnimation : MonoBehaviour {

	public Text txtTotal;
	public Text txtDiscount;
	public Text txtCalc;

	int curDiscount = 0;
	int curCalc = 0;
	UITween uiScale = null;
	UITween uiColor = null;

	void _init()
	{
		txtTotal.text = Info.MakeMoneyString (0);
		txtDiscount.text = Info.MakeMoneyString (0);
		txtCalc.text = Info.MakeMoneyString (0);

		curDiscount = 0;
		curCalc = 0;

		txtCalc.transform.localScale = Vector3.one;
		txtCalc.color = Info.HexToColor ("FF5C00");

		_StopTween ();
		uiScale = null;
		uiColor = null;
	}

	public void SendREQ()
	{		
		NetworkManager.Instance.Order_Detail_REQ();
	}

	public void SetInfo(string menuPacking, int discountPrice)
	{
		_init ();
		UIManager.Instance.PlayMusic (UIManager.Instance.clipMagnificent);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));

		JsonData json = JsonMapper.ToObject (menuPacking);

		int total = 0;
		for (int i = 0; i < json.Count; i++) 
		{
			int menu = int.Parse(json[i]["menu"].ToString());
			int cnt = int.Parse(json[i]["cnt"].ToString());

			MenuData data = MenuData.Get (menu);
			if (data == null)
				continue;

			int price = data.price * cnt;
			total += price;
		}

		curDiscount = Mathf.Min (total, discountPrice);
		curCalc = Mathf.Max (0, total - discountPrice);

		int discountApply = 0;
		switch ((EDiscount)Info.GameDiscountWon) {
		case EDiscount.e500won:		discountApply = 500;		break;
		case EDiscount.e1000won:	discountApply = 1000;		break;
		case EDiscount.e2000won:	discountApply = 2000;		break;
		case EDiscount.e5000won:	discountApply = 5000;		break;
		case EDiscount.eAll:		discountApply = total;		break;
		}

		int prevDiscount = Mathf.Max (0, curDiscount - discountApply);
		int prevCalc = Mathf.Min (curCalc + discountApply, total);

		txtTotal.text = Info.MakeMoneyString (total);
		StartCoroutine (_Animating (prevDiscount, prevCalc));
	}

	public void TestAnimating()
	{
		_init ();
		UIManager.Instance.PlayMusic (UIManager.Instance.clipMagnificent);
		UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));

		Info.GameDiscountWon = 4;

		int total = 5000;
		int discountPrice = 6000;

		curDiscount = Mathf.Min (total, discountPrice);
		curCalc = Mathf.Max (0, total - discountPrice);

		int discountApply = 0;
		switch ((EDiscount)Info.GameDiscountWon) {
		case EDiscount.e500won:		discountApply = 500;		break;
		case EDiscount.e1000won:	discountApply = 1000;		break;
		case EDiscount.e2000won:	discountApply = 2000;		break;
		case EDiscount.e5000won:	discountApply = 5000;		break;
		case EDiscount.eAll:		discountApply = total;		break;
		}

		int prevDiscount = Mathf.Max (0, curDiscount - discountApply);
		int prevCalc = Mathf.Min (curCalc + discountApply, total);

		txtTotal.text = Info.MakeMoneyString (total);
		StartCoroutine (_Animating (prevDiscount, prevCalc));
	}

	IEnumerator _Animating(int prevDiscount, int prevCalc)
	{
		txtCalc.text = Info.MakeMoneyString (prevCalc);
		txtDiscount.text = Info.MakeMoneyString (prevDiscount);

		yield return new WaitForSeconds (1f);
		uiScale = UITweenScale.Start (txtCalc.gameObject, 1f, 1.05f, TWParam.New (.5f).Loop(TWLoop.PingPong).Curve (TWCurve.Shake));

		float limitTime = 3f;

		float value1 = curDiscount;
		float prevValue1 = prevDiscount;
		float distance1 = Mathf.Abs (prevValue1 - value1);

		float value2 = curCalc;
		float prevValue2 = prevCalc;
		float distance2 = Mathf.Abs (prevValue2 - value2);

		while (true) {
			float dis1 = distance1 * (Time.deltaTime / limitTime);
			prevValue1 = _GetMovedValue (prevValue1, value1, dis1);
			txtDiscount.text = Info.MakeMoneyString ((int)prevValue1);

			float dis2 = distance2 * (Time.deltaTime / limitTime);
			prevValue2 = _GetMovedValue (prevValue2, value2, dis2);
			txtCalc.text = Info.MakeMoneyString ((int)prevValue2);

			if (limitTime < 1f)
				break;

			limitTime -= Time.deltaTime;
			yield return null;
		}

		if (uiScale)
			uiScale.StopTween ();

		yield return new WaitForSeconds (.2f);
		uiColor = UITweenColor.Start (txtCalc.gameObject, Color.white, Color.red, TWParam.New (.3f).Loop(TWLoop.PingPong).Curve (TWCurve.Linear));

		txtCalc.text = Info.MakeMoneyString (curCalc);
		txtDiscount.text = Info.MakeMoneyString (curDiscount);

		Info.GameDiscountWon = -1;

		yield return new WaitForSeconds (1f);

		if (uiColor)
			uiColor.StopTween ();

		UIManager.Instance.MuteMusic ();
		UITweenAlpha.Start (gameObject, 1f, 0f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));
		yield return new WaitForSeconds (.8f);

		UIManager.Instance.Hide (eUI.eDiscountAni);
		Info.AfterDiscountBehavior ();
	}

	float _GetMovedValue(float start, float end, float distance)
	{
		if (start == end)
			return end;

		float value = Mathf.Abs (start - end);
		if (value < distance)
			return end;

		if (start < end)
			return start + distance;

		return start - distance;
	}

	void _StopTween()
	{
		if (uiScale)	uiScale.StopTween ();
		if (uiColor)	uiColor.StopTween ();
	}
}
