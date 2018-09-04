using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class SendMenu
{
	public int menu = 0;
	public int cnt = 0;

    public SendMenu(int menu, int cnt)
    {
        this.menu = menu;
        this.cnt = cnt;
    }
}

public class Bill : MonoBehaviour {
    
	public GameObject prefab;
	public GameObject objEmpty;
	public RectTransform rtScroll;
    public GridLayoutGroup layout;
	public Text totalPrice;
    public Text textDiscountPrice;
    public Text textCalcPrice;

	List<BillElt> listElt = new List<BillElt>();

    int billTotalPrice = 0;
    int billTotalDiscount = 0;

    int discountPrice = 0;
    public int BillTotalPrice { get { return Mathf.Max(0, (billTotalPrice - billTotalDiscount)); } }

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

        ResizeScroll();
	}

	public void CalcTotalPrice()
	{
		int total = 0;
		for (int i = 0; i < listElt.Count; i++)
			total += listElt [i].GetPrice ();
        
        billTotalPrice = total;

        totalPrice.text = Info.MakeMoneyString (billTotalPrice);

        billTotalDiscount = Mathf.Min(billTotalPrice, discountPrice + (Info.GamePlayCnt * 100));

        if(textDiscountPrice != null)
            textDiscountPrice.text = "-"+Info.MakeMoneyString(billTotalDiscount);

        if(textCalcPrice != null)
            textCalcPrice.text = Info.MakeMoneyString(BillTotalPrice);
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
        ResizeScroll();
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

        discountPrice = 0;

		CalcTotalPrice ();
        ResizeScroll();
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
        ResizeScroll();
	}

	public void CopyBill(List<KeyValuePair<EMenuDetail, int>> list, int discountPrice)
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

		this.discountPrice = discountPrice;

		CalcTotalPrice ();
        ResizeScroll();
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
        int orderCnt = 0;
		List<SendMenu> list = new List<SendMenu> ();
		for (int i = 0; i < listElt.Count; i++) {
			int menu = (int)listElt [i].MenuDetailType ();
			int cnt = listElt [i].GetCount ();
			SendMenu send = new SendMenu (menu, cnt);
			list.Add (send);

			int dcChanceCnt = Info.GetGameCntByMenuType (listElt [i].MenuType ());
            orderCnt += (dcChanceCnt * cnt);
        }

		JsonData json = JsonMapper.ToJson (list);
        NetworkManager.Instance.Order_REQ (json.ToString (), orderCnt);

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
        SceneChanger.LoadScene ("Main", PageBase.Instance.curBoardObj ());
	}

    void ResizeScroll()
    {
        float height = layout.cellSize.y * (float)listElt.Count;
        height += layout.spacing.y * (float)Mathf.Max((listElt.Count - 1), 0);
        height += layout.padding.top + layout.padding.bottom;

        if (height != rtScroll.rect.height)
        {
            rtScroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            float parentHeight = rtScroll.parent.GetComponent<RectTransform>().rect.height;
            rtScroll.anchoredPosition = new Vector2(0f, Mathf.Max((height - parentHeight), 0f));
        }
    }
}
