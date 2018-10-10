using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class UIDiscountAnimation : MonoBehaviour {

	public Text txtTotal;
	public Text txtDiscount;
	public Text txtCalc;

	int afterDiscount = 0;
	int afterCalc = 0;
	UITween tween = null;

	public void SendREQ()
	{		
		NetworkManager.Instance.Order_Detail_REQ();
	}

	public void SetInfo(string meuPacking, int discountPrice)
	{
		tween = UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));

		JsonData json = JsonMapper.ToObject (meuPacking);

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

		afterDiscount = Mathf.Min (total, discountPrice + (Info.GamePlayCnt * 100));
		afterCalc = Mathf.Max (0, total - discountPrice);

		int discountApply = 0;
		switch ((EDiscount)Info.GameDiscountWon) {
		case EDiscount.e1000won:	discountApply = 1000;		break;
		case EDiscount.e5000won:	discountApply = 5000;		break;
		case EDiscount.eHalf:		discountApply = 1000;		break;
		case EDiscount.eAll:		discountApply = 1000;		break;
		}

		int prevDiscount = Mathf.Max (0, afterDiscount - discountApply);
		int prevCalc = afterCalc + discountApply;

		txtTotal.text = Info.MakeMoneyString (total);

		StartCoroutine (_Animating (prevDiscount, prevCalc));
	}

	public void TestAnimating()
	{
		UIManager.Instance.PlayMusic (UIManager.Instance.clipMagnificent);
		tween = UITweenAlpha.Start (gameObject, 0f, 1f, TWParam.New (1f).Curve (TWCurve.CurveLevel2).Speed (TWSpeed.Slower));

		Info.GameDiscountWon = 1;

		int total = 40000;
		int discountPrice = 5000;

		afterDiscount = Mathf.Min (total, discountPrice + (Info.GamePlayCnt * 100));
		afterCalc = Mathf.Max (0, total - discountPrice);

		int discountApply = 0;
		switch ((EDiscount)Info.GameDiscountWon) {
		case EDiscount.e1000won:	discountApply = 1000;		break;
		case EDiscount.e5000won:	discountApply = 5000;		break;
		case EDiscount.eHalf:		discountApply = 1000;		break;
		case EDiscount.eAll:		discountApply = 1000;		break;
		}

		int prevDiscount = Mathf.Max (0, afterDiscount - discountApply);
		int prevCalc = afterCalc + discountApply;

		txtTotal.text = Info.MakeMoneyString (total);

		StartCoroutine (_Animating (prevDiscount, prevCalc));
	}

	IEnumerator _Animating(int discount, int calc)
	{
		txtCalc.text = Info.MakeMoneyString (calc);
		txtDiscount.text = Info.MakeMoneyString (discount);

		List<int> listCalc = new List<int> ();
		while (calc != 0) {
			listCalc.Add (calc % 10);
			calc /= 10;
		}

		List<int> listDis = new List<int> ();
		while (discount != 0) {
			listDis.Add (discount % 10);
			discount /= 10;
		}			

		yield return new WaitForSeconds (1f);

		int counting = 0;
		while (true) {
			listCalc [counting] -= 1;
			if (listCalc [counting] == -1)
				listCalc [counting] = 9;

			listDis [counting] += 1;
			if (listDis [counting] == 10)
				listDis [counting] = 0;

			int numCalc = 0;
			for (int i = 0; i < listCalc.Count; i++)
				numCalc += listCalc [i] * (int)Mathf.Pow(10, i);

			int numDis = 0;
			for (int i = 0; i < listDis.Count; i++)
				numDis += listDis [i] * (int)Mathf.Pow(10, i);

			txtCalc.text = Info.MakeMoneyString (numCalc);
			txtDiscount.text = Info.MakeMoneyString (numDis);

			if (numCalc == afterCalc && numDis == afterDiscount)
				break;

			float waitTime = .01f;
			if (counting > 0)
				waitTime += .05f;

			if (listCalc [counting] == 0)
				++counting;

			yield return new WaitForSeconds (waitTime);
		}

		txtCalc.text = Info.MakeMoneyString (afterCalc);
		txtDiscount.text = Info.MakeMoneyString (afterDiscount);

		UIManager.Instance.StopMusic ();
		Info.GameDiscountWon = -1;
	}
}
