using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminBillConfirm : SingletonMonobehaviour<AdminBillConfirm> {

	public Text table;
    public Text tableExtraGameCnt;
	public AdminBill bill;
    public AdminBillConfirmChange billChange;
    public GameObject objComplete;
	public GameObject objCompleteDesc;

	byte tableNo = 0;

    public void SetInfo(byte tableNo, List<KeyValuePair<EMenuDetail,int>> list, int discount, int extraGameCnt)
	{
		this.tableNo = tableNo;
		waitComplete = false;
		objComplete.SetActive(false);

		table.text = tableNo.ToString () + "번 테이블";
        tableExtraGameCnt.text = "남은 할인 찬스 : <color='#70ad47'><size=23>" + extraGameCnt.ToString() + "</size></color>";
        bill.CopyBill (list, discount, extraGameCnt);
        billChange.SetTable(tableNo);
	}

	public void OnLogout()
	{
		if (waitComplete)
			return;

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
		objComplete.SetActive(true);
		UITweenAlpha.Start (objCompleteDesc.gameObject, 0f, 1f, TWParam.New (.4f).Curve (TWCurve.CurveLevel2));
		UITweenScale.Start (objCompleteDesc.gameObject, 1.2f, 1f, TWParam.New (.3f).Curve (TWCurve.Bounce));
        StartCoroutine(_DelayComplete());
    }

    IEnumerator _DelayComplete()
    {
        yield return new WaitForSeconds (1f);
		        
        objComplete.SetActive(false);
        OnClose();
		waitComplete = false;
    }

    public void OnClose() 
    {
        if (waitComplete)
            return;

        gameObject.SetActive (false); 
    }
}
