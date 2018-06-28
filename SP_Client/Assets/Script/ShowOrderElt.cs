using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOrderElt : MonoBehaviour {

	public Text textTableNo;
	public Text textOrderNo;

	CanvasGroup cvGroup;
	Coroutine show_routine = null;

	int _order_num = -1;

	void Awake()
	{
		cvGroup = GetComponent<CanvasGroup> ();
	}		

	public void RefreshInfo(int orderNum, int tableNo)
	{
		_order_num = orderNum;
		textTableNo.text = "TABLE <size='50'>" + tableNo.ToString () + "</size>";

		if (show_routine != null) 
		{
			StopCoroutine (show_routine);
			show_routine = null;
		}

		show_routine = StartCoroutine (_show ());
	}		

	IEnumerator _show()
	{
		UITweenAlpha.Start(gameObject, 1f, TWParam.New (.5f).Curve (TWCurve.CurveLevel2));

		yield return new WaitForSeconds (10f);

		PageShowOrder.Instance.RemoveOrder (_order_num);
		show_routine = null;
	}		

	public bool CheckAlpha() { return cvGroup.alpha > 0f; }
	public void SetCanvasGroupAlpha(float val) { cvGroup.alpha = val; }
	public void SetPosition(Vector3 position) {	transform.localPosition = position; }
	public int GetOrderNum() { return _order_num; }
}
