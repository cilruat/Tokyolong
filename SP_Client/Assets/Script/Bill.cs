using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class Bill : MonoBehaviour {

	class SendMenu
	{
		public int menu;
		public int cnt;

		public SendMenu(int menu, int cnt)
		{
			this.menu = menu;
			this.cnt = cnt;
		}
	}

	public GameObject prefab;
	public GameObject objEmpty;
	public RectTransform rtScroll; 
	public Text totalPrice;

	List<BillElt> listElt = new List<BillElt>();

    int billTotalPrice = 0;
    public int BillTotalPrice { get { return billTotalPrice; } }

	public void SetMenu(EMenuDetail eType)
	{
		int findIdx = -1;
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].MenuDetailType () != eType)
				continue;

			findIdx = i;
			break;
		}

		if (findIdx != -1) {
			listElt [findIdx].OnChangeValue (true);
		} else {
			GameObject obj = Instantiate (prefab) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScroll);
			tr.InitTransform ();

			BillElt elt = obj.GetComponent<BillElt> ();
			elt.SetInfo (eType);
			listElt.Add (elt);

			CalcTotalPrice ();
		}			

		if (objEmpty != null && objEmpty.activeSelf)
			objEmpty.SetActive (false);
	}

	public void CalcTotalPrice()
	{
		int total = 0;
		for (int i = 0; i < listElt.Count; i++)
			total += listElt [i].GetPrice ();

        billTotalPrice = total;
		totalPrice.text = Info.MakeMoneyString (total);
	}

	public void RemoveElt(EMenuDetail eType)
	{
		int findIdx = -1;
		for (int i = 0; i < listElt.Count; i++) {
			if (listElt [i].MenuDetailType () != eType)
				continue;

			findIdx = i;
			listElt.RemoveAt (i);
			break;
		}

		if (findIdx == -1)
			return;

		for (int i = 0; i < rtScroll.childCount; i++) {
			if (i != findIdx)
				continue;

			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
			break;
		}

		CalcTotalPrice ();
	}

	void _Clear()
	{
		for (int i = 0; i < rtScroll.childCount; i++) {
			Transform child = rtScroll.GetChild (i);
			if (child)
				Destroy (child.gameObject);
		}

		listElt.Clear ();

		if (objEmpty != null && objEmpty.activeSelf == false)
			objEmpty.SetActive (true);

		CalcTotalPrice ();
	}

	public void CopyBill(List<BillElt> list)
	{
		_Clear ();

		for (int i = 0; i < list.Count; i++) {
			GameObject obj = Instantiate (prefab) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScroll);
			tr.InitTransform ();

			BillElt elt = obj.GetComponent<BillElt> ();
			elt.SetInfo (list [i].MenuDetailType (), list [i].GetCount ());
			listElt.Add (elt);
		}

        if (objEmpty != null)
            objEmpty.SetActive(list.Count <= 0);
        
		CalcTotalPrice ();
	}

	public void CopyBill(List<KeyValuePair<EMenuDetail, int>> list)
	{
		_Clear ();

		for (int i = 0; i < list.Count; i++) {
			GameObject obj = Instantiate (prefab) as GameObject;
			obj.SetActive (true);

			Transform tr = obj.transform;
			tr.SetParent (rtScroll);
			tr.InitTransform ();

			BillElt elt = obj.GetComponent<BillElt> ();
			elt.SetInfo (list [i].Key, list [i].Value);
			listElt.Add (elt);
		}

        if (objEmpty != null)
            objEmpty.SetActive(list.Count <= 0);

		CalcTotalPrice ();
	}

	public void OnOrder()
	{
		if (listElt.Count == 0) {
			SystemMessage.Instance.Add ("주문내역이 없습니다");
			return;
		}

		GameObject obj = UIManager.Instance.Show (eUI.eBillSending);
		Bill sendBill = obj.GetComponentInChildren<Bill> ();
		sendBill.CopyBill (listElt);

		CountDown countDown = obj.GetComponentInChildren<CountDown> ();
		countDown.Set (3, () => FinishOrder ());
	}

	public void FinishOrder()
	{
		List<SendMenu> list = new List<SendMenu> ();
		for (int i = 0; i < listElt.Count; i++) {
			int menu = (int)listElt [i].MenuDetailType ();
			int cnt = listElt [i].GetCount ();
			SendMenu send = new SendMenu (menu, cnt);
			list.Add (send);
		}

		JsonData json = JsonMapper.ToJson (list);
		NetworkManager.Instance.Order_REQ (json.ToString ());

		_Clear ();
	}

	public void CompleteOrder()
	{
		_OrderState (false);
		StartCoroutine (_DelayedBillSending ());
	}

	void _OrderState(bool complete)
	{
		GameObject obj = UIManager.Instance.GetCurUI ();
		if (obj == null)
			return;

		Transform child = obj.transform.Find ("BtnCancle");
		child.gameObject.SetActive (complete ? true : false);

		child = obj.transform.Find ("DescComplete");
		if (complete)
			child.gameObject.SetActive (false);
		else {
			UITweenAlpha.Start (child.gameObject, 0f, 1f, TWParam.New (.4f).Curve (TWCurve.CurveLevel2));
			UITweenScale.Start (child.gameObject, 1.2f, 1f, TWParam.New (.3f).Curve (TWCurve.Bounce));
		}
	}

	IEnumerator _DelayedBillSending()
	{
		yield return new WaitForSeconds (.8f);

		_OrderState (true);
		UIManager.Instance.Hide (eUI.eBillSending);
	}
}
