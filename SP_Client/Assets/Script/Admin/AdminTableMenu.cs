using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminTableMenu : SingletonMonobehaviour<AdminTableMenu> {

	public Text table;

	byte tableNo = 0;
	public void SetInfo (byte tableNo)
	{
		this.tableNo = tableNo;
		table.text = tableNo.ToString () + "번 테이블";
	}

	public void OnCallConfirm()
	{
		PageAdmin.Instance.StopUrgency (tableNo);
		OnClose ();
	}

	public void OnBillConfirm()
	{
        NetworkManager.Instance.Table_Order_Confirm_REQ(tableNo);
	}

    public void OnTableOrderInput()
    {
        PageAdmin.Instance.ShowTableOrderInput(tableNo);
        OnClose();
    }

    public void OnTableDiscountInput()
    {
        NetworkManager.Instance.TablePriceConfirm_REQ(tableNo);
        OnClose();
    }

	public void OnClose() {	gameObject.SetActive (false); }
}