using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminBillConfirm : SingletonMonobehaviour<AdminBillConfirm> {

	public Text table;
	public AdminBill bill;
    public AdminBillConfirmChange billChange;
    public GameObject objComplete;

	byte tableNo = 0;

	public void SetInfo(byte tableNo, List<KeyValuePair<EMenuDetail,int>> list)
	{
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블";
		bill.CopyBill (list);
        billChange.SetTable(tableNo);
	}

	public void OnLogout()
	{
		NetworkManager.Instance.Logout_REQ(tableNo);
	}

    public void MenuChange(EMenuDetail type, int value, int oriValue)
    {
        bill.CalcTotalPrice();
        billChange.SetMenu(type, value, oriValue);
    }

    [System.NonSerialized]public bool waitComplete = false;
    public void OnCompleteTableOrderInput()
    {
        UITweenAlpha.Start (objComplete.gameObject, 0f, 1f, TWParam.New (.4f).Curve (TWCurve.CurveLevel2));
        UITweenScale.Start (objComplete.gameObject, 1.2f, 1f, TWParam.New (.3f).Curve (TWCurve.Bounce));
        StartCoroutine(_DelayComplete());
    }

    IEnumerator _DelayComplete()
    {
        yield return new WaitForSeconds (1f);

        waitComplete = false;
        objComplete.SetActive(false);
        OnClose();
    }

    public void OnClose() 
    {
        if (waitComplete)
            return;

        gameObject.SetActive (false); 
    }
}
