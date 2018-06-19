using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bill : SingletonMonobehaviour<Bill> {

	public GameObject prefab;
	public GameObject objEmpty;
	public RectTransform rtScroll; 
	public Text totalPrice;

	List<BillElt> listElt = new List<BillElt>();

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

		if (objEmpty.activeSelf)
			objEmpty.SetActive (false);
	}

	public void CalcTotalPrice()
	{
		int total = 0;
		for (int i = 0; i < listElt.Count; i++)
			total += listElt [i].GetPrice ();

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

		if (objEmpty.activeSelf == false)
			objEmpty.SetActive (true);
	}

	public void OnOrder()
	{
		UIManager.Instance.Show (eUI.eBillSending);
	}

	public void FinishOrder()
	{
		NetworkManager.Instance.Order_REQ ("");
		_Clear ();
	}
}
